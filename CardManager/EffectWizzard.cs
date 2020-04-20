namespace CardManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class EffectWizzard : Form
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private TextBox CateBox;
        private Button cateBt;
        private string cID;
        private TextBox CodeBox;
        private Button codeBt;
        private IContainer components;
        private TextBox conBox;
        private CheckBox conCBox;
        private TextBox CostBox;
        private CheckBox costCBox;
        private NumericUpDown countNUD;
        private NumericUpDown descrNUD;
        private string dlgxmlPath = (Application.StartupPath + @"\Data\Dlg.xml");
        private List<EffectSet> effects = new List<EffectSet>();
        private GroupBox groupAction;
        private GroupBox groupBox1;
        private GroupBox groupbox3;
        private GroupBox groupBox5;
        private GroupBox groupEffect;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private ListBox listBox1;
        private TextBox nameBox;
        private NumericUpDown numericUpDown3;
        private TextBox opBox;
        private CheckBox opCBox;
        private Panel panel1;
        private TextBox propBox;
        private Button propBt;
        private TextBox RangeBox;
        private Button rangeBt;
        private TextBox resetBox;
        private Button resetBt;
        private NumericUpDown resetCountNUD;
        private List<Selection> selections = new List<Selection>();
        private Splitter splitter1;
        private TextBox TargetRange;
        private TextBox TargetRange2;
        private ComboBox tmpltComb;
        private RadioButton toCard;
        private RadioButton toPlayer;
        private TextBox TrgBox;
        private CheckBox trgCBox;
        private Button TrgRangBt1;
        private Button TrgRangBt2;
        private TextBox TypeBox;
        private Button typeBt;
        private TextBox varBox;
        private CheckBox varCBox;

        public EffectWizzard(string cID)
        {
            this.cID = cID;
            this.InitializeComponent();
            this.tmpltComb.SelectedIndex = 0;
            this.enableSet();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.listBox1.SelectedIndex;
            this.listBox1.Items.RemoveAt(selectedIndex);
            this.effects.RemoveAt(selectedIndex);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.checkeItems())
            {
                this.effects.Add(this.createEffect());
                this.listBox1.Items.Clear();
                this.listBox1.Items.AddRange((object[]) this.effects.ToArray());
                this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                this.clear();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            this.editEffectSet(this.listBox1.SelectedIndex);
        }

        private void cateBt_Click(object sender, EventArgs e)
        {
            SelectDlg dlg = new SelectDlg(1, this.tmpltComb.SelectedIndex, this.dlgxmlPath);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.CateBox.Text = dlg.Value;
            }
        }

        private bool checkeItems()
        {
            if (string.IsNullOrEmpty(this.nameBox.Text))
            {
                MessageBox.Show("Must have the effect of name!");
                return false;
            }
            if (this.TypeBox.Enabled && string.IsNullOrEmpty(this.TypeBox.Text))
            {
                MessageBox.Show("Type property can not be empty.");
                return false;
            }
            if (this.CodeBox.Enabled && string.IsNullOrEmpty(this.CodeBox.Text))
            {
                MessageBox.Show("Code property can not be empty.");
                return false;
            }
            if (this.RangeBox.Enabled && string.IsNullOrEmpty(this.RangeBox.Text))
            {
                MessageBox.Show("Range property can not be empty.");
                return false;
            }
            if ((this.TargetRange.Enabled && this.TargetRange2.Enabled) && (string.IsNullOrEmpty(this.TargetRange.Text) || string.IsNullOrEmpty(this.TargetRange2.Text)))
            {
                MessageBox.Show("TargetRange can not be empty .");
                return false;
            }
            if (this.groupAction.Enabled)
            {
                if (string.IsNullOrEmpty(this.conBox.Text) && this.conCBox.Checked == true)
                {
                    MessageBox.Show("Please write the condition function name");
                    return false;
                }
                if (string.IsNullOrEmpty(this.CostBox.Text) && this.costCBox.Checked == true)
                {

                    MessageBox.Show("Please write the cost function name");
                    return false;
                }
                if (string.IsNullOrEmpty(this.opBox.Text) && this.opCBox.Checked == true)
                {
                    MessageBox.Show("Please write the operation function name");
                    return false;
                }
            }
            if (this.groupEffect.Enabled && this.varBox.Enabled)
            {
                if (MessageBox.Show(@" is set affect the value ? \N attack power increased by 1000.", "prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.varBox.Focus();
                    return false;
                }
                this.varCBox.Checked = false;
            }
            if (this.TrgBox.Enabled)
            {
                if (MessageBox.Show(@" whether the Effect filter goal ? \N If the court [ Warrior ] attack rises.", "prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.TrgBox.Focus();
                    return false;
                }
                this.trgCBox.Checked = false;
            }
            return true;
        }

        private void clear()
        {
            this.descrNUD.Value = -1M;
            this.TypeBox.Text = "";
            this.CodeBox.Text = "";
            this.RangeBox.Text = "";
            this.TargetRange.Text = "";
            this.TargetRange2.Text = "";
            this.resetBox.Text = "";
            this.resetCountNUD.Value = -1M;
            this.propBox.Text = "";
            this.CateBox.Text = "";
            this.countNUD.Value = -1M;
            this.conBox.Text = "";
            this.TrgBox.Text = "";
            this.CostBox.Text = "";
            this.opBox.Text = "";
            this.varBox.Text = "";
            this.nameBox.Text = "";
            this.toCard.Checked = true;
            this.conCBox.Checked = true;
            this.costCBox.Checked = true;
            this.opCBox.Checked = true;
            this.varCBox.Checked = true;
            this.trgCBox.Checked = true;
        }

        private void codeBt_Click(object sender, EventArgs e)
        {
            SelectDlg dlg = new SelectDlg(2, this.tmpltComb.SelectedIndex, this.dlgxmlPath);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.CodeBox.Text = dlg.Value;
            }
        }

        private void conCBox_CheckedChanged(object sender, EventArgs e)
        {
            this.conBox.Enabled = this.conCBox.Checked;
        }

        private void costCBox_CheckedChanged(object sender, EventArgs e)
        {
            this.CostBox.Enabled = this.costCBox.Checked;
        }

        private EffectSet createEffect()
        {
            return new EffectSet { 
                cID = this.cID, Name = this.nameBox.Text, Description = (int) this.descrNUD.Value, Type = this.TypeBox.Text, Code = this.CodeBox.Text, Range = this.RangeBox.Text, TargetRange1 = this.TargetRange.Text, TargetRange2 = this.TargetRange2.Text, Reset = this.resetBox.Text, Property = this.propBox.Text, Category = this.CateBox.Text, CountLimit = (int) this.countNUD.Value, Condition = this.conBox.Text, Target = this.TrgBox.Text, Cost = this.CostBox.Text, Operation = this.opBox.Text, 
                Value = this.varBox.Text, ResetCount = (int) this.resetCountNUD.Value, RegistTo = this.toCard.Checked ? 0 : 1
             };
        }

        private void disableSet()
        {
            this.groupBox1.Enabled = this.groupAction.Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void editEffectSet(int index)
        {
            EffectSet set = this.effects[index];
            set.cID = this.cID;
            set.Name = this.nameBox.Text;
            set.Description = (int) this.descrNUD.Value;
            set.Type = this.TypeBox.Text;
            set.Code = this.CodeBox.Text;
            set.Range = this.RangeBox.Text;
            set.TargetRange1 = this.TargetRange.Text;
            set.TargetRange2 = this.TargetRange2.Text;
            set.Reset = this.resetBox.Text;
            set.Property = this.propBox.Text;
            set.Category = this.CateBox.Text;
            set.CountLimit = (int) this.countNUD.Value;
            set.Condition = this.conBox.Text;
            set.Target = this.TrgBox.Text;
            set.Cost = this.CostBox.Text;
            set.Operation = this.opBox.Text;
            set.Value = this.varBox.Text;
            set.ResetCount = (int) this.resetCountNUD.Value;
            set.RegistTo = this.toCard.Checked ? 0 : 1;
        }

        private void enableSet()
        {
            this.groupBox1.Enabled = this.groupAction.Enabled = true;
        }

        public string getScript()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int selectedIndex = this.listBox1.SelectedIndex;
                builder.AppendLine(this.effects[selectedIndex].getScript());
                MessageBox.Show("Code generation has to clipboard!");
                return builder.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupEffect = new System.Windows.Forms.GroupBox();
            this.varCBox = new System.Windows.Forms.CheckBox();
            this.varBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupbox3 = new System.Windows.Forms.GroupBox();
            this.toPlayer = new System.Windows.Forms.RadioButton();
            this.toCard = new System.Windows.Forms.RadioButton();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.tmpltComb = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cateBt = new System.Windows.Forms.Button();
            this.descrNUD = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.resetBt = new System.Windows.Forms.Button();
            this.propBt = new System.Windows.Forms.Button();
            this.TrgRangBt2 = new System.Windows.Forms.Button();
            this.TrgRangBt1 = new System.Windows.Forms.Button();
            this.rangeBt = new System.Windows.Forms.Button();
            this.TargetRange2 = new System.Windows.Forms.TextBox();
            this.codeBt = new System.Windows.Forms.Button();
            this.TargetRange = new System.Windows.Forms.TextBox();
            this.resetBox = new System.Windows.Forms.TextBox();
            this.RangeBox = new System.Windows.Forms.TextBox();
            this.CateBox = new System.Windows.Forms.TextBox();
            this.propBox = new System.Windows.Forms.TextBox();
            this.CodeBox = new System.Windows.Forms.TextBox();
            this.typeBt = new System.Windows.Forms.Button();
            this.TypeBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.resetCountNUD = new System.Windows.Forms.NumericUpDown();
            this.countNUD = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupAction = new System.Windows.Forms.GroupBox();
            this.opCBox = new System.Windows.Forms.CheckBox();
            this.costCBox = new System.Windows.Forms.CheckBox();
            this.conCBox = new System.Windows.Forms.CheckBox();
            this.CostBox = new System.Windows.Forms.TextBox();
            this.opBox = new System.Windows.Forms.TextBox();
            this.conBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.trgCBox = new System.Windows.Forms.CheckBox();
            this.TrgBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupEffect.SuspendLayout();
            this.groupbox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.descrNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resetCountNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.countNUD)).BeginInit();
            this.groupAction.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(585, 324);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(688, 324);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 28);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupEffect);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.groupbox3);
            this.panel1.Controls.Add(this.nameBox);
            this.panel1.Controls.Add(this.tmpltComb);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupAction);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(799, 357);
            this.panel1.TabIndex = 2;
            // 
            // groupEffect
            // 
            this.groupEffect.Controls.Add(this.varCBox);
            this.groupEffect.Controls.Add(this.varBox);
            this.groupEffect.Controls.Add(this.label14);
            this.groupEffect.Location = new System.Drawing.Point(528, 151);
            this.groupEffect.Name = "groupEffect";
            this.groupEffect.Size = new System.Drawing.Size(257, 50);
            this.groupEffect.TabIndex = 50;
            this.groupEffect.TabStop = false;
            this.groupEffect.Text = " sustainable type function ";
            // 
            // varCBox
            // 
            this.varCBox.AutoSize = true;
            this.varCBox.Checked = true;
            this.varCBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.varCBox.Location = new System.Drawing.Point(236, 23);
            this.varCBox.Name = "varCBox";
            this.varCBox.Size = new System.Drawing.Size(15, 14);
            this.varCBox.TabIndex = 29;
            this.varCBox.UseVisualStyleBackColor = true;
            this.varCBox.CheckedChanged += new System.EventHandler(this.varCBox_CheckedChanged);
            // 
            // varBox
            // 
            this.varBox.Location = new System.Drawing.Point(100, 20);
            this.varBox.Name = "varBox";
            this.varBox.Size = new System.Drawing.Size(121, 20);
            this.varBox.TabIndex = 26;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(19, 24);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(34, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Value";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 355);
            this.splitter1.TabIndex = 49;
            this.splitter1.TabStop = false;
            // 
            // groupbox3
            // 
            this.groupbox3.Controls.Add(this.toPlayer);
            this.groupbox3.Controls.Add(this.toCard);
            this.groupbox3.Location = new System.Drawing.Point(528, 266);
            this.groupbox3.Name = "groupbox3";
            this.groupbox3.Size = new System.Drawing.Size(257, 44);
            this.groupbox3.TabIndex = 48;
            this.groupbox3.TabStop = false;
            this.groupbox3.Text = " Registered to ";
            // 
            // toPlayer
            // 
            this.toPlayer.AutoSize = true;
            this.toPlayer.Location = new System.Drawing.Point(160, 20);
            this.toPlayer.Name = "toPlayer";
            this.toPlayer.Size = new System.Drawing.Size(61, 17);
            this.toPlayer.TabIndex = 1;
            this.toPlayer.Text = "players ";
            this.toPlayer.UseVisualStyleBackColor = true;
            // 
            // toCard
            // 
            this.toCard.AutoSize = true;
            this.toCard.Checked = true;
            this.toCard.Location = new System.Drawing.Point(57, 20);
            this.toCard.Name = "toCard";
            this.toCard.Size = new System.Drawing.Size(52, 17);
            this.toCard.TabIndex = 0;
            this.toCard.TabStop = true;
            this.toCard.Text = "Cards";
            this.toCard.UseVisualStyleBackColor = true;
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(232, 6);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(100, 20);
            this.nameBox.TabIndex = 47;
            // 
            // tmpltComb
            // 
            this.tmpltComb.FormattingEnabled = true;
            this.tmpltComb.Items.AddRange(new object[] {
            "triggered",
            "continuous"});
            this.tmpltComb.Location = new System.Drawing.Point(582, 9);
            this.tmpltComb.Name = "tmpltComb";
            this.tmpltComb.Size = new System.Drawing.Size(179, 21);
            this.tmpltComb.TabIndex = 41;
            this.tmpltComb.SelectedIndexChanged += new System.EventHandler(this.tmpltComb_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(167, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(66, 13);
            this.label16.TabIndex = 38;
            this.label16.Text = " effect name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(502, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 13);
            this.label12.TabIndex = 38;
            this.label12.Text = " effect template ";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(143, 320);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(92, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "add ";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(276, 320);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(89, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "modify ";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(406, 320);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(89, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = " Delete Effect";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(13, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(95, 293);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cateBt);
            this.groupBox1.Controls.Add(this.descrNUD);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.resetBt);
            this.groupBox1.Controls.Add(this.propBt);
            this.groupBox1.Controls.Add(this.TrgRangBt2);
            this.groupBox1.Controls.Add(this.TrgRangBt1);
            this.groupBox1.Controls.Add(this.rangeBt);
            this.groupBox1.Controls.Add(this.TargetRange2);
            this.groupBox1.Controls.Add(this.codeBt);
            this.groupBox1.Controls.Add(this.TargetRange);
            this.groupBox1.Controls.Add(this.resetBox);
            this.groupBox1.Controls.Add(this.RangeBox);
            this.groupBox1.Controls.Add(this.CateBox);
            this.groupBox1.Controls.Add(this.propBox);
            this.groupBox1.Controls.Add(this.CodeBox);
            this.groupBox1.Controls.Add(this.typeBt);
            this.groupBox1.Controls.Add(this.TypeBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.numericUpDown3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.resetCountNUD);
            this.groupBox1.Controls.Add(this.countNUD);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(127, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 272);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // cateBt
            // 
            this.cateBt.Location = new System.Drawing.Point(328, 157);
            this.cateBt.Name = "cateBt";
            this.cateBt.Size = new System.Drawing.Size(25, 21);
            this.cateBt.TabIndex = 43;
            this.cateBt.Text = "+";
            this.cateBt.UseVisualStyleBackColor = true;
            this.cateBt.Click += new System.EventHandler(this.cateBt_Click);
            // 
            // descrNUD
            // 
            this.descrNUD.Location = new System.Drawing.Point(105, 20);
            this.descrNUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.descrNUD.Name = "descrNUD";
            this.descrNUD.Size = new System.Drawing.Size(248, 20);
            this.descrNUD.TabIndex = 31;
            this.descrNUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "Category";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Description";
            // 
            // resetBt
            // 
            this.resetBt.Location = new System.Drawing.Point(328, 183);
            this.resetBt.Name = "resetBt";
            this.resetBt.Size = new System.Drawing.Size(25, 21);
            this.resetBt.TabIndex = 43;
            this.resetBt.Text = "+";
            this.resetBt.UseVisualStyleBackColor = true;
            this.resetBt.Click += new System.EventHandler(this.resetBt_Click);
            // 
            // propBt
            // 
            this.propBt.Location = new System.Drawing.Point(328, 212);
            this.propBt.Name = "propBt";
            this.propBt.Size = new System.Drawing.Size(25, 21);
            this.propBt.TabIndex = 43;
            this.propBt.Text = "+";
            this.propBt.UseVisualStyleBackColor = true;
            this.propBt.Click += new System.EventHandler(this.propBt_Click);
            // 
            // TrgRangBt2
            // 
            this.TrgRangBt2.Location = new System.Drawing.Point(328, 129);
            this.TrgRangBt2.Name = "TrgRangBt2";
            this.TrgRangBt2.Size = new System.Drawing.Size(25, 21);
            this.TrgRangBt2.TabIndex = 43;
            this.TrgRangBt2.Text = "+";
            this.TrgRangBt2.UseVisualStyleBackColor = true;
            this.TrgRangBt2.Click += new System.EventHandler(this.trgBt2_Click);
            // 
            // TrgRangBt1
            // 
            this.TrgRangBt1.Location = new System.Drawing.Point(207, 130);
            this.TrgRangBt1.Name = "TrgRangBt1";
            this.TrgRangBt1.Size = new System.Drawing.Size(25, 21);
            this.TrgRangBt1.TabIndex = 43;
            this.TrgRangBt1.Text = "+";
            this.TrgRangBt1.UseVisualStyleBackColor = true;
            this.TrgRangBt1.Click += new System.EventHandler(this.trgBt_Click);
            // 
            // rangeBt
            // 
            this.rangeBt.Location = new System.Drawing.Point(328, 102);
            this.rangeBt.Name = "rangeBt";
            this.rangeBt.Size = new System.Drawing.Size(25, 21);
            this.rangeBt.TabIndex = 43;
            this.rangeBt.Text = "+";
            this.rangeBt.UseVisualStyleBackColor = true;
            this.rangeBt.Click += new System.EventHandler(this.rangeBt_Click);
            // 
            // TargetRange2
            // 
            this.TargetRange2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TargetRange2.Location = new System.Drawing.Point(238, 131);
            this.TargetRange2.Name = "TargetRange2";
            this.TargetRange2.Size = new System.Drawing.Size(84, 19);
            this.TargetRange2.TabIndex = 42;
            this.TargetRange2.TextChanged += new System.EventHandler(this.TargetRange_TextChanged);
            // 
            // codeBt
            // 
            this.codeBt.Location = new System.Drawing.Point(328, 76);
            this.codeBt.Name = "codeBt";
            this.codeBt.Size = new System.Drawing.Size(25, 21);
            this.codeBt.TabIndex = 43;
            this.codeBt.Text = "+";
            this.codeBt.UseVisualStyleBackColor = true;
            this.codeBt.Click += new System.EventHandler(this.codeBt_Click);
            // 
            // TargetRange
            // 
            this.TargetRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TargetRange.Location = new System.Drawing.Point(105, 131);
            this.TargetRange.Name = "TargetRange";
            this.TargetRange.Size = new System.Drawing.Size(96, 19);
            this.TargetRange.TabIndex = 42;
            this.TargetRange.TextChanged += new System.EventHandler(this.TargetRange_TextChanged);
            // 
            // resetBox
            // 
            this.resetBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.resetBox.Location = new System.Drawing.Point(149, 185);
            this.resetBox.Name = "resetBox";
            this.resetBox.Size = new System.Drawing.Size(173, 19);
            this.resetBox.TabIndex = 42;
            // 
            // RangeBox
            // 
            this.RangeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RangeBox.Location = new System.Drawing.Point(105, 104);
            this.RangeBox.Name = "RangeBox";
            this.RangeBox.Size = new System.Drawing.Size(217, 19);
            this.RangeBox.TabIndex = 42;
            // 
            // CateBox
            // 
            this.CateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CateBox.Location = new System.Drawing.Point(105, 158);
            this.CateBox.Name = "CateBox";
            this.CateBox.Size = new System.Drawing.Size(217, 19);
            this.CateBox.TabIndex = 42;
            // 
            // propBox
            // 
            this.propBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.propBox.Location = new System.Drawing.Point(105, 212);
            this.propBox.Name = "propBox";
            this.propBox.Size = new System.Drawing.Size(217, 19);
            this.propBox.TabIndex = 42;
            // 
            // CodeBox
            // 
            this.CodeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CodeBox.Location = new System.Drawing.Point(105, 77);
            this.CodeBox.Name = "CodeBox";
            this.CodeBox.Size = new System.Drawing.Size(217, 19);
            this.CodeBox.TabIndex = 42;
            // 
            // typeBt
            // 
            this.typeBt.Location = new System.Drawing.Point(328, 49);
            this.typeBt.Name = "typeBt";
            this.typeBt.Size = new System.Drawing.Size(25, 21);
            this.typeBt.TabIndex = 43;
            this.typeBt.Text = "+";
            this.typeBt.UseVisualStyleBackColor = true;
            this.typeBt.Click += new System.EventHandler(this.typeBt_Click);
            // 
            // TypeBox
            // 
            this.TypeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TypeBox.Location = new System.Drawing.Point(105, 50);
            this.TypeBox.Name = "TypeBox";
            this.TypeBox.Size = new System.Drawing.Size(217, 19);
            this.TypeBox.TabIndex = 42;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 216);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Property";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(16, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "*Code";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(16, 53);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "*Type";
            // 
            // label15
            // 
            this.label15.AutoEllipsis = true;
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(16, 133);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 13);
            this.label15.TabIndex = 37;
            this.label15.Text = "*TargetRange";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(416, 113);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(121, 20);
            this.numericUpDown3.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(16, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "*Range";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Reset";
            // 
            // resetCountNUD
            // 
            this.resetCountNUD.Location = new System.Drawing.Point(105, 185);
            this.resetCountNUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.resetCountNUD.Name = "resetCountNUD";
            this.resetCountNUD.Size = new System.Drawing.Size(36, 20);
            this.resetCountNUD.TabIndex = 30;
            this.resetCountNUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // countNUD
            // 
            this.countNUD.Location = new System.Drawing.Point(105, 239);
            this.countNUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.countNUD.Name = "countNUD";
            this.countNUD.Size = new System.Drawing.Size(248, 20);
            this.countNUD.TabIndex = 30;
            this.countNUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "CountLimit";
            // 
            // groupAction
            // 
            this.groupAction.Controls.Add(this.opCBox);
            this.groupAction.Controls.Add(this.costCBox);
            this.groupAction.Controls.Add(this.conCBox);
            this.groupAction.Controls.Add(this.CostBox);
            this.groupAction.Controls.Add(this.opBox);
            this.groupAction.Controls.Add(this.conBox);
            this.groupAction.Controls.Add(this.label8);
            this.groupAction.Controls.Add(this.label11);
            this.groupAction.Controls.Add(this.label10);
            this.groupAction.Enabled = false;
            this.groupAction.Location = new System.Drawing.Point(528, 38);
            this.groupAction.Name = "groupAction";
            this.groupAction.Size = new System.Drawing.Size(257, 107);
            this.groupAction.TabIndex = 46;
            this.groupAction.TabStop = false;
            this.groupAction.Text = " trigger -type function";
            // 
            // opCBox
            // 
            this.opCBox.AutoSize = true;
            this.opCBox.Checked = true;
            this.opCBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.opCBox.Location = new System.Drawing.Point(236, 78);
            this.opCBox.Name = "opCBox";
            this.opCBox.Size = new System.Drawing.Size(15, 14);
            this.opCBox.TabIndex = 29;
            this.opCBox.UseVisualStyleBackColor = true;
            this.opCBox.CheckedChanged += new System.EventHandler(this.opCBox_CheckedChanged);
            // 
            // costCBox
            // 
            this.costCBox.AutoSize = true;
            this.costCBox.Checked = true;
            this.costCBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.costCBox.Location = new System.Drawing.Point(236, 51);
            this.costCBox.Name = "costCBox";
            this.costCBox.Size = new System.Drawing.Size(15, 14);
            this.costCBox.TabIndex = 29;
            this.costCBox.UseVisualStyleBackColor = true;
            this.costCBox.CheckedChanged += new System.EventHandler(this.costCBox_CheckedChanged);
            // 
            // conCBox
            // 
            this.conCBox.AutoSize = true;
            this.conCBox.Checked = true;
            this.conCBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.conCBox.Location = new System.Drawing.Point(236, 25);
            this.conCBox.Name = "conCBox";
            this.conCBox.Size = new System.Drawing.Size(15, 14);
            this.conCBox.TabIndex = 29;
            this.conCBox.UseVisualStyleBackColor = true;
            this.conCBox.CheckedChanged += new System.EventHandler(this.conCBox_CheckedChanged);
            // 
            // CostBox
            // 
            this.CostBox.Location = new System.Drawing.Point(100, 48);
            this.CostBox.Name = "CostBox";
            this.CostBox.Size = new System.Drawing.Size(121, 20);
            this.CostBox.TabIndex = 26;
            // 
            // opBox
            // 
            this.opBox.Location = new System.Drawing.Point(100, 75);
            this.opBox.Name = "opBox";
            this.opBox.Size = new System.Drawing.Size(121, 20);
            this.opBox.TabIndex = 26;
            // 
            // conBox
            // 
            this.conBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.conBox.Location = new System.Drawing.Point(100, 21);
            this.conBox.Name = "conBox";
            this.conBox.Size = new System.Drawing.Size(121, 20);
            this.conBox.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Condtion";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(28, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Cost";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Operation";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.trgCBox);
            this.groupBox5.Controls.Add(this.TrgBox);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Location = new System.Drawing.Point(528, 207);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(257, 53);
            this.groupBox5.TabIndex = 51;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " There are two";
            // 
            // trgCBox
            // 
            this.trgCBox.AutoSize = true;
            this.trgCBox.Checked = true;
            this.trgCBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trgCBox.Location = new System.Drawing.Point(236, 25);
            this.trgCBox.Name = "trgCBox";
            this.trgCBox.Size = new System.Drawing.Size(15, 14);
            this.trgCBox.TabIndex = 29;
            this.trgCBox.UseVisualStyleBackColor = true;
            this.trgCBox.CheckedChanged += new System.EventHandler(this.trgCBox_CheckedChanged);
            // 
            // TrgBox
            // 
            this.TrgBox.Location = new System.Drawing.Point(100, 20);
            this.TrgBox.Name = "TrgBox";
            this.TrgBox.Size = new System.Drawing.Size(121, 20);
            this.TrgBox.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Target";
            // 
            // EffectWizzard
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(824, 379);
            this.Controls.Add(this.panel1);
            this.Name = "EffectWizzard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EffectWizzard";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupEffect.ResumeLayout(false);
            this.groupEffect.PerformLayout();
            this.groupbox3.ResumeLayout(false);
            this.groupbox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.descrNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resetCountNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.countNUD)).EndInit();
            this.groupAction.ResumeLayout(false);
            this.groupAction.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        private void label15_Click(object sender, EventArgs e)
        {
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.loadEffectSet(this.listBox1.SelectedIndex);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void loadEffectSet(int index)
        {
            EffectSet set = this.effects[index];
            this.descrNUD.Value = set.Description;
            this.TypeBox.Text = set.Type;
            this.CodeBox.Text = set.Code;
            this.RangeBox.Text = set.Range;
            this.TargetRange.Text = set.TargetRange1;
            this.TargetRange2.Text = set.TargetRange2;
            this.resetBox.Text = set.Reset;
            this.resetCountNUD.Value = set.ResetCount;
            this.propBox.Text = set.Property;
            this.CateBox.Text = set.Category;
            this.countNUD.Value = set.CountLimit;
            this.conBox.Text = set.Condition;
            this.TrgBox.Text = set.Target;
            this.CostBox.Text = set.Cost;
            this.opBox.Text = set.Operation;
            this.varBox.Text = set.Value;
            this.nameBox.Text = set.Name;
            this.toCard.Checked = set.RegistTo == 0;
            this.conCBox.Checked = !string.IsNullOrEmpty(set.Condition);
            this.costCBox.Checked = !string.IsNullOrEmpty(set.Cost);
            this.opCBox.Checked = !string.IsNullOrEmpty(set.Operation);
            this.trgCBox.Checked = !string.IsNullOrEmpty(set.Target);
            this.varCBox.Checked = !string.IsNullOrEmpty(set.Value);
        }

        private void opCBox_CheckedChanged(object sender, EventArgs e)
        {
            this.opBox.Enabled = this.opCBox.Checked;
        }

        private void propBt_Click(object sender, EventArgs e)
        {
            SelectDlg dlg = new SelectDlg(4, -1, this.dlgxmlPath);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.propBox.Text = dlg.Value;
            }
        }

        private void rangeBt_Click(object sender, EventArgs e)
        {
            SelectDlg dlg = new SelectDlg(3, -1, this.dlgxmlPath);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.RangeBox.Text = dlg.Value;
            }
        }

        private void resetBt_Click(object sender, EventArgs e)
        {
            SelectDlg dlg = new SelectDlg(5, this.tmpltComb.SelectedIndex, this.dlgxmlPath);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.resetBox.Text = dlg.Value;
            }
        }

        public void setCID(string cID)
        {
            this.cID = cID;
        }

        private void setViewState(bool[] setting)
        {
            try
            {
                this.CodeBox.Enabled = this.codeBt.Enabled = setting[0];
                this.RangeBox.Enabled = this.rangeBt.Enabled = setting[1];
                this.TargetRange.Enabled = this.TrgRangBt1.Enabled = setting[2];
                this.TargetRange2.Enabled = this.TrgRangBt2.Enabled = setting[2];
                this.countNUD.Enabled = setting[3];
                this.groupAction.Enabled = setting[4];
                this.groupEffect.Enabled = setting[5];
            }
            catch (Exception)
            {
            }
        }

        private void TargetRange_TextChanged(object sender, EventArgs e)
        {
        }

        private void tmpltComb_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tmpltComb.SelectedIndex)
            {
                case 0:
                {
                    EffectWizzard wizzard = this;
                    bool[] flagArray = new bool[6];
                    flagArray[4] = true;
                    bool[] setting = flagArray;
                    wizzard.setViewState(setting);
                    break;
                }
                case 1:
                {
                    EffectWizzard wizzard2 = this;
                    bool[] flagArray3 = new bool[6];
                    flagArray3[5] = true;
                    bool[] flagArray4 = flagArray3;
                    wizzard2.setViewState(flagArray4);
                    break;
                }
            }
            this.clear();
        }

        private void trgBt_Click(object sender, EventArgs e)
        {
            SelectDlg dlg = new SelectDlg(3, -1, this.dlgxmlPath);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.TargetRange.Text = dlg.Value;
            }
        }

        private void trgBt2_Click(object sender, EventArgs e)
        {
            SelectDlg dlg = new SelectDlg(3, -1, this.dlgxmlPath);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.TargetRange2.Text = dlg.Value;
            }
        }

        private void trgCBox_CheckedChanged(object sender, EventArgs e)
        {
            this.TrgBox.Enabled = this.trgCBox.Checked;
        }

        private void typeBt_Click(object sender, EventArgs e)
        {
            SelectDlg dlg = new SelectDlg(0, this.tmpltComb.SelectedIndex, this.dlgxmlPath);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.TypeBox.Text = dlg.Value;
                this.setViewState(dlg.getViewSetting());
            }
        }

        private void varCBox_CheckedChanged(object sender, EventArgs e)
        {
            this.varBox.Enabled = this.varCBox.Checked;
        }
    }
}

