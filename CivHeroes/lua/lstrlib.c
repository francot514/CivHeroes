/*
** $Id: lstrlib.c,v 1.176 2012/05/23 15:37:09 roberto Exp $
** Standard library for string operations and pattern-matching
** See Copyright Notice in lua.h
*/


#include <ctype.h>
#include <stddef.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define lstrlib_c
#define LUA_LIB

#include "lua.h"

#include "lauxlib.h"
#include "lualib.h"

#ifdef UTF8
#include "utf8.h"
#endif

/*
** maximum number of captures that a pattern can do during
** pattern-matching. This limit is arbitrary.
*/
#if !defined(LUA_MAXCAPTURES)
#define LUA_MAXCAPTURES		32
#endif


/* macro to `unsign' a character */
#define uchar(c)	((unsigned char)(c))


#ifdef UTF8
/*
** Calcurate length of UTF-8 string.
** Parameter `str' is a pointer of raw string.
** Parameter `len' is a length of raw string.
** Parameter `result' is a output length of calcurated UTF-8 string.
*/
#define UTF8_STR_LEN(str, len, result) \
  { \
    int i, n; \
    for (i = 0, n = 0; i < len; n++) \
      i += UTF8_CHAR_LENGTH(str[i]); \
    result = n; \
  }

/*
** Calcurate length of UTF-8 string.
** Parameter `str' is a pointer of raw string.
** Parameter `len' is a length of raw string.
** Parameter `result' is a output length of calcurated UTF-8 string.
** This function returns the length of calcurated UTF-8 string
*/
int utf8_str_len(const char *str, size_t len) {
  int result;
  UTF8_STR_LEN(str, len, result);
  return result;
}
#endif

static int str_len (lua_State *L) {
  size_t l;
#ifdef UTF8
  const char *s = luaL_checklstring(L, 1, &l);
  UTF8_STR_LEN(s, l, l); /* update byte length to character length */
#else
  luaL_checklstring(L, 1, &l);
#endif
  lua_pushinteger(L, (lua_Integer)l);
  return 1;
}


/* translate a relative string position: negative means back from end */
static size_t posrelat (ptrdiff_t pos, size_t len) {
  if (pos >= 0) return (size_t)pos;
  else if (0u - (size_t)pos > len) return 0;
  else return len - ((size_t)-pos) + 1;
}


static int str_sub (lua_State *L) {
  size_t l;
  const char *s = luaL_checklstring(L, 1, &l);
#ifdef UTF8
  size_t start;
  size_t end;
  UTF8_STR_LEN(s, l, l); /* update byte length to character length */
  start = posrelat(luaL_checkinteger(L, 2), l);
  end = posrelat(luaL_optinteger(L, 3, -1), l);
#else
  size_t start = posrelat(luaL_checkinteger(L, 2), l);
  size_t end = posrelat(luaL_optinteger(L, 3, -1), l);
#endif
  if (start < 1) start = 1;
  if (end > l) end = l;
#ifdef UTF8
  if (start <= end) {
    int i, pos, st, ed;
    for (i = 1, pos = 0, st = 0; i < start; i++) {
      int len = UTF8_CHAR_LENGTH(s[pos]);
      st += len;
      pos += len;
    }
    for (ed = st; i < end; i++) {
      int len = UTF8_CHAR_LENGTH(s[pos]);
      ed += len;
      pos += len;
    }
    lua_pushlstring(L, s + st, ed - st + UTF8_CHAR_LENGTH(s[st]));
  }
#else
  if (start <= end)
    lua_pushlstring(L, s + start - 1, end - start + 1);
#endif
  else lua_pushliteral(L, "");
  return 1;
}


static int str_reverse (lua_State *L) {
  size_t l, i;
  luaL_Buffer b;
  const char *s = luaL_checklstring(L, 1, &l);
  char *p = luaL_buffinitsize(L, &b, l);
#ifdef UTF8
  int j;
#endif
  for (i = 0; i < l; i++)
    p[i] = s[l - i - 1];
#ifdef UTF8
  for (j = l - 1; j >= 0;) {
    int len = UTF8_CHAR_LENGTH(p[j]);
    if (len == 2) {
      char c = p[j];
      p[j] = p[j - 1];
      p[j - 1] = c;
    } else if (len == 3) {
      char c = p[j];
      p[j] = p[j - 2];
      p[j - 2] = c;
    } else if (len == 4) {
      char c = p[j];
      p[j] = p[j - 3];
      p[j - 3] = c;
      c = p[j - 1];
      p[j - 1] = p[j - 2];
      p[j - 2] = c;
    }
    j -= len;
  }
#endif
  luaL_pushresultsize(&b, l);
  return 1;
}


static int str_lower (lua_State *L) {
  size_t l;
  size_t i;
  luaL_Buffer b;
  const char *s = luaL_checklstring(L, 1, &l);
  char *p = luaL_buffinitsize(L, &b, l);
  for (i=0; i<l; i++)
    p[i] = tolower(uchar(s[i]));
  luaL_pushresultsize(&b, l);
  return 1;
}


static int str_upper (lua_State *L) {
  size_t l;
  size_t i;
  luaL_Buffer b;
  const char *s = luaL_checklstring(L, 1, &l);
  char *p = luaL_buffinitsize(L, &b, l);
  for (i=0; i<l; i++)
    p[i] = toupper(uchar(s[i]));
  luaL_pushresultsize(&b, l);
  return 1;
}


/* reasonable limit to avoid arithmetic overflow */
#define MAXSIZE		((~(size_t)0) >> 1)

static int str_rep (lua_State *L) {
  size_t l, lsep;
  const char *s = luaL_checklstring(L, 1, &l);
  int n = luaL_checkint(L, 2);
  const char *sep = luaL_optlstring(L, 3, "", &lsep);
  if (n <= 0) lua_pushliteral(L, "");
  else if (l + lsep < l || l + lsep >= MAXSIZE / n)  /* may overflow? */
    return luaL_error(L, "resulting string too large");
  else {
    size_t totallen = n * l + (n - 1) * lsep;
    luaL_Buffer b;
    char *p = luaL_buffinitsize(L, &b, totallen);
    while (n-- > 1) {  /* first n-1 copies (followed by separator) */
      memcpy(p, s, l * sizeof(char)); p += l;
      if (lsep > 0) {  /* avoid empty 'memcpy' (may be expensive) */
        memcpy(p, sep, lsep * sizeof(char)); p += lsep;
      }
    }
    memcpy(p, s, l * sizeof(char));  /* last copy (not followed by separator) */
    luaL_pushresultsize(&b, totallen);
  }
  return 1;
}


static int str_byte (lua_State *L) {
  size_t l;
  const char *s = luaL_checklstring(L, 1, &l);
#ifdef UTF8
  size_t posi;
  size_t pose;
  int n, i;
  int off;
  UTF8_STR_LEN(s, l, l); /* update byte length to character length */
  posi = posrelat(luaL_optinteger(L, 2, 1), l);
  pose = posrelat(luaL_optinteger(L, 3, posi), l);
#else
  size_t posi = posrelat(luaL_optinteger(L, 2, 1), l);
  size_t pose = posrelat(luaL_optinteger(L, 3, posi), l);
  int n, i;
#endif
  if (posi < 1) posi = 1;
  if (pose > l) pose = l;
  if (posi > pose) return 0;  /* empty interval; return no values */
  n = (int)(pose -  posi + 1); /* `n' is character length, not byte length */
  if (posi + n <= pose)  /* (size_t -> int) overflow? */
    return luaL_error(L, "string slice too long");
  luaL_checkstack(L, n, "string slice too long");
#ifdef UTF8
  /* count byte length until `posi' */
  off = 0;
  for (i=1; i<posi; i++) {
    off += UTF8_CHAR_LENGTH(s[off]);
  }
  /* push to stack of every UTF-8 code after `posi' */
  for (i=0; i<n; i++) {
    int len = UTF8_CHAR_LENGTH(s[off]);
    int code = UTF8_DECODE(s+off, len);
    lua_pushinteger(L, code);
    off += len;
  }
#else
  for (i=0; i<n; i++)
    lua_pushinteger(L, uchar(s[posi+i-1]));
#endif
  return n;
}


static int str_char (lua_State *L) {
#ifdef UTF8
  int n = lua_gettop(L);  /* number of arguments */
  int i;
  int sz = 0;
  luaL_Buffer b;
  char *p;
  for (i=1; i<=n; i++) {
    int c = luaL_checkint(L, i);
    sz += UTF8_CODE_LENGTH(c);
  }
  p = luaL_buffinitsize(L, &b, sz);
  for (i=1; i<=n; i++) {
    int c = luaL_checkint(L, i);
    int len = UTF8_CODE_LENGTH(c);
    UTF8_ENCODE(c, len, p);
    p += len;
  }
  luaL_pushresultsize(&b, sz);
  return 1;
#else
  int n = lua_gettop(L);  /* number of arguments */
  int i;
  luaL_Buffer b;
  char *p = luaL_buffinitsize(L, &b, n);
  for (i=1; i<=n; i++) {
    int c = luaL_checkint(L, i);
    luaL_argcheck(L, uchar(c) == c, i, "value out of range");
    p[i - 1] = uchar(c);
  }
  luaL_pushresultsize(&b, n);
  return 1;
#endif
}


static int writer (lua_State *L, const void* b, size_t size, void* B) {
  (void)L;
  luaL_addlstring((luaL_Buffer*) B, (const char *)b, size);
  return 0;
}


static int str_dump (lua_State *L) {
  luaL_Buffer b;
  luaL_checktype(L, 1, LUA_TFUNCTION);
  lua_settop(L, 1);
  luaL_buffinit(L,&b);
  if (lua_dump(L, writer, &b) != 0)
    return luaL_error(L, "unable to dump given function");
  luaL_pushresult(&b);
  return 1;
}



/*
** {======================================================
** PATTERN MATCHING
** =======================================================
*/


#define CAP_UNFINISHED	(-1)
#define CAP_POSITION	(-2)

typedef struct MatchState {
  const char *src_init;  /* init of source string */
  const char *src_end;  /* end ('\0') of source string */
  const char *p_end;  /* end ('\0') of pattern */
  lua_State *L;
  int level;  /* total number of captures (finished or unfinished) */
  struct {
    const char *init;
    ptrdiff_t len;
  } capture[LUA_MAXCAPTURES];
} MatchState;


#define L_ESC		'%'
#define SPECIALS	"^$*+?.([%-"


static int check_capture (MatchState *ms, int l) {
  l -= '1';
  if (l < 0 || l >= ms->level || ms->capture[l].len == CAP_UNFINISHED)
    return luaL_error(ms->L, "invalid capture index %%%d", l + 1);
  return l;
}


static int capture_to_close (MatchState *ms) {
  int level = ms->level;
  for (level--; level>=0; level--)
    if (ms->capture[level].len == CAP_UNFINISHED) return level;
  return luaL_error(ms->L, "invalid pattern capture");
}


static const char *classend (MatchState *ms, const char *p) {
#ifdef UTF8
  const char *prev;
  int len1 = UTF8_CHAR_LENGTH(*p); /* prev char length */
  int len2; /* current char length */
  switch (prev = p, p += len1, len2 = UTF8_CHAR_LENGTH(*p), *prev) {
#else
  switch (*p++) {
#endif
    case L_ESC: {
      if (p == ms->p_end)
        luaL_error(ms->L, "malformed pattern (ends with " LUA_QL("%%") ")");
#ifdef UTF8
      return p+len2;
#else
      return p+1;
#endif
    }
    case '[': {
      if (*p == '^') p++;
      do {  /* look for a `]' */
        if (p == ms->p_end)
          luaL_error(ms->L, "malformed pattern (missing " LUA_QL("]") ")");
#ifdef UTF8
#define NEXT (prev = p, len1 = len2, p += len2, len2 = UTF8_CHAR_LENGTH(*p))
        if (NEXT, *prev == L_ESC && p < ms->p_end)
          NEXT;  /* skip escapes (e.g. `%]') */
#undef NEXT
#else
        if (*(p++) == L_ESC && p < ms->p_end)
          p++;  /* skip escapes (e.g. `%]') */
#endif
      } while (*p != ']');
#ifdef UTF8
      return p+len2;
#else
      return p+1;
#endif
    }
    default: {
      return p;
    }
  }
}


static int match_class (int c, int cl) {
  int res;
  switch (tolower(cl)) {
#ifdef UTF8
    case 'a' : res = c <= 0x7F && isalpha(c); break;
#else
    case 'a' : res = isalpha(c); break;
#endif
    case 'c' : res = iscntrl(c); break;
    case 'd' : res = isdigit(c); break;
#ifdef UTF8
    case 'g' : res = (c <= 0x7F ? isgraph(c) : match_class(c, 'S')); break;
#else
    case 'g' : res = isgraph(c); break;
#endif
    case 'l' : res = islower(c); break;
#ifdef UTF8
    case 'p' : res = ispunct(c) || UTF8_ISPUNCT(c); break;
#else
    case 'p' : res = ispunct(c); break;
#endif
#ifdef UTF8
    case 's' : res = isspace(c) || UTF8_ISSPACE(c); break;
#else
    case 's' : res = isspace(c); break;
#endif
    case 'u' : res = isupper(c); break;
#ifdef UTF8
    case 'w' : res = c <= 0x7F && isalnum(c); break;
#else
    case 'w' : res = isalnum(c); break;
#endif
#ifdef UTF8
    case 'x' : res = c <= 0x7F && isxdigit(c); break;
#else
    case 'x' : res = isxdigit(c); break;
#endif
    case 'z' : res = (c == 0); break;  /* deprecated option */
    default: return (cl == c);
  }
  return (islower(cl) ? res : !res);
}


static int matchbracketclass (int c, const char *p, const char *ec) {
#ifdef UTF8
  int sig = 1;
  if (*(p+1) == '^') {
    sig = 0;
    p++;  /* skip the `^' */
  }
  p++; /* skip the `[' */
  while (p < ec) {
    int len1 = UTF8_CHAR_LENGTH(*p);
    int code1 = UTF8_DECODE(p, len1);
    if (*p == L_ESC) {
      p++;
      len1 = UTF8_CHAR_LENGTH(*p);
      code1 = UTF8_DECODE(p, len1);
      if (match_class(c, code1)) /* TODO: check buffer over run */
        return sig;
    }
    else if ((*(p+len1) == '-') && (p+(len1+1) < ec)) {
      int len2 = UTF8_CHAR_LENGTH(*(p+(len1+1)));
      int code2 = UTF8_DECODE(p+(len1+1), len2);
      p+=len1+1;
      len1 = len2;
      if (code1 <= c && c <= code2)
        return sig;
    }
    else if (code1 == c) return sig;
    p+=len1;
  }
  return !sig;
#else
  int sig = 1;
  if (*(p+1) == '^') {
    sig = 0;
    p++;  /* skip the `^' */
  }
  while (++p < ec) {
    if (*p == L_ESC) {
      p++;
      if (match_class(c, uchar(*p)))
        return sig;
    }
    else if ((*(p+1) == '-') && (p+2 < ec)) {
      p+=2;
      if (uchar(*(p-2)) <= c && c <= uchar(*p))
        return sig;
    }
    else if (uchar(*p) == c) return sig;
  }
  return !sig;
#endif
}


static int singlematch (int c, const char *p, const char *ep) {
  switch (*p) {
    case '.': return 1;  /* matches any char */
#ifdef UTF8
    case L_ESC: return match_class(c, UTF8_DECODE(p+1, UTF8_CHAR_LENGTH(*(p+1))));
#else
    case L_ESC: return match_class(c, uchar(*(p+1)));
#endif
    case '[': return matchbracketclass(c, p, ep-1);
#ifdef UTF8
    default:  return (UTF8_DECODE(p, UTF8_CHAR_LENGTH(*p)) == c);
#else
    default:  return (uchar(*p) == c);
#endif
  }
}


static const char *match (MatchState *ms, const char *s, const char *p);


static const char *matchbalance (MatchState *ms, const char *s,
                                   const char *p) {
#ifdef UTF8
  int ss = UTF8_DECODE(s, UTF8_CHAR_LENGTH(*s));
  int ps = UTF8_DECODE(p, UTF8_CHAR_LENGTH(*p));
#endif
  if (p >= ms->p_end - 1)
    luaL_error(ms->L, "malformed pattern "
                      "(missing arguments to " LUA_QL("%%b") ")");
#ifdef UTF8
  if (ss != ps) return NULL;
#else
  if (*s != *p) return NULL;
#endif
  else {
#ifdef UTF8
    int blen = UTF8_CHAR_LENGTH(*p);
    int b = UTF8_DECODE(p, blen);
    int elen = UTF8_CHAR_LENGTH(p[blen]);
    int e = UTF8_DECODE(p + blen, elen);
    int cont = 1;
    int len;
    while ((s += (len = UTF8_CHAR_LENGTH(*s))) < ms->src_end) {
      int code = UTF8_DECODE(s, len);
      if (code == e) {
        if (--cont == 0) return s+len;
      }
      else if (code == b) cont++;
    }
  }
#else
    int b = *p;
    int e = *(p+1);
    int cont = 1;
    while (++s < ms->src_end) {
      if (*s == e) {
        if (--cont == 0) return s+1;
      }
      else if (*s == b) cont++;
    }
  }
#endif
  return NULL;  /* string ends out of balance */
}


static const char *max_expand (MatchState *ms, const char *s,
                                 const char *p, const char *ep) {
#ifdef UTF8
  int i = 0;
  int len = UTF8_CHAR_LENGTH(*s);
  int index = 0;
  char *memo = (char *)malloc(ms->src_end - ms->src_init + 1);
  while ((s+i)<ms->src_end && singlematch(UTF8_DECODE(s+i, len), p, ep)) {
    i += len;
    memo[index++] = len;
    len = UTF8_CHAR_LENGTH(s[i]);
  }
  if (s[i]) {
    memo[index++] = len;
  }
#else
  ptrdiff_t i = 0;  /* counts maximum expand for item */
  while ((s+i)<ms->src_end && singlematch(uchar(*(s+i)), p, ep))
    i++;
#endif
  /* keeps trying to match with the maximum repetitions */
  while (i>=0) {
    const char *res = match(ms, (s+i), ep+1);
#ifdef UTF8
    if (res) free(memo);
#endif
    if (res) return res;
#ifdef UTF8
    if (index == 0) {
      break;
    }
    i -= memo[--index];
#else
    i--;  /* else didn't match; reduce 1 repetition to try again */
#endif
  }
#ifdef UTF8
  free(memo);
#endif
  return NULL;
}


static const char *min_expand (MatchState *ms, const char *s,
                                 const char *p, const char *ep) {
  for (;;) {
    const char *res = match(ms, s, ep+1);
    if (res != NULL)
      return res;
    else if (s<ms->src_end && singlematch(uchar(*s), p, ep))
      s++;  /* try with one more repetition */
    else return NULL;
  }
}


static const char *start_capture (MatchState *ms, const char *s,
                                    const char *p, int what) {
  const char *res;
  int level = ms->level;
  if (level >= LUA_MAXCAPTURES) luaL_error(ms->L, "too many captures");
  ms->capture[level].init = s;
  ms->capture[level].len = what;
  ms->level = level+1;
  if ((res=match(ms, s, p)) == NULL)  /* match failed? */
    ms->level--;  /* undo capture */
  return res;
}


static const char *end_capture (MatchState *ms, const char *s,
                                  const char *p) {
  int l = capture_to_close(ms);
  const char *res;
  ms->capture[l].len = s - ms->capture[l].init;  /* close capture */
  if ((res = match(ms, s, p)) == NULL)  /* match failed? */
    ms->capture[l].len = CAP_UNFINISHED;  /* undo capture */
  return res;
}


static const char *match_capture (MatchState *ms, const char *s, int l) {
  size_t len;
  l = check_capture(ms, l);
  len = ms->capture[l].len;
  if ((size_t)(ms->src_end-s) >= len &&
      memcmp(ms->capture[l].init, s, len) == 0)
    return s+len;
  else return NULL;
}


static const char *match (MatchState *ms, const char *s, const char *p) {
#ifdef UTF8
  int len;
#endif
  init: /* using goto's to optimize tail recursion */
#ifdef UTF8
  len = UTF8_CHAR_LENGTH(*s);
#endif
  if (p == ms->p_end)  /* end of pattern? */
    return s;  /* match succeeded */
  switch (*p) {
    case '(': {  /* start capture */
      if (*(p+1) == ')')  /* position capture? */
        return start_capture(ms, s, p+2, CAP_POSITION);
      else
        return start_capture(ms, s, p+1, CAP_UNFINISHED);
    }
    case ')': {  /* end capture */
      return end_capture(ms, s, p+1);
    }
    case '$': {
      if ((p+1) == ms->p_end)  /* is the `$' the last char in pattern? */
        return (s == ms->src_end) ? s : NULL;  /* check end of string */
      else goto dflt;
    }
    case L_ESC: {  /* escaped sequences not in the format class[*+?-]? */
      switch (*(p+1)) {
        case 'b': {  /* balanced string? */
          s = matchbalance(ms, s, p+2);
          if (s == NULL) return NULL;
#ifdef UTF8
          {
            int blen = UTF8_CHAR_LENGTH(p[2]);
            int elen = UTF8_CHAR_LENGTH(p[2+blen]);
            p+=2+blen+elen; goto init;
          }
#else
          p+=4; goto init;  /* else return match(ms, s, p+4); */
#endif
        }
        case 'f': {  /* frontier? */
          const char *ep; char previous;
#ifdef UTF8
          const char *ep2;
          int slen;
          const char *previousp;
          int previousc;
          previous = 0; /* restraint compiler warning */
#endif
          p += 2;
          if (*p != '[')
            luaL_error(ms->L, "missing " LUA_QL("[") " after "
                               LUA_QL("%%f") " in pattern");
          ep = classend(ms, p);  /* points to what is next */
#ifdef UTF8
          UTF8_PREV_CHAR_PTR(ep, ep2);
          slen = UTF8_CHAR_LENGTH(*s);
          previousp = (s == ms->src_init) ? NULL : (s-slen);
          previousc = previousp == NULL ? 0 : UTF8_CHAR_LENGTH(*previousp);
          if (matchbracketclass(previousc, p, ep2) ||
             !matchbracketclass(UTF8_DECODE(s, slen), p, ep2)) return NULL;
#else
          previous = (s == ms->src_init) ? '\0' : *(s-1);
          if (matchbracketclass(uchar(previous), p, ep-1) ||
             !matchbracketclass(uchar(*s), p, ep-1)) return NULL;
#endif
          p=ep; goto init;  /* else return match(ms, s, ep); */
        }
        case '0': case '1': case '2': case '3':
        case '4': case '5': case '6': case '7':
        case '8': case '9': {  /* capture results (%0-%9)? */
          s = match_capture(ms, s, uchar(*(p+1)));
          if (s == NULL) return NULL;
          p+=2; goto init;  /* else return match(ms, s, p+2) */
        }
        default: goto dflt;
      }
    }
    default: dflt: {  /* pattern class plus optional suffix */
      const char *ep = classend(ms, p);  /* points to what is next */
#ifdef UTF8
      int m = s < ms->src_end && singlematch(UTF8_DECODE(s, len), p, ep);
#else
      int m = s < ms->src_end && singlematch(uchar(*s), p, ep);
#endif
      switch (*ep) {
        case '?': {  /* optional */
          const char *res;
#ifdef UTF8
          int eplen = UTF8_CHAR_LENGTH(*ep);
          if (m && ((res=match(ms, s+len, ep+eplen)) != NULL))
#else
          if (m && ((res=match(ms, s+1, ep+1)) != NULL))
#endif
            return res;
#ifdef UTF8
          p=ep+eplen; goto init;  /* else return match(ms, s, ep+eplen); */
#else
          p=ep+1; goto init;  /* else return match(ms, s, ep+1); */
#endif
        }
        case '*': {  /* 0 or more repetitions */
          return max_expand(ms, s, p, ep);
        }
        case '+': {  /* 1 or more repetitions */
#ifdef UTF8
          return (m ? max_expand(ms, s+len, p, ep) : NULL);
#else
          return (m ? max_expand(ms, s+1, p, ep) : NULL);
#endif
        }
        case '-': {  /* 0 or more repetitions (minimum) */
          return min_expand(ms, s, p, ep);
        }
        default: {
          if (!m) return NULL;
#ifdef UTF8
          s+=len; p=ep; goto init;  /* else return match(ms, s+len, ep); */
#else
          s++; p=ep; goto init;  /* else return match(ms, s+1, ep); */
#endif
        }
      }
    }
  }
}



static const char *lmemfind (const char *s1, size_t l1,
                               const char *s2, size_t l2) {
  if (l2 == 0) return s1;  /* empty strings are everywhere */
  else if (l2 > l1) return NULL;  /* avoids a negative `l1' */
  else {
    const char *init;  /* to search for a `*s2' inside `s1' */
    l2--;  /* 1st char will be checked by `memchr' */
    l1 = l1-l2;  /* `s2' cannot be found after that */
    while (l1 > 0 && (init = (const char *)memchr(s1, *s2, l1)) != NULL) {
      init++;   /* 1st char is already checked */
      if (memcmp(init, s2+1, l2) == 0)
        return init-1;
      else {  /* correct `l1' and `s1' to try again */
        l1 -= init-s1;
        s1 = init;
      }
    }
    return NULL;  /* not found */
  }
}


static void push_onecapture (MatchState *ms, int i, const char *s,
                                                    const char *e) {
  if (i >= ms->level) {
    if (i == 0)  /* ms->level == 0, too */
      lua_pushlstring(ms->L, s, e - s);  /* add whole match */
    else
      luaL_error(ms->L, "invalid capture index");
  }
  else {
    ptrdiff_t l = ms->capture[i].len;
    if (l == CAP_UNFINISHED) luaL_error(ms->L, "unfinished capture");
    if (l == CAP_POSITION)
      lua_pushinteger(ms->L, ms->capture[i].init - ms->src_init + 1);
    else
      lua_pushlstring(ms->L, ms->capture[i].init, l);
  }
}


static int push_captures (MatchState *ms, const char *s, const char *e) {
  int i;
  int nlevels = (ms->level == 0 && s) ? 1 : ms->level;
  luaL_checkstack(ms->L, nlevels, "too many captures");
  for (i = 0; i < nlevels; i++)
    push_onecapture(ms, i, s, e);
  return nlevels;  /* number of strings pushed */
}


/* check whether pattern has no special characters */
static int nospecials (const char *p, size_t l) {
  size_t upto = 0;
  do {
    if (strpbrk(p + upto, SPECIALS))
      return 0;  /* pattern has a special character */
    upto += strlen(p + upto) + 1;  /* may have more after \0 */
  } while (upto <= l);
  return 1;  /* no special chars found */
}


static int str_find_aux (lua_State *L, int find) {
  size_t ls, lp;
  const char *s = luaL_checklstring(L, 1, &ls);
  const char *p = luaL_checklstring(L, 2, &lp);
#ifdef UTF8
  size_t utf8ls = utf8_str_len(s, ls);
  size_t utf8lp = utf8_str_len(p, lp);
  size_t init = posrelat(luaL_optinteger(L, 3, 1), utf8ls);
#else
  size_t init = posrelat(luaL_optinteger(L, 3, 1), ls);
#endif
#ifdef UTF8
  int i;
  /* `init' is updated here */
  if (init < 1) init = 1;
  {
    size_t workinit = 1;
    int j;
    /* `init' is UTF-8 character length, so convert to byte length */
    for (i = 0, j = 1; i < ls && j < init; j++) {
      int len = UTF8_CHAR_LENGTH(s[i]);
      workinit += len;
      i += len;
    }
    init = workinit;
  }
#endif
  if (init < 1) init = 1;
  else if (init > ls + 1) {  /* start after string's end? */
    lua_pushnil(L);  /* cannot find anything */
    return 1;
  }
  /* explicit request or no special characters? */
  if (find && (lua_toboolean(L, 4) || nospecials(p, lp))) {
    /* do a plain search */
    const char *s2 = lmemfind(s + init - 1, ls - init + 1, p, lp);
    if (s2) {
#ifdef UTF8
      int begin = 0;
      int end;
      /* calcurate UTF-8 character length from top to matched position */
      for (i = 0; i < (s2 - s);) {
        i += UTF8_CHAR_LENGTH(s[i]);
        begin++;
      }
      /* added matched position character length and pattern string length */
      end = begin + utf8lp;
      lua_pushinteger(L, begin + 1);
      lua_pushinteger(L, end);
#else
      lua_pushinteger(L, s2 - s + 1);
      lua_pushinteger(L, s2 - s + lp);
#endif
      return 2;
    }
  }
  else {
    MatchState ms;
    const char *s1 = s + init - 1;
    int anchor = (*p == '^');
    if (anchor) {
      p++; lp--;  /* skip anchor character */
    }
    ms.L = L;
    ms.src_init = s;
    ms.src_end = s + ls;
    ms.p_end = p + lp;
    do {
      const char *res;
      ms.level = 0;
      if ((res=match(&ms, s1, p)) != NULL) {
        if (find) {
#ifdef UTF8
          int start = 0; /* start character index */
          int actual = 0; /* actual byte length of to begin */
          int end; /* end character index */

          /* calcurate UTF-8 character length and byte length of matched position */
          for (i = 0; i < (s1 - s);) {
            int len = UTF8_CHAR_LENGTH(s[i]);
            i += len;
            actual += len;
            start++;
          }

          /* calcurate character length of matched string */
          end = start;
          /* `res' holds pointer that is matchd string position */
          for (i = actual; s + i < res;) {
            i += UTF8_CHAR_LENGTH(s[i]);
            end++;
          }

          lua_pushinteger(L, start + 1);
          lua_pushinteger(L, end);
#else
          lua_pushinteger(L, s1 - s + 1);  /* start */
          lua_pushinteger(L, res - s);   /* end */
#endif
          return push_captures(&ms, NULL, 0) + 2;
        }
        else
          return push_captures(&ms, s1, res);
      }
#ifdef UTF8
    } while ((s1 += UTF8_CHAR_LENGTH(*s1)) < ms.src_end && !anchor);
#else
    } while (s1++ < ms.src_end && !anchor);
#endif
  }
  lua_pushnil(L);  /* not found */
  return 1;
}


static int str_find (lua_State *L) {
  return str_find_aux(L, 1);
}


static int str_match (lua_State *L) {
  return str_find_aux(L, 0);
}


static int gmatch_aux (lua_State *L) {
  MatchState ms;
  size_t ls, lp;
  const char *s = lua_tolstring(L, lua_upvalueindex(1), &ls);
  const char *p = lua_tolstring(L, lua_upvalueindex(2), &lp);
  const char *src;
  ms.L = L;
  ms.src_init = s;
  ms.src_end = s+ls;
  ms.p_end = p + lp;
  for (src = s + (size_t)lua_tointeger(L, lua_upvalueindex(3));
       src <= ms.src_end;
       src++) {
    const char *e;
    ms.level = 0;
    if ((e = match(&ms, src, p)) != NULL) {
      lua_Integer newstart = e-s;
      if (e == src) newstart++;  /* empty match? go at least one position */
      lua_pushinteger(L, newstart);
      lua_replace(L, lua_upvalueindex(3));
      return push_captures(&ms, src, e);
    }
  }
  return 0;  /* not found */
}


static int gmatch (lua_State *L) {
  luaL_checkstring(L, 1);
  luaL_checkstring(L, 2);
  lua_settop(L, 2);
  lua_pushinteger(L, 0);
  lua_pushcclosure(L, gmatch_aux, 3);
  return 1;
}


static void add_s (MatchState *ms, luaL_Buffer *b, const char *s,
                                                   const char *e) {
  size_t l, i;
  const char *news = lua_tolstring(ms->L, 3, &l);
  for (i = 0; i < l; i++) {
    if (news[i] != L_ESC)
      luaL_addchar(b, news[i]);
    else {
      i++;  /* skip ESC */
      if (!isdigit(uchar(news[i]))) {
        if (news[i] != L_ESC)
          luaL_error(ms->L, "invalid use of " LUA_QL("%c")
                           " in replacement string", L_ESC);
        luaL_addchar(b, news[i]);
      }
      else if (news[i] == '0')
          luaL_addlstring(b, s, e - s);
      else {
        push_onecapture(ms, news[i] - '1', s, e);
        luaL_addvalue(b);  /* add capture to accumulated result */
      }
    }
  }
}


static void add_value (MatchState *ms, luaL_Buffer *b, const char *s,
                                       const char *e, int tr) {
  lua_State *L = ms->L;
  switch (tr) {
    case LUA_TFUNCTION: {
      int n;
      lua_pushvalue(L, 3);
      n = push_captures(ms, s, e);
      lua_call(L, n, 1);
      break;
    }
    case LUA_TTABLE: {
      push_onecapture(ms, 0, s, e);
      lua_gettable(L, 3);
      break;
    }
    default: {  /* LUA_TNUMBER or LUA_TSTRING */
      add_s(ms, b, s, e);
      return;
    }
  }
  if (!lua_toboolean(L, -1)) {  /* nil or false? */
    lua_pop(L, 1);
    lua_pushlstring(L, s, e - s);  /* keep original text */
  }
  else if (!lua_isstring(L, -1))
    luaL_error(L, "invalid replacement value (a %s)", luaL_typename(L, -1));
  luaL_addvalue(b);  /* add result to accumulator */
}


static int str_gsub (lua_State *L) {
  size_t srcl, lp;
  const char *src = luaL_checklstring(L, 1, &srcl);
  const char *p = luaL_checklstring(L, 2, &lp);
  int tr = lua_type(L, 3);
  size_t max_s = luaL_optinteger(L, 4, srcl+1);
  int anchor = (*p == '^');
  size_t n = 0;
  MatchState ms;
  luaL_Buffer b;
  luaL_argcheck(L, tr == LUA_TNUMBER || tr == LUA_TSTRING ||
                   tr == LUA_TFUNCTION || tr == LUA_TTABLE, 3,
                      "string/function/table expected");
  luaL_buffinit(L, &b);
  if (anchor) {
    p++; lp--;  /* skip anchor character */
  }
  ms.L = L;
  ms.src_init = src;
  ms.src_end = src+srcl;
  ms.p_end = p + lp;
  while (n < max_s) {
    const char *e;
    ms.level = 0;
    e = match(&ms, src, p);
    if (e) {
      n++;
      add_value(&ms, &b, src, e, tr);
    }
    if (e && e>src) /* non empty match? */
      src = e;  /* skip it */
    else if (src < ms.src_end)
#ifdef UTF8
    {
      int i;
      int len = UTF8_CHAR_LENGTH(*src);
      for (i = 0; i < len; i++) {
        luaL_addchar(&b, *src++);
      }
    }
#else
      luaL_addchar(&b, *src++);
#endif
    else break;
    if (anchor) break;
  }
  luaL_addlstring(&b, src, ms.src_end-src);
  luaL_pushresult(&b);
  lua_pushinteger(L, n);  /* number of substitutions */
  return 2;
}

/* }====================================================== */



/*
** {======================================================
** STRING FORMAT
** =======================================================
*/

/*
** LUA_INTFRMLEN is the length modifier for integer conversions in
** 'string.format'; LUA_INTFRM_T is the integer type corresponding to
** the previous length
*/
#if !defined(LUA_INTFRMLEN)	/* { */
#if defined(LUA_USE_LONGLONG)

#define LUA_INTFRMLEN		"ll"
#define LUA_INTFRM_T		long long

#else

#define LUA_INTFRMLEN		"l"
#define LUA_INTFRM_T		long

#endif
#endif				/* } */


/*
** LUA_FLTFRMLEN is the length modifier for float conversions in
** 'string.format'; LUA_FLTFRM_T is the float type corresponding to
** the previous length
*/
#if !defined(LUA_FLTFRMLEN)

#define LUA_FLTFRMLEN		""
#define LUA_FLTFRM_T		double

#endif


/* maximum size of each formatted item (> len(format('%99.99f', -1e308))) */
#define MAX_ITEM	512
/* valid flags in a format specification */
#define FLAGS	"-+ #0"
/*
** maximum size of each format specification (such as '%-099.99d')
** (+10 accounts for %99.99x plus margin of error)
*/
#define MAX_FORMAT	(sizeof(FLAGS) + sizeof(LUA_INTFRMLEN) + 10)


static void addquoted (lua_State *L, luaL_Buffer *b, int arg) {
  size_t l;
  const char *s = luaL_checklstring(L, arg, &l);
  luaL_addchar(b, '"');
  while (l--) {
    if (*s == '"' || *s == '\\' || *s == '\n') {
      luaL_addchar(b, '\\');
      luaL_addchar(b, *s);
    }
    else if (*s == '\0' || iscntrl(uchar(*s))) {
      char buff[10];
      if (!isdigit(uchar(*(s+1))))
        sprintf(buff, "\\%d", (int)uchar(*s));
      else
        sprintf(buff, "\\%03d", (int)uchar(*s));
      luaL_addstring(b, buff);
    }
    else
      luaL_addchar(b, *s);
    s++;
  }
  luaL_addchar(b, '"');
}

static const char *scanformat (lua_State *L, const char *strfrmt, char *form) {
  const char *p = strfrmt;
  while (*p != '\0' && strchr(FLAGS, *p) != NULL) p++;  /* skip flags */
  if ((size_t)(p - strfrmt) >= sizeof(FLAGS)/sizeof(char))
    luaL_error(L, "invalid format (repeated flags)");
  if (isdigit(uchar(*p))) p++;  /* skip width */
  if (isdigit(uchar(*p))) p++;  /* (2 digits at most) */
  if (*p == '.') {
    p++;
    if (isdigit(uchar(*p))) p++;  /* skip precision */
    if (isdigit(uchar(*p))) p++;  /* (2 digits at most) */
  }
  if (isdigit(uchar(*p)))
    luaL_error(L, "invalid format (width or precision too long)");
  *(form++) = '%';
  memcpy(form, strfrmt, (p - strfrmt + 1) * sizeof(char));
  form += p - strfrmt + 1;
  *form = '\0';
  return p;
}


/*
** add length modifier into formats
*/
static void addlenmod (char *form, const char *lenmod) {
  size_t l = strlen(form);
  size_t lm = strlen(lenmod);
  char spec = form[l - 1];
  strcpy(form + l - 1, lenmod);
  form[l + lm - 1] = spec;
  form[l + lm] = '\0';
}


static int str_format (lua_State *L) {
  int top = lua_gettop(L);
  int arg = 1;
  size_t sfl;
  const char *strfrmt = luaL_checklstring(L, arg, &sfl);
  const char *strfrmt_end = strfrmt+sfl;
  luaL_Buffer b;
  luaL_buffinit(L, &b);
  while (strfrmt < strfrmt_end) {
    if (*strfrmt != L_ESC)
      luaL_addchar(&b, *strfrmt++);
    else if (*++strfrmt == L_ESC)
      luaL_addchar(&b, *strfrmt++);  /* %% */
    else { /* format item */
      char form[MAX_FORMAT];  /* to store the format (`%...') */
      char *buff = luaL_prepbuffsize(&b, MAX_ITEM);  /* to put formatted item */
      int nb = 0;  /* number of bytes in added item */
      if (++arg > top)
        luaL_argerror(L, arg, "no value");
      strfrmt = scanformat(L, strfrmt, form);
      switch (*strfrmt++) {
        case 'c': {
          nb = sprintf(buff, form, luaL_checkint(L, arg));
          break;
        }
        case 'd':  case 'i': {
          lua_Number n = luaL_checknumber(L, arg);
          LUA_INTFRM_T ni = (LUA_INTFRM_T)n;
          lua_Number diff = n - (lua_Number)ni;
          luaL_argcheck(L, -1 < diff && diff < 1, arg,
                        "not a number in proper range");
          addlenmod(form, LUA_INTFRMLEN);
          nb = sprintf(buff, form, ni);
          break;
        }
        case 'o':  case 'u':  case 'x':  case 'X': {
          lua_Number n = luaL_checknumber(L, arg);
          unsigned LUA_INTFRM_T ni = (unsigned LUA_INTFRM_T)n;
          lua_Number diff = n - (lua_Number)ni;
          luaL_argcheck(L, -1 < diff && diff < 1, arg,
                        "not a non-negative number in proper range");
          addlenmod(form, LUA_INTFRMLEN);
          nb = sprintf(buff, form, ni);
          break;
        }
        case 'e':  case 'E': case 'f':
#if defined(LUA_USE_AFORMAT)
        case 'a': case 'A':
#endif
        case 'g': case 'G': {
          addlenmod(form, LUA_FLTFRMLEN);
          nb = sprintf(buff, form, (LUA_FLTFRM_T)luaL_checknumber(L, arg));
          break;
        }
        case 'q': {
          addquoted(L, &b, arg);
          break;
        }
        case 's': {
          size_t l;
          const char *s = luaL_tolstring(L, arg, &l);
          if (!strchr(form, '.') && l >= 100) {
            /* no precision and string is too long to be formatted;
               keep original string */
            luaL_addvalue(&b);
            break;
          }
          else {
            nb = sprintf(buff, form, s);
            lua_pop(L, 1);  /* remove result from 'luaL_tolstring' */
            break;
          }
        }
        default: {  /* also treat cases `pnLlh' */
          return luaL_error(L, "invalid option " LUA_QL("%%%c") " to "
                               LUA_QL("format"), *(strfrmt - 1));
        }
      }
      luaL_addsize(&b, nb);
    }
  }
  luaL_pushresult(&b);
  return 1;
}

/* }====================================================== */


static const luaL_Reg strlib[] = {
  {"byte", str_byte},
  {"char", str_char},
  {"dump", str_dump},
  {"find", str_find},
  {"format", str_format},
  {"gmatch", gmatch},
  {"gsub", str_gsub},
  {"len", str_len},
  {"lower", str_lower},
  {"match", str_match},
  {"rep", str_rep},
  {"reverse", str_reverse},
  {"sub", str_sub},
  {"upper", str_upper},
  {NULL, NULL}
};


static void createmetatable (lua_State *L) {
  lua_createtable(L, 0, 1);  /* table to be metatable for strings */
  lua_pushliteral(L, "");  /* dummy string */
  lua_pushvalue(L, -2);  /* copy table */
  lua_setmetatable(L, -2);  /* set table as metatable for strings */
  lua_pop(L, 1);  /* pop dummy string */
  lua_pushvalue(L, -2);  /* get string library */
  lua_setfield(L, -2, "__index");  /* metatable.__index = string */
  lua_pop(L, 1);  /* pop metatable */
}


/*
** Open string library
*/
LUAMOD_API int luaopen_string (lua_State *L) {
  luaL_newlib(L, strlib);
  createmetatable(L);
  return 1;
}

