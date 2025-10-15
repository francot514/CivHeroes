#ifdef UTF8
#ifndef utf8_h
#define utf8_h

/*
** This header file provides useful functions for handling UTF-8 string.
**
** The following table is the valid UTF-8 sequences.
**
** +------------------+------+------+------+------+
** |    Code point    |byte 1|byte 2|byte 3|byte 4|
** +------------------+------+------+------+------+
** |U+000000..U+00007F|00..7F|      |      |      |
** |U+000080..U+0007FF|C2..DF|80..BF|      |      |
** |U+000800..U+000FFF|E0    |A0..BF|80..BF|      |
** |U+001000..U+00CFFF|E1..EC|80..BF|80..BF|      |
** |U+00D000..U+00D7FF|ED    |80..9F|80..BF|      |
** |U+00E000..U+00FFFF|EE..EF|80..BF|80..BF|      |
** |U+010000..U+03FFFF|F0    |90..BF|80..BF|80..BF|
** |U+040000..U+0FFFFF|F1..F3|80..BF|80..BF|80..BF|
** |U+100000..U+10FFFF|F4    |80..8F|80..BF|80..BF|
** +------------------+------+------+------+------+
**
** This is specified by RFC-3629. The following BNF is quoted from RFC-3629.
**
** > UTF8-octets = *( UTF8-char )
** > UTF8-char   = UTF8-1 / UTF8-2 / UTF8-3 / UTF8-4
** > UTF8-1      = %x00-7F
** > UTF8-2      = %xC2-DF UTF8-tail
** > UTF8-3      = %xE0 %xA0-BF UTF8-tail / %xE1-EC 2( UTF8-tail ) /
** >               %xED %x80-9F UTF8-tail / %xEE-EF 2( UTF8-tail )
** > UTF8-4      = %xF0 %x90-BF 2( UTF8-tail ) / %xF1-F3 3( UTF8-tail ) /
** >               %xF4 %x80-8F 2( UTF8-tail )
** > UTF8-tail   = %x80-BF
**
** - RFC-3629: UTF-8, a transformation format of ISO 10646
*/

/*
** Guess UTF-8 character byte length.
** Parameter `b1' is a first byte of UTF-8 character.
** This macro returns integer that range from 1 to 4.
*/
#define UTF8_CHAR_LENGTH(b1)          \
  (((unsigned char) (b1) > 0xEF) ? 4  \
  : ((unsigned char) (b1) > 0xDF) ? 3 \
  : ((unsigned char) (b1) > 0x7F) ? 2 \
  : 1)

/*
** Guess UTF-8 code length.
** Parameter `code' is a character code of UTF-8.
** This macro returns integer that range from 1 to 4.
*/
#define UTF8_CODE_LENGTH(code) \
  (((code) < 0x80) ? 1         \
  : ((code) < 0x800) ? 2       \
  : ((code) < 0x10000) ? 3     \
  : ((code) < 0x200000) ? 4    \
  : -1)

/*
** Search previous character pointer.
** Parameter `s' is a reference pointer.
** Parameter `prev' is output of previous character pointer.
*/
#define UTF8_PREV_CHAR_PTR(s, prev)                                   \
  {                                                                   \
    const char *cp;                                                   \
    for (cp = s - 1; ; cp -= 1) {                                     \
      int cc = *cp & 0xFF;                                            \
      if ((cc >= 0 && cc <= 0x7F) || (!(cc >= 0x80 && cc <= 0xBF))) { \
        prev = cp;                                                    \
        break;                                                        \
      }                                                               \
    }                                                                 \
  }

/*
** Convert code to byte array.
** The following matrix is specification of UTF-8 transfer encoding.
**
** +-------------------+-----------+-----------+-----------+-----------+-------+-------+-------+-------+-------+
** | Range of code     | 1st byte  | 2nd byte  | 3rd byte  | 4th byte  | 1st b | 2nd b | 3rd b | 4th b | Bits  |
** +-------------------+-----------+-----------+-----------+-----------+-------+-------+-------+-------+-------+
** | U+0000..U+007F    | 0xxx xxxx |           |           |           | 00-7F |       |       |       |  7bit |
** |   min: U+0000     |  000 0000 |           |           |           |    00 |       |       |       |       |
** |   max: U+007F     |  111 1111 |           |           |           |    7F |       |       |       |       |
** +-------------------+-----------+-----------+-----------+-----------+-------+-------+-------+-------+-------+
** | U+0080..U+07FF    | 110yyy yx | 10xx xxxx |           |           | C2-DF | 80-BF |       |       | 11bit |
** |   min: U+0080     |    000 10 |   00 0000 |           |           |    C2 | 80    |       |       |       |
** |   max: U+07FF     |    111 11 |   11 1111 |           |           |    DF | BF    |       |       |       |
** +-------------------+-----------+-----------+-----------+-----------+-------+-------+-------+-------+-------+
** | U+0800..U+FFFF    |  1110yyyy | 10yxxx xx | 10xx xxxx |           | E0-EF | 80-BF | 80-BF |       | 16bit |
** |   min: U+0800     |      0000 |   1000 00 |   00 0000 |           |    E0 |    A0 |    80 |       |       |
** |   max: U+FFFF     |      1111 |   1111 11 |   11 1111 |           |    EF |    BF |    BF |       |       |
** +-------------------+-----------+-----------+-----------+-----------+-------+-------+-------+-------+-------+
** | U+10000..U+1FFFFF | 11110y yy | 10yy xxxx | 10xxxx xx | 10xx xxxx | F0-F7 | 80-BF | 80-BF | 80-BF | 21bit |
** |   min: U+10000    |      0 00 |   01 0000 |   0000 00 |   00 0000 |    F0 |    90 |    80 |    80 |       |
** |   max: U+1FFFFF   |      1 11 |   11 1111 |   1111 11 |   11 1111 |    F7 |    BF |    BF |    BF |       |
** +-------------------+-----------+-----------+-----------+-----------+-------+-------+-------+-------+-------+
**
** Parameter `code' is a character code of UTF-8.
** Parameter `len' is a length of `code'.
** Parameter `buf' is a buffer of output.
*/
/* TODO: Maybe conditional branch is not needed if implementation is well. */
#define UTF8_ENCODE(code, len, buf)                                                                                         \
  (len == 1 ? buf[0] = code                                                                                                 \
  : len == 2 ? (buf[0] = ((code >> 6) & 0x1F) | 0xC0, buf[1] = (code & 0x3F) | 0x80)                                        \
  : len == 3 ? (buf[0] = ((code >> 12) & 0x0F) | 0xE0, buf[1] = ((code >> 6) & 0x3F) | 0x80, buf[2] = (code & 0x3F) | 0x80) \
  : (buf[0] = ((code >> 18) & 0x07) | 0xF0, buf[1] = ((code >> 12) & 0x3F) | 0x80, buf[2] = ((code >> 6) & 0x3F) | 0x80, buf[3] = (code & 0x3F) | 0x80))

/* Helper macro for UTF8_BYTES_TO_CODE. */
#define mask(c, n, s) ((c & ((1 << n) - 1)) << s)

/*
** Convert byte array to code.
** Parameter `s' is a byte array of UTF-8 string.
** Parameter `len' is a length of first UTF-8 character of `s'
** This macro returns code of first UTF-8 character of `s'
*/
/* TODO: Maybe conditional branch is not needed if added 2 NULL terminated to tail of Lua internal string. */
#define UTF8_DECODE(s, len)                                                  \
  ((unsigned int) (len == 1 ? (s)[0]                                         \
  : len == 2 ? mask((s)[0], 5, 6) | mask((s)[1], 6, 0)                       \
  : len == 3 ? mask((s)[0], 4, 12) | mask((s)[1], 6, 6) | mask((s)[2], 6, 0) \
  : mask((s)[0], 3, 18) | mask((s)[1], 6, 12) | mask((s)[2], 6, 6) | mask((s)[3], 6, 0)))

/*
** This function returns true if parameter `c' is space of UTF-8,
** otherwise returns false.
*/
#define UTF8_ISSPACE(c) (                             \
  (c) == 0x0020 ? 1 : /* SPACE */                     \
  (c) == 0x00A0 ? 1 : /* NO-BREAK SPACE */            \
  (c) == 0x1680 ? 1 : /* OGHAM SPACE MARK */          \
  (c) == 0x180E ? 1 : /* MONGOLIAN VOWEL SEPARATOR */ \
  (c) == 0x2000 ? 1 : /* EN QUAD */                   \
  (c) == 0x2001 ? 1 : /* EM QUAD */                   \
  (c) == 0x2002 ? 1 : /* EN SPACE */                  \
  (c) == 0x2003 ? 1 : /* EM SPACE */                  \
  (c) == 0x2004 ? 1 : /* THREE-PER-EM SPACE */        \
  (c) == 0x2005 ? 1 : /* FOUR-PER-EM SPACE */         \
  (c) == 0x2006 ? 1 : /* SIX-PER-EM SPACE */          \
  (c) == 0x2007 ? 1 : /* FIGURE SPACE */              \
  (c) == 0x2008 ? 1 : /* PUNCTUATION SPACE */         \
  (c) == 0x2009 ? 1 : /* THIN SPACE */                \
  (c) == 0x200A ? 1 : /* HAIR SPACE */                \
  (c) == 0x202F ? 1 : /* NARROW NO-BREAK SPACE */     \
  (c) == 0x205F ? 1 : /* MEDIUM MATHEMATICAL SPACE */ \
  (c) == 0x3000 ? 1 : /* IDEOGRAPHIC SPACE */         \
  0)

/*
** This function returns true if parameter `c' is punctuation of UTF-8,
** otherwise returns false.
*/
#define UTF8_ISPUNCT(c) (                                               \
  ((c) >= 0x2000 && (c) <= 0x206F) || /* General Punctuation */         \
  ((c) >= 0xFE50 && (c) <= 0xFE6F) || /* Small Form Variants */         \
  ((c) >= 0x2E00 && (c) <= 0x2E7F) || /* Supplemental Punctuation */    \
  ((c) >= 0x3000 && (c) <= 0x303F) || /* CJK Symbols and Punctuation */ \
  ((c) >= 0xFE30 && (c) <= 0xFE4F) || /* CJK Compatibility Forms */     \
  ((c) >= 0xFE10 && (c) <= 0xFE1F)    /* Vertical Forms */              \
  ? 1 : 0)

#endif
#endif
