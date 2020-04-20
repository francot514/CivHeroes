namespace CardEditor
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml;

    internal class ColorText
    {
        private List<Keyword> keywords = new List<Keyword>();
        private Color normalColor;
        private Font normalFont;
        private RichTextBox rich;
        private bool running;

        public ColorText(RichTextBox rich)
        {
            this.rich = rich;
            this.normalColor = rich.SelectionColor;
            this.normalFont = rich.Font;
        }

        public void colorAll()
        {
            this.running = true;
            this.rich.SelectAll();
            this.resetFont();
            string text = this.rich.Text;
            text.ToLower();
            this.rich.Visible = false;
            int selectionStart = this.rich.SelectionStart;
            foreach (Keyword keyword in this.keywords)
            {
                for (Match match = new Regex(@"\b" + keyword.Value + @"\b").Match(text); match.Success; match = match.NextMatch())
                {
                    this.rich.Select(match.Index, match.Length);
                    this.rich.SelectionColor = Color.FromName(keyword.Color);
                    this.rich.ClearUndo();
                    try
                    {
                        FontFamily fontFamily = this.rich.SelectionFont.FontFamily;
                        float size = this.rich.SelectionFont.Size;
                        this.rich.SelectionFont = new Font(this.normalFont.FontFamily, this.normalFont.Size, keyword.Bold ? FontStyle.Bold : FontStyle.Regular);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString());
                    }
                }
            }
            for (Match match2 = new Regex(@"\-\-[^\n]*(?!=\n)").Match(this.rich.Text); match2.Success; match2 = match2.NextMatch())
            {
                this.rich.Select(match2.Index, match2.Length);
                this.rich.SelectionColor = Color.Green;
            }
            this.rich.Visible = true;
            this.rich.Focus();
            this.rich.Select(selectionStart, 0);
            this.resetFont();
            this.running = false;
        }

        public string GetLastWord()
        {
            return new Regex(@"\-\-[^\n]*(?!=\n)|\s|\W|\b\w+\b", RegexOptions.RightToLeft).Match(this.rich.Text.Substring(0, this.rich.SelectionStart)).Value;
        }

        public void loadKeywordXml(string fileName)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(fileName);
                foreach (XmlElement element in document.DocumentElement.ChildNodes)
                {
                    this.keywords.Add(new Keyword { Value = element.GetAttribute("value"), Bold = element.GetAttribute("bold") == "true", Color = element.GetAttribute("color") });
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void MySelect(string str)
        {
            int selectionStart = this.rich.SelectionStart;
            Color selectionColor = this.rich.SelectionColor;
            bool flag = false;
            foreach (Keyword keyword in this.keywords)
            {
                if (keyword.Value == str)
                {
                    this.rich.Select(this.rich.SelectionStart - str.Length, str.Length);
                    this.rich.SelectionColor = Color.FromName(keyword.Color);
                    try
                    {
                        this.rich.SelectionFont = new Font(this.normalFont.FontFamily, this.normalFont.Size, keyword.Bold ? FontStyle.Bold : FontStyle.Regular);
                        flag = true;
                        continue;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                        MessageBox.Show(exception.ToString());
                        continue;
                    }
                }
            }
            if (str.Contains("--"))
            {
                this.rich.Select(selectionStart - str.Length, str.Length);
                this.rich.SelectionColor = Color.Green;
                flag = true;
            }
            if (!flag)
            {
                this.rich.Select(this.rich.SelectionStart - str.Length, str.Length);
                try
                {
                    this.resetFont();
                }
                catch
                {
                    Console.WriteLine("Font mistake");
                }
            }
            this.rich.Select(selectionStart, 0);
        }

        public void resetFont()
        {
            this.rich.SelectionFont = this.normalFont;
            this.rich.SelectionColor = this.normalColor;
        }

        public void stepColor()
        {
            if (!this.running)
            {
                this.MySelect(this.GetLastWord());
            }
        }
    }
}

