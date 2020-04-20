namespace CardManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;

    public class SelectDlg : Form
    {
        private Button button1;
        private Button button2;
        public List<CheckBox> checkBoxes;
        private IContainer components;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel1;
        private ProgressBar progressBar1;
        public List<Selection> selections;

        public SelectDlg()
        {
            this.selections = new List<Selection>();
            this.checkBoxes = new List<CheckBox>();
            this.InitializeComponent();
        }

        public SelectDlg(int dlgType, int effectType, string xmlPath) : this()
        {
            Predicate<Selection> match = null;
            this.loadSelectionXml(xmlPath);
            if (match == null)
            {
                match = delegate (Selection s) {
                    if (s.DlgType == dlgType)
                    {
                        return s.EffectType != effectType;
                    }
                    return true;
                };
            }
            this.selections.RemoveAll(match);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void checked_Changed(object sender, EventArgs e)
        {
            CheckBox cbx = (CheckBox) sender;
            if (cbx.Checked)
            {
                Predicate<Selection> predicate = null;
                switch (this.findSelectionFromCheckBox(cbx).Combine)
                {
                    case "-1":
                        return;

                    case "0":
                        this.turnOnCheckBox(false, cbx);
                        return;
                }
                this.turnOnCheckBox(false, cbx);
                Selection ts1 = this.findSelectionFromCheckBox(cbx);
                if (predicate == null)
                {
                    predicate = delegate (Selection s) {
                        return s.isOneInCombines(ts1.Combine);
                    };
                }
                using (List<Selection>.Enumerator enumerator = this.selections.FindAll(predicate).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        this.findCheckBoxFromSelection(enumerator.Current).Enabled = true;
                    }
                    return;
                }
            }
            Predicate<Selection> match = null;
            Selection ts2;
            switch (this.findSelectionFromCheckBox(cbx).Combine)
            {
                case "-1":
                    return;

                case "0":
                    this.turnOnCheckBox(true, cbx);
                    this.resetCheckBox();
                    return;

                default:
                    this.turnOnCheckBox(true, cbx);
                    this.resetCheckBox();
                    ts2 = this.findSelectionFromCheckBox(cbx);
                    if (match == null)
                    {
                        match = delegate (Selection s) {
                            return s.isOneInCombines(ts2.Combine);
                        };
                    }
                    using (List<Selection>.Enumerator enumerator2 = this.selections.FindAll(match).GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            this.findCheckBoxFromSelection(enumerator2.Current).Checked = false;
                        }
                    }
                    return;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private CheckBox findCheckBoxFromSelection(Selection s)
        {
            return this.checkBoxes[this.selections.IndexOf(s)];
        }

        private Selection findSelectionFromCheckBox(CheckBox cbx)
        {
            return this.selections[this.checkBoxes.IndexOf(cbx)];
        }

        public bool[] getViewSetting()
        {
            bool[] flagArray = new bool[4];
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            foreach (CheckBox box in this.checkBoxes)
            {
                if (box.Checked)
                {
                    Selection selection = this.selections[this.checkBoxes.IndexOf(box)];
                    num += selection.CodeNeed;
                    num2 += selection.RangeNeed;
                    num3 += selection.TargetRangeNeed;
                    num4 += selection.CountLimitNeed;
                }
            }
            if (num > 0)
            {
                flagArray[0] = true;
            }
            if (num2 > 0)
            {
                flagArray[1] = true;
            }
            if (num3 > 0)
            {
                flagArray[2] = true;
            }
            if (num4 > 0)
            {
                flagArray[3] = true;
            }
            return flagArray;
        }

        private void InitializeComponent()
        {
            this.button1 = new Button();
            this.button2 = new Button();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.progressBar1 = new ProgressBar();
            this.panel1 = new Panel();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.button1.Location = new Point(0x1a1, 12);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.Location = new Point(0x1f2, 12);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x4b, 0x17);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = SystemColors.ControlDark;
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(0x249, 0x18a);
            this.flowLayoutPanel1.TabIndex = 3;
            this.progressBar1.Location = new Point(9, 12);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(0x179, 0x17);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Visible = false;
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x18a);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x249, 0x2c);
            this.panel1.TabIndex = 4;
            this.AutoSize = true;
            base.ClientSize = new Size(0x249, 0x1b6);
            base.Controls.Add(this.flowLayoutPanel1);
            base.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            base.Name = "SelectDlg";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "SelectDlg";
            base.Shown += new EventHandler(this.SelectDlg_Shown);
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void InitialView()
        {
            this.progressBar1.Maximum = this.selections.Count;
            foreach (Selection selection in this.selections)
            {
                CheckBox item = new CheckBox {
                    Text = selection.ToString(),
                    AutoSize = true
                };
                if (selection.Enable == 0)
                {
                    item.Enabled = false;
                }
                item.CheckedChanged += new EventHandler(this.checked_Changed);
                this.checkBoxes.Add(item);
                this.progressBar1.Value++;
            }
            this.flowLayoutPanel1.Controls.AddRange((Control[]) this.checkBoxes.ToArray());
        }

        public void loadSelectionXml(string path)
        {
            XmlReader reader = null;
            try
            {
                reader = XmlReader.Create(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Dlg.xml读取失败！");
            }
            using (reader)
            {
                while (reader.Read())
                {
                    if (reader.Name == "selection")
                    {
                        this.selections.Add(new Selection { DlgType = int.Parse(reader.GetAttribute(0)), EffectType = int.Parse(reader.GetAttribute(1)), ConstID = int.Parse(reader.GetAttribute(2)), ConstName = reader.GetAttribute(3), Enable = int.Parse(reader.GetAttribute(4)), Combine = reader.GetAttribute(5), CodeNeed = int.Parse(reader.GetAttribute(6)), RangeNeed = int.Parse(reader.GetAttribute(7)), TargetRangeNeed = int.Parse(reader.GetAttribute(8)), CountLimitNeed = int.Parse(reader.GetAttribute(9)), ValueNeed = int.Parse(reader.GetAttribute(10)), Description = reader.GetAttribute(11) });
                    }
                }
            }
        }

        private void resetCheckBox()
        {
            for (int i = 0; i < this.selections.Count; i++)
            {
                this.checkBoxes[i].Checked = false;
                this.checkBoxes[i].Enabled = this.selections[i].Enable == 1;
            }
        }

        private void SelectDlg_Shown(object sender, EventArgs e)
        {
            this.InitialView();
        }

        private void turnOnCheckBox(bool enable, CheckBox tcbx)
        {
            foreach (CheckBox box in this.checkBoxes)
            {
                if (box != tcbx)
                {
                    box.Enabled = enable;
                }
            }
        }

        public string Value
        {
            get
            {
                string str = "";
                foreach (CheckBox box in this.checkBoxes)
                {
                    if (box.Checked)
                    {
                        int index = this.checkBoxes.IndexOf(box);
                        str = str + this.selections[index].ConstName;
                        str = str + "+";
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    return str.Remove(str.Length - 1);
                }
                return "";
            }
        }
    }
}

