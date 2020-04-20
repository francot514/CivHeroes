namespace CardManager
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class SearchDlg : Form
    {
        private IContainer components;
        private bool down;
        private RadioButton downBox;
        private GroupBox groupBox1;
        private bool hasSearched;
        private CheckBox ignoreCase;
        private int index;
        private Label label1;
        private Label label2;
        private MatchCollection mats;
        private Regex regex;
        private Button replace;
        private Button replaceAll;
        private TextBox replaceBox;
        private TextBox searchBox;
        private Button searchNext;
        private TextBoxBase textBox;
        private RadioButton upBox;

        public SearchDlg()
        {
            this.down = true;
            this.InitializeComponent();
        }

        public SearchDlg(TextBoxBase textBx) : this()
        {
            this.textBox = textBx;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void downBox_CheckedChanged(object sender, EventArgs e)
        {
            this.down = true;
        }

        private void ignoreCase_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.searchBox = new TextBox();
            this.replaceBox = new TextBox();
            this.searchNext = new Button();
            this.replace = new Button();
            this.replaceAll = new Button();
            this.ignoreCase = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.downBox = new RadioButton();
            this.upBox = new RadioButton();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find what:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(14, 0x40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x48, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Replace with:";
            this.searchBox.Location = new Point(0x55, 12);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new Size(0xf8, 20);
            this.searchBox.TabIndex = 1;
            this.searchBox.TextChanged += new EventHandler(this.searchBox_TextChanged);
            this.replaceBox.Location = new Point(0x55, 0x3d);
            this.replaceBox.Name = "replaceBox";
            this.replaceBox.Size = new Size(0xf8, 20);
            this.replaceBox.TabIndex = 1;
            this.searchNext.Enabled = false;
            this.searchNext.Location = new Point(0x160, 12);
            this.searchNext.Name = "searchNext";
            this.searchNext.Size = new Size(0x5b, 0x17);
            this.searchNext.TabIndex = 2;
            this.searchNext.Text = "Find Next";
            this.searchNext.UseVisualStyleBackColor = true;
            this.searchNext.Click += new EventHandler(this.searchNext_Click);
            this.replace.Enabled = false;
            this.replace.Location = new Point(0x160, 0x3d);
            this.replace.Name = "replace";
            this.replace.Size = new Size(0x5b, 0x17);
            this.replace.TabIndex = 3;
            this.replace.Text = "Replace";
            this.replace.UseVisualStyleBackColor = true;
            this.replace.Click += new EventHandler(this.replace_Click);
            this.replaceAll.Enabled = false;
            this.replaceAll.Location = new Point(0x160, 0x6f);
            this.replaceAll.Name = "replaceAll";
            this.replaceAll.Size = new Size(0x5b, 0x17);
            this.replaceAll.TabIndex = 3;
            this.replaceAll.Text = "Replace All";
            this.replaceAll.UseVisualStyleBackColor = true;
            this.replaceAll.Click += new EventHandler(this.replaceAll_Click);
            this.ignoreCase.AutoSize = true;
            this.ignoreCase.Location = new Point(0x10, 0x6f);
            this.ignoreCase.Name = "ignoreCase";
            this.ignoreCase.Size = new Size(0x5e, 0x11);
            this.ignoreCase.TabIndex = 4;
            this.ignoreCase.Text = "Case sensitive";
            this.ignoreCase.UseVisualStyleBackColor = true;
            this.ignoreCase.CheckedChanged += new EventHandler(this.ignoreCase_CheckedChanged);
            this.groupBox1.Controls.Add(this.downBox);
            this.groupBox1.Controls.Add(this.upBox);
            this.groupBox1.Location = new Point(0x83, 0x60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xca, 0x26);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Direction";
            this.downBox.AutoSize = true;
            this.downBox.Checked = true;
            this.downBox.Location = new Point(0x74, 0x10);
            this.downBox.Name = "downBox";
            this.downBox.Size = new Size(0x35, 0x11);
            this.downBox.TabIndex = 0;
            this.downBox.TabStop = true;
            this.downBox.Text = "Down";
            this.downBox.UseVisualStyleBackColor = true;
            this.downBox.CheckedChanged += new EventHandler(this.downBox_CheckedChanged);
            this.upBox.AutoSize = true;
            this.upBox.Location = new Point(0x21, 0x10);
            this.upBox.Name = "upBox";
            this.upBox.Size = new Size(0x27, 0x11);
            this.upBox.TabIndex = 0;
            this.upBox.Text = "Up";
            this.upBox.UseVisualStyleBackColor = true;
            this.upBox.CheckedChanged += new EventHandler(this.upBox_CheckedChanged);
            base.ClientSize = new Size(0x1c9, 0x91);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.ignoreCase);
            base.Controls.Add(this.replaceAll);
            base.Controls.Add(this.replace);
            base.Controls.Add(this.searchNext);
            base.Controls.Add(this.replaceBox);
            base.Controls.Add(this.searchBox);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Name = "SearchDlg";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Find and Replace";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void next()
        {
            try
            {
                if (!this.hasSearched)
                {
                    this.regex = this.ignoreCase.Checked ? new Regex(this.searchBox.Text) : new Regex(this.searchBox.Text, RegexOptions.IgnoreCase);
                    this.mats = this.regex.Matches(this.textBox.Text);
                    this.textBox.Select(this.mats[0].Index, this.mats[0].Value.Length);
                    this.index = 0;
                    this.hasSearched = true;
                }
                else if (this.down)
                {
                    this.index++;
                    if (this.index >= this.mats.Count)
                    {
                        this.index = this.mats.Count - 1;
                    }
                    this.textBox.Select(this.mats[this.index].Index, this.mats[this.index].Value.Length);
                    string text1 = this.mats[this.index].Value;
                }
                else
                {
                    this.index--;
                    if (this.index < 0)
                    {
                        this.index = 0;
                    }
                    this.textBox.Select(this.mats[this.index].Index, this.mats[this.index].Value.Length);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void replace_Click(object sender, EventArgs e)
        {
            try
            {
                this.searchNext_Click(sender, e);
                this.textBox.SelectedText = this.replaceBox.Text;
                this.hasSearched = false;
            }
            catch (Exception)
            {
                MessageBox.Show("没找到!");
            }
        }

        private void replaceAll_Click(object sender, EventArgs e)
        {
            try
            {
                while (true)
                {
                    this.next();
                    this.textBox.SelectedText = this.replaceBox.Text;
                    this.hasSearched = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("替换完毕！");
            }
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            this.hasSearched = false;
            this.index = 0;
            if (!string.IsNullOrEmpty(this.searchBox.Text))
            {
                this.searchNext.Enabled = true;
                this.replaceAll.Enabled = true;
                this.replace.Enabled = true;
            }
            else
            {
                this.searchNext.Enabled = false;
                this.replace.Enabled = false;
                this.replaceAll.Enabled = false;
            }
        }

        private void searchNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.next();
            }
            catch (Exception)
            {
                MessageBox.Show("没找到！");
            }
        }

        private void upBox_CheckedChanged(object sender, EventArgs e)
        {
            this.down = false;
        }
    }
}

