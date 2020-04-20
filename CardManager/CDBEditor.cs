using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using CardManager.Enums;
using Mono.Data.Sqlite;

namespace CardManager
{
    public partial class CDBEditor: Form
    {
        private System.ComponentModel.IContainer components = null;
        private const string Cdbdir = "cards.cdb";
        string m_loadedImage = "";
        Dictionary<int, string> m_setCodes;
        List<int> m_formats;
        List<int> m_cardRaces;
        List<int> m_cardAttributes;
        private Components.SearchBox searchBox1;
        private CheckBox checkBox1;
        private GroupBox groupBox3;
        int m_loadedCard;
        private GroupBox groupBox7;
        private TableLayoutPanel tableLayoutPanel8;
        private FlowLayoutPanel flowLayoutPanel4;
        private GroupBox groupBox5;
        private TableLayoutPanel tableLayoutPanel7;
        private ListBox EffectList;
        private TextBox EffectInput;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button DeleteEffectbtn;
        private Button MoveEffectUp;
        private Button MoveEffectDown;
        private Button AddEffectbtn;
        private Label label14;
        private CheckedListBox Effect2List;
        private CheckedListBox Effect1List;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button Clearbtn;
        private Button SaveCardbtn;
        private Button DeleteBtn;
        private Button CreateScriptBtn;
        private ComboBox comboBox1;
        private string currentexpansiondb;
        private Button ExtractScriptBtn;
        private bool expansionload;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.CardID = new System.Windows.Forms.TextBox();
            this.Alias = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CardFormats = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SetCodeOne = new System.Windows.Forms.ComboBox();
            this.SetCodeTwo = new System.Windows.Forms.ComboBox();
            this.SetCodeThree = new System.Windows.Forms.ComboBox();
            this.Level = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SetCodeFour = new System.Windows.Forms.ComboBox();
            this.DEF = new System.Windows.Forms.MaskedTextBox();
            this.ATK = new System.Windows.Forms.TextBox();
            this.CardAttribute = new System.Windows.Forms.ComboBox();
            this.Race = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.LScale = new System.Windows.Forms.ComboBox();
            this.RScale = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.Effect1List = new System.Windows.Forms.CheckedListBox();
            this.Effect2List = new System.Windows.Forms.CheckedListBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.CardName = new System.Windows.Forms.TextBox();
            this.CardDescription = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.EffectList = new System.Windows.Forms.ListBox();
            this.EffectInput = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.DeleteEffectbtn = new System.Windows.Forms.Button();
            this.MoveEffectUp = new System.Windows.Forms.Button();
            this.MoveEffectDown = new System.Windows.Forms.Button();
            this.AddEffectbtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CardTypeList = new System.Windows.Forms.CheckedListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.CategoryList = new System.Windows.Forms.CheckedListBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.OpenScriptBtn = new System.Windows.Forms.Button();
            this.ExtractScriptBtn = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.Clearbtn = new System.Windows.Forms.Button();
            this.SaveCardbtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.CreateScriptBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.CardImg = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.LoadImageBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.searchBox1 = new CardManager.Components.SearchBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CardImg)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 177F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(903, 593);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel4, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox4, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox5, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel3, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(180, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 371F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(720, 587);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 557);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(294, 27);
            this.flowLayoutPanel4.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 365);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Card Info";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.06897F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.93103F));
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.CardID, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.Alias, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.CardFormats, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.SetCodeOne, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.SetCodeTwo, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.SetCodeThree, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.Level, 1, 7);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.SetCodeFour, 1, 6);
            this.tableLayoutPanel4.Controls.Add(this.DEF, 1, 13);
            this.tableLayoutPanel4.Controls.Add(this.ATK, 1, 12);
            this.tableLayoutPanel4.Controls.Add(this.CardAttribute, 1, 11);
            this.tableLayoutPanel4.Controls.Add(this.Race, 1, 10);
            this.tableLayoutPanel4.Controls.Add(this.label9, 0, 13);
            this.tableLayoutPanel4.Controls.Add(this.label8, 0, 12);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 11);
            this.tableLayoutPanel4.Controls.Add(this.label6, 0, 10);
            this.tableLayoutPanel4.Controls.Add(this.label12, 0, 8);
            this.tableLayoutPanel4.Controls.Add(this.label13, 0, 9);
            this.tableLayoutPanel4.Controls.Add(this.LScale, 1, 8);
            this.tableLayoutPanel4.Controls.Add(this.RScale, 1, 9);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 14;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(288, 346);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Alias";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CardID
            // 
            this.CardID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CardID.Location = new System.Drawing.Point(109, 3);
            this.CardID.Name = "CardID";
            this.CardID.Size = new System.Drawing.Size(176, 20);
            this.CardID.TabIndex = 0;
            // 
            // Alias
            // 
            this.Alias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Alias.Location = new System.Drawing.Point(109, 28);
            this.Alias.Name = "Alias";
            this.Alias.Size = new System.Drawing.Size(176, 20);
            this.Alias.TabIndex = 1;
            this.Alias.Text = "0";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "ID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CardFormats
            // 
            this.CardFormats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CardFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CardFormats.FormattingEnabled = true;
            this.CardFormats.Location = new System.Drawing.Point(109, 53);
            this.CardFormats.Name = "CardFormats";
            this.CardFormats.Size = new System.Drawing.Size(176, 21);
            this.CardFormats.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Card Format";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 25);
            this.label4.TabIndex = 18;
            this.label4.Text = "Set Codes";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SetCodeOne
            // 
            this.SetCodeOne.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetCodeOne.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SetCodeOne.FormattingEnabled = true;
            this.SetCodeOne.Location = new System.Drawing.Point(109, 78);
            this.SetCodeOne.Name = "SetCodeOne";
            this.SetCodeOne.Size = new System.Drawing.Size(176, 21);
            this.SetCodeOne.TabIndex = 19;
            // 
            // SetCodeTwo
            // 
            this.SetCodeTwo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetCodeTwo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SetCodeTwo.FormattingEnabled = true;
            this.SetCodeTwo.Location = new System.Drawing.Point(109, 103);
            this.SetCodeTwo.Name = "SetCodeTwo";
            this.SetCodeTwo.Size = new System.Drawing.Size(176, 21);
            this.SetCodeTwo.TabIndex = 20;
            // 
            // SetCodeThree
            // 
            this.SetCodeThree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetCodeThree.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SetCodeThree.FormattingEnabled = true;
            this.SetCodeThree.Location = new System.Drawing.Point(109, 128);
            this.SetCodeThree.Name = "SetCodeThree";
            this.SetCodeThree.Size = new System.Drawing.Size(176, 21);
            this.SetCodeThree.TabIndex = 31;
            // 
            // Level
            // 
            this.Level.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Level.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Level.FormattingEnabled = true;
            this.Level.Location = new System.Drawing.Point(109, 178);
            this.Level.Name = "Level";
            this.Level.Size = new System.Drawing.Size(176, 21);
            this.Level.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 25);
            this.label5.TabIndex = 21;
            this.label5.Text = "Level";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SetCodeFour
            // 
            this.SetCodeFour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetCodeFour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SetCodeFour.FormattingEnabled = true;
            this.SetCodeFour.Location = new System.Drawing.Point(109, 153);
            this.SetCodeFour.Name = "SetCodeFour";
            this.SetCodeFour.Size = new System.Drawing.Size(176, 21);
            this.SetCodeFour.TabIndex = 32;
            // 
            // DEF
            // 
            this.DEF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DEF.Location = new System.Drawing.Point(109, 328);
            this.DEF.Name = "DEF";
            this.DEF.Size = new System.Drawing.Size(176, 20);
            this.DEF.TabIndex = 30;
            this.DEF.Text = "0";
            // 
            // ATK
            // 
            this.ATK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ATK.Location = new System.Drawing.Point(109, 303);
            this.ATK.Name = "ATK";
            this.ATK.Size = new System.Drawing.Size(176, 20);
            this.ATK.TabIndex = 29;
            this.ATK.Text = "0";
            // 
            // CardAttribute
            // 
            this.CardAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CardAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CardAttribute.FormattingEnabled = true;
            this.CardAttribute.Location = new System.Drawing.Point(109, 278);
            this.CardAttribute.Name = "CardAttribute";
            this.CardAttribute.Size = new System.Drawing.Size(176, 21);
            this.CardAttribute.TabIndex = 28;
            // 
            // Race
            // 
            this.Race.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Race.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Race.FormattingEnabled = true;
            this.Race.Location = new System.Drawing.Point(109, 253);
            this.Race.Name = "Race";
            this.Race.Size = new System.Drawing.Size(176, 21);
            this.Race.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 325);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 24);
            this.label9.TabIndex = 25;
            this.label9.Text = "DEF";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(3, 300);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 25);
            this.label8.TabIndex = 24;
            this.label8.Text = "ATK";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(3, 275);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 25);
            this.label7.TabIndex = 23;
            this.label7.Text = "Attribute";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 250);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 25);
            this.label6.TabIndex = 22;
            this.label6.Text = "Race";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(63, 206);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 13);
            this.label12.TabIndex = 33;
            this.label12.Text = "LScale";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(61, 231);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(42, 13);
            this.label13.TabIndex = 34;
            this.label13.Text = "RScale";
            // 
            // LScale
            // 
            this.LScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LScale.FormattingEnabled = true;
            this.LScale.Location = new System.Drawing.Point(109, 203);
            this.LScale.Name = "LScale";
            this.LScale.Size = new System.Drawing.Size(176, 21);
            this.LScale.TabIndex = 35;
            // 
            // RScale
            // 
            this.RScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RScale.FormattingEnabled = true;
            this.RScale.Location = new System.Drawing.Point(109, 228);
            this.RScale.Name = "RScale";
            this.RScale.Size = new System.Drawing.Size(176, 21);
            this.RScale.TabIndex = 36;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox7);
            this.groupBox4.Controls.Add(this.tableLayoutPanel6);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(303, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(414, 365);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Card Text";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tableLayoutPanel8);
            this.groupBox7.Location = new System.Drawing.Point(6, 196);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(402, 163);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Card Effect";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.75309F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.24691F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 233F));
            this.tableLayoutPanel8.Controls.Add(this.label14, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.Effect1List, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.Effect2List, 2, 1);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(-1, 19);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(405, 135);
            this.tableLayoutPanel8.TabIndex = 2;
            // 
            // Effect1List
            // 
            this.Effect1List.FormattingEnabled = true;
            this.Effect1List.Location = new System.Drawing.Point(36, 60);
            this.Effect1List.Name = "Effect1List";
            this.Effect1List.Size = new System.Drawing.Size(132, 49);
            this.Effect1List.TabIndex = 2;
            // 
            // Effect2List
            // 
            this.Effect2List.FormattingEnabled = true;
            this.Effect2List.Location = new System.Drawing.Point(174, 60);
            this.Effect2List.Name = "Effect2List";
            this.tableLayoutPanel8.SetRowSpan(this.Effect2List, 2);
            this.Effect2List.Size = new System.Drawing.Size(136, 49);
            this.Effect2List.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(36, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(67, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Effect Types";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.58124F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.41875F));
            this.tableLayoutPanel6.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.CardName, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.CardDescription, 1, 1);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(408, 174);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(94, 6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Name";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(69, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Description";
            // 
            // CardName
            // 
            this.CardName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CardName.Location = new System.Drawing.Point(135, 3);
            this.CardName.Name = "CardName";
            this.CardName.Size = new System.Drawing.Size(270, 20);
            this.CardName.TabIndex = 2;
            // 
            // CardDescription
            // 
            this.CardDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CardDescription.Location = new System.Drawing.Point(135, 28);
            this.CardDescription.Multiline = true;
            this.CardDescription.Name = "CardDescription";
            this.CardDescription.Size = new System.Drawing.Size(270, 143);
            this.CardDescription.TabIndex = 3;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tableLayoutPanel7);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(303, 374);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(414, 141);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Optional Card Effect Text";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.95454F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.04546F));
            this.tableLayoutPanel7.Controls.Add(this.EffectList, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.EffectInput, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.AddEffectbtn, 0, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.02098F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.97902F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(408, 122);
            this.tableLayoutPanel7.TabIndex = 4;
            // 
            // EffectList
            // 
            this.EffectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EffectList.FormattingEnabled = true;
            this.EffectList.Location = new System.Drawing.Point(137, 3);
            this.EffectList.Name = "EffectList";
            this.EffectList.Size = new System.Drawing.Size(268, 90);
            this.EffectList.TabIndex = 1;
            // 
            // EffectInput
            // 
            this.EffectInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EffectInput.Location = new System.Drawing.Point(137, 99);
            this.EffectInput.Name = "EffectInput";
            this.EffectInput.Size = new System.Drawing.Size(268, 20);
            this.EffectInput.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.DeleteEffectbtn);
            this.flowLayoutPanel1.Controls.Add(this.MoveEffectUp);
            this.flowLayoutPanel1.Controls.Add(this.MoveEffectDown);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(128, 90);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // DeleteEffectbtn
            // 
            this.DeleteEffectbtn.Location = new System.Drawing.Point(50, 3);
            this.DeleteEffectbtn.Name = "DeleteEffectbtn";
            this.DeleteEffectbtn.Size = new System.Drawing.Size(75, 23);
            this.DeleteEffectbtn.TabIndex = 1;
            this.DeleteEffectbtn.Text = "Delete";
            this.DeleteEffectbtn.UseVisualStyleBackColor = true;
            this.DeleteEffectbtn.Click += new System.EventHandler(this.DeleteEffectbtn_Click);
            // 
            // MoveEffectUp
            // 
            this.MoveEffectUp.Location = new System.Drawing.Point(50, 32);
            this.MoveEffectUp.Name = "MoveEffectUp";
            this.MoveEffectUp.Size = new System.Drawing.Size(75, 23);
            this.MoveEffectUp.TabIndex = 2;
            this.MoveEffectUp.Text = "Up";
            this.MoveEffectUp.UseVisualStyleBackColor = true;
            this.MoveEffectUp.Click += new System.EventHandler(this.MoveEffectUp_Click);
            // 
            // MoveEffectDown
            // 
            this.MoveEffectDown.Location = new System.Drawing.Point(50, 61);
            this.MoveEffectDown.Name = "MoveEffectDown";
            this.MoveEffectDown.Size = new System.Drawing.Size(75, 23);
            this.MoveEffectDown.TabIndex = 3;
            this.MoveEffectDown.Text = "Down";
            this.MoveEffectDown.UseVisualStyleBackColor = true;
            this.MoveEffectDown.Click += new System.EventHandler(this.MoveEffectDown_Click);
            // 
            // AddEffectbtn
            // 
            this.AddEffectbtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.AddEffectbtn.Location = new System.Drawing.Point(56, 99);
            this.AddEffectbtn.Name = "AddEffectbtn";
            this.AddEffectbtn.Size = new System.Drawing.Size(75, 20);
            this.AddEffectbtn.TabIndex = 4;
            this.AddEffectbtn.Text = "Add";
            this.AddEffectbtn.UseVisualStyleBackColor = true;
            this.AddEffectbtn.Click += new System.EventHandler(this.AddEffectbtn_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.groupBox6, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 374);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(294, 141);
            this.tableLayoutPanel5.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CardTypeList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(141, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Card Types";
            // 
            // CardTypeList
            // 
            this.CardTypeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CardTypeList.FormattingEnabled = true;
            this.CardTypeList.Location = new System.Drawing.Point(3, 16);
            this.CardTypeList.Name = "CardTypeList";
            this.CardTypeList.Size = new System.Drawing.Size(135, 116);
            this.CardTypeList.TabIndex = 0;
            this.CardTypeList.SelectedIndexChanged += new System.EventHandler(this.CardTypeList_SelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.CategoryList);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(150, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(141, 135);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Card Category";
            // 
            // CategoryList
            // 
            this.CategoryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CategoryList.FormattingEnabled = true;
            this.CategoryList.Items.AddRange(new object[] {
            "S/T Destory",
            "Destory Monster",
            "Banish",
            "Graveyard",
            "Back to Hand",
            "Back to Deck",
            "Destory Hand",
            "Destory Deck",
            "Draw",
            "Search",
            "Recovery",
            "Position",
            "Control",
            "Change ATK/DEF",
            "Piercing",
            "Repeat Attack",
            "Limit Attack",
            "Direct Attack",
            "Special Summon",
            "Token",
            "Type-Related",
            "Property-Related",
            "Damage LP",
            "Recover LP",
            "Destory",
            "Select",
            "Counter",
            "Gamble",
            "Fusion-Related",
            "Tuner-Related",
            "Xyz-Related",
            "Negate Effect"});
            this.CategoryList.Location = new System.Drawing.Point(3, 16);
            this.CategoryList.Name = "CategoryList";
            this.CategoryList.Size = new System.Drawing.Size(135, 116);
            this.CategoryList.TabIndex = 0;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.OpenScriptBtn);
            this.flowLayoutPanel3.Controls.Add(this.ExtractScriptBtn);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 521);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(294, 30);
            this.flowLayoutPanel3.TabIndex = 7;
            // 
            // OpenScriptBtn
            // 
            this.OpenScriptBtn.Location = new System.Drawing.Point(179, 3);
            this.OpenScriptBtn.Name = "OpenScriptBtn";
            this.OpenScriptBtn.Size = new System.Drawing.Size(112, 23);
            this.OpenScriptBtn.TabIndex = 0;
            this.OpenScriptBtn.Text = "Open Script";
            this.OpenScriptBtn.UseVisualStyleBackColor = true;
            this.OpenScriptBtn.Click += new System.EventHandler(this.OpenScriptBtn_Click_1);
            // 
            // ExtractScriptBtn
            // 
            this.ExtractScriptBtn.Location = new System.Drawing.Point(61, 3);
            this.ExtractScriptBtn.Name = "ExtractScriptBtn";
            this.ExtractScriptBtn.Size = new System.Drawing.Size(112, 23);
            this.ExtractScriptBtn.TabIndex = 1;
            this.ExtractScriptBtn.Text = "Extract Script";
            this.ExtractScriptBtn.UseVisualStyleBackColor = true;
            this.ExtractScriptBtn.Click += new System.EventHandler(this.ExtractScriptBtn_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.Clearbtn);
            this.flowLayoutPanel2.Controls.Add(this.SaveCardbtn);
            this.flowLayoutPanel2.Controls.Add(this.DeleteBtn);
            this.flowLayoutPanel2.Controls.Add(this.CreateScriptBtn);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(303, 521);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(414, 27);
            this.flowLayoutPanel2.TabIndex = 10;
            // 
            // Clearbtn
            // 
            this.Clearbtn.Location = new System.Drawing.Point(328, 3);
            this.Clearbtn.Name = "Clearbtn";
            this.Clearbtn.Size = new System.Drawing.Size(83, 23);
            this.Clearbtn.TabIndex = 0;
            this.Clearbtn.Text = "Clear";
            this.Clearbtn.UseVisualStyleBackColor = true;
            this.Clearbtn.Click += new System.EventHandler(this.Clearbtn_Click);
            // 
            // SaveCardbtn
            // 
            this.SaveCardbtn.Location = new System.Drawing.Point(239, 3);
            this.SaveCardbtn.Name = "SaveCardbtn";
            this.SaveCardbtn.Size = new System.Drawing.Size(83, 23);
            this.SaveCardbtn.TabIndex = 1;
            this.SaveCardbtn.Text = "Save Card";
            this.SaveCardbtn.UseVisualStyleBackColor = true;
            this.SaveCardbtn.Click += new System.EventHandler(this.SaveCardbtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(150, 3);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(83, 23);
            this.DeleteBtn.TabIndex = 2;
            this.DeleteBtn.Text = "Delete Card";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // CreateScriptBtn
            // 
            this.CreateScriptBtn.Location = new System.Drawing.Point(32, 3);
            this.CreateScriptBtn.Name = "CreateScriptBtn";
            this.CreateScriptBtn.Size = new System.Drawing.Size(112, 23);
            this.CreateScriptBtn.TabIndex = 11;
            this.CreateScriptBtn.Text = "Create Script";
            this.CreateScriptBtn.UseVisualStyleBackColor = true;
            this.CreateScriptBtn.Click += new System.EventHandler(this.CreateScriptBtn_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.comboBox1, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.CardImg, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBox1, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.LoadImageBtn, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.searchBox1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 254F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 219F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(171, 587);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 564);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // CardImg
            // 
            this.CardImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CardImg.ErrorImage = null;
            this.CardImg.Location = new System.Drawing.Point(3, 3);
            this.CardImg.Name = "CardImg";
            this.CardImg.Size = new System.Drawing.Size(165, 248);
            this.CardImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CardImg.TabIndex = 0;
            this.CardImg.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(3, 534);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(139, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Enable expansion cards";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // LoadImageBtn
            // 
            this.LoadImageBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LoadImageBtn.Location = new System.Drawing.Point(48, 257);
            this.LoadImageBtn.Name = "LoadImageBtn";
            this.LoadImageBtn.Size = new System.Drawing.Size(75, 23);
            this.LoadImageBtn.TabIndex = 1;
            this.LoadImageBtn.Text = "Set Image";
            this.LoadImageBtn.UseVisualStyleBackColor = true;
            this.LoadImageBtn.Click += new System.EventHandler(this.LoadImageBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(3, 505);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(165, 23);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Expansion Cards";
            // 
            // searchBox1
            // 
            this.searchBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchBox1.Location = new System.Drawing.Point(3, 286);
            this.searchBox1.Name = "searchBox1";
            this.searchBox1.Size = new System.Drawing.Size(165, 213);
            this.searchBox1.TabIndex = 2;
            this.searchBox1.TabStop = false;
            this.searchBox1.Text = "Search";
            // 
            // CDBEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(903, 593);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CDBEditor";
            this.Text = "CDBEditor";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CardImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


        public CDBEditor ()
		{
			InitializeComponent();
            TopLevel = false;
            Dock = DockStyle.Fill;
            Visible = true;
            searchBox1.List.DoubleClick += CardList_DoubleClick;
            SetDataTypes();
            LoadData(Cdbdir);
            LScale.SelectedIndex = 0;
            RScale.SelectedIndex = 0;
        }

        private void SetDataTypes()
        {
            LoadCardFormatsFromFile("Assets\\cardformats.txt");
            LoadCardRacesFromFile("Assets\\cardraces.txt");
            LoadCardAttributesFromFile("assets\\cardattributes.txt");
            LoadExpansionCbdList(AppDomain.CurrentDomain.BaseDirectory + "\\expansions");
            for (int i = 1; i <= 13; i++)
                Level.Items.Add("★" + i);
            for (int i = 0; i < 14; i++)
                LScale.Items.Add(i);
            for (int i = 0; i < 14; i++)
                RScale.Items.Add(i);
            if (!LoadSetCodesFromFile("strings.conf"))
                LoadSetCodesFromOldFile("Assets\\setname.txt");
            CardTypeList.Items.AddRange(Enum.GetNames(typeof(CardType)));
        }

        private void CardList_DoubleClick(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex < 0) return;

            int current = Int32.Parse(list.SelectedItem.ToString());
            if (!LoadCard(current))
            {
                MessageBox.Show("Error Loading card", "Error", MessageBoxButtons.OK);
            }
            else
            {
                LoadCardImage(current);
            }
        }

        private bool LoadExpansionCbdList(string dir)
        {
            DirectoryInfo dirinfo = new DirectoryInfo(dir);

            if (dirinfo.Exists)
                foreach (FileInfo file in dirinfo.GetFiles())

                    if (file.Extension == ".cdb")
                        comboBox1.Items.Add(file.Name);

                            return true;

        }

        private bool LoadSetCodesFromFile(string filedir)
        {
            m_setCodes = new Dictionary<int, string>();
            List<string> setnames = new List<string>();

            if (!File.Exists(filedir))
                return false;

            m_setCodes.Add(0, "None");

            var reader = new StreamReader(File.OpenRead(filedir));
            while (!reader.EndOfStream)
            {
                //!setcode 0x8d Ghostrick
                string line = reader.ReadLine();
                if (line == null || !line.StartsWith("!setcode")) continue;
                string[] parts = line.Split(' ');
                if (parts.Length == 1) continue;

                int setcode = Convert.ToInt32(parts[1], 16);
                string setname = line.Split(new string[] { parts[1] }, StringSplitOptions.RemoveEmptyEntries)[1];

                if (!m_setCodes.ContainsKey(setcode))
                {
                    setnames.Add(setname);
                    m_setCodes.Add(setcode, setname);
                }
            }
            if (setnames.Count == 0)
                return false;

            setnames.Sort();
            setnames.Insert(0, "None");
            SetCodeOne.Items.AddRange(setnames.ToArray());
            SetCodeTwo.Items.AddRange(setnames.ToArray());
            SetCodeThree.Items.AddRange(setnames.ToArray());
            SetCodeFour.Items.AddRange(setnames.ToArray());

            return true;
        }

        private void LoadSetCodesFromOldFile(string filedir)
        {
            m_setCodes = new Dictionary<int, string>();
            List<string> setnames = new List<string>();

            if (!File.Exists(filedir))
                return;

            var reader = new StreamReader(File.OpenRead(filedir));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line == null) continue;
                string[] parts = line.Split(' ');
                if (parts.Length == 1) continue;
                string setname = line.Substring(parts[0].Length, line.Length - parts[0].Length).Trim();
                int setcode = Convert.ToInt32(parts[0], 16);

                if (!m_setCodes.ContainsKey(setcode))
                {
                    setnames.Add(setname);
                    m_setCodes.Add(setcode, setname);
                }
            }
            setnames.Sort();
            SetCodeOne.Items.AddRange(setnames.ToArray());
            SetCodeTwo.Items.AddRange(setnames.ToArray());
            SetCodeThree.Items.AddRange(setnames.ToArray());
            SetCodeFour.Items.AddRange(setnames.ToArray());
        }

        private void LoadCardFormatsFromFile(string filedir)
        {
            m_formats = new List<int>();

            if (!File.Exists(filedir))
            {
                return;
            }

            var reader = new StreamReader(File.OpenRead(filedir));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line == null) continue;
                string[] parts = line.Split(' ');
                if (parts.Length == 1) continue;
                string formatname = line.Substring(parts[0].Length, line.Length - parts[0].Length).Trim();

                CardFormats.Items.Add(formatname);
                m_formats.Add(Convert.ToInt32(parts[0], 16));

            }
        }

        private void LoadCardRacesFromFile(string filedir)
        {
            m_cardRaces = new List<int>();

            if (!File.Exists(filedir))
            {
                return;
            }

            var reader = new StreamReader(File.OpenRead(filedir));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line == null) continue;
                string[] parts = line.Split(' ');
                if (parts.Length == 1) continue;
                string racename = line.Substring(parts[0].Length, line.Length - parts[0].Length).Trim();

                Race.Items.Add(racename);
                m_cardRaces.Add(Convert.ToInt32(parts[0], 16));

            }
        }

        private void LoadCardAttributesFromFile(string filedir)
        {
            m_cardAttributes = new List<int>();

            if (!File.Exists(filedir))
            {
                return;
            }

            var reader = new StreamReader(File.OpenRead(filedir));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line == null) continue;
                string[] parts = line.Split(' ');
                if (parts.Length == 1) continue;
                string attributename = line.Substring(parts[0].Length, line.Length - parts[0].Length).Trim();

                CardAttribute.Items.Add(attributename);
                m_cardAttributes.Add(Convert.ToInt32(parts[0], 16));

            }
        }

        private bool LoadCard(int cardid)
        {
            if (!Program.CardData.ContainsKey(cardid))
                return false;

            Clearbtn_Click(null, EventArgs.Empty);
            CardInfo info = Program.CardData[cardid];

            CardID.Text = info.Id.ToString(CultureInfo.InvariantCulture);
            Alias.Text = info.AliasId.ToString(CultureInfo.InvariantCulture);
            for (int i = 0; i < m_formats.Count; i++)
            {
                if (m_formats[i] == info.Ot)
                {
                    CardFormats.SelectedIndex = i;
                    break;
                }
            }
            Level.SelectedIndex = (int)info.Level - 1;
            RScale.SelectedIndex = (int)info.RScale;
            LScale.SelectedIndex = (int)info.LScale;
            for (int i = 0; i < m_cardRaces.Count; i++)
            {
                if (m_cardRaces[i] == info.Race)
                {
                    Race.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < m_cardAttributes.Count; i++)
            {
                if (m_cardAttributes[i] == info.Attribute)
                {
                    CardAttribute.SelectedIndex = i;
                    break;
                }
            }
            ATK.Text = info.Atk.ToString(CultureInfo.InvariantCulture);
            DEF.Text = info.Def.ToString(CultureInfo.InvariantCulture);
            CardName.Text = info.Name;
            CardDescription.Text = info.Description;
            foreach (string effect in info.EffectStrings)
                EffectList.Items.Add(effect);
            SetCardTypes(info.GetCardTypes());

            long setcode = info.SetCode & 0xffff;
            if (m_setCodes.ContainsKey((int)setcode))
                SetCodeOne.SelectedItem = m_setCodes[(int)setcode];
            else
                SetCodeOne.SelectedItem = m_setCodes[0];
            setcode = info.SetCode >> 16 & 0xffff;
            if (m_setCodes.ContainsKey((int)setcode))
                SetCodeTwo.SelectedItem = m_setCodes[(int)setcode];
            else
                SetCodeTwo.SelectedItem = m_setCodes[0];
            setcode = info.SetCode >> 32 & 0xffff;
            if (m_setCodes.ContainsKey((int)setcode))
                SetCodeThree.SelectedItem = m_setCodes[(int)setcode];
            else
                SetCodeThree.SelectedItem = m_setCodes[0];
            setcode = info.SetCode >> 48 & 0xffff;
            if (m_setCodes.ContainsKey((int)setcode))
                SetCodeFour.SelectedItem = m_setCodes[(int)setcode];
            else
                SetCodeFour.SelectedItem = m_setCodes[0];
            SetCategoryCheckBoxs(info.Category);

            m_loadedCard = info.Id;

            return true;
        }

        private void SetCardTypes(IEnumerable<CardType> types)
        {
            foreach (var cardtype in types)
            {
                switch (cardtype)
                {
                    case CardType.Monster:
                        CardTypeList.SetItemCheckState(0, CheckState.Checked);
                        break;
                    case CardType.Spell:
                        CardTypeList.SetItemCheckState(1, CheckState.Checked);
                        break;
                    case CardType.Trap:
                        CardTypeList.SetItemCheckState(2, CheckState.Checked);
                        break;
                    case CardType.Normal:
                        CardTypeList.SetItemCheckState(3, CheckState.Checked);
                        break;
                    case CardType.Effect:
                        CardTypeList.SetItemCheckState(4, CheckState.Checked);
                        break;
                    case CardType.Fusion:
                        CardTypeList.SetItemCheckState(5, CheckState.Checked);
                        break;
                    case CardType.Ritual:
                        CardTypeList.SetItemCheckState(6, CheckState.Checked);
                        break;
                    case CardType.TrapMonster:
                        CardTypeList.SetItemCheckState(7, CheckState.Checked);
                        break;
                    case CardType.Spirit:
                        CardTypeList.SetItemCheckState(8, CheckState.Checked);
                        break;
                    case CardType.Union:
                        CardTypeList.SetItemCheckState(9, CheckState.Checked);
                        break;
                    case CardType.Gemini:
                        CardTypeList.SetItemCheckState(10, CheckState.Checked);
                        break;
                    case CardType.Tuner:
                        CardTypeList.SetItemCheckState(11, CheckState.Checked);
                        break;
                    case CardType.Synchro:
                        CardTypeList.SetItemCheckState(12, CheckState.Checked);
                        break;
                    case CardType.Token:
                        CardTypeList.SetItemCheckState(13, CheckState.Checked);
                        break;
                    case CardType.QuickPlay:
                        CardTypeList.SetItemCheckState(14, CheckState.Checked);
                        break;
                    case CardType.Continuous:
                        CardTypeList.SetItemCheckState(15, CheckState.Checked);
                        break;
                    case CardType.Equip:
                        CardTypeList.SetItemCheckState(16, CheckState.Checked);
                        break;
                    case CardType.Field:
                        CardTypeList.SetItemCheckState(17, CheckState.Checked);
                        break;
                    case CardType.Counter:
                        CardTypeList.SetItemCheckState(18, CheckState.Checked);
                        break;
                    case CardType.Flip:
                        CardTypeList.SetItemCheckState(19, CheckState.Checked);
                        break;
                    case CardType.Toon:
                        CardTypeList.SetItemCheckState(20, CheckState.Checked);
                        break;
                    case CardType.Xyz:
                        CardTypeList.SetItemCheckState(21, CheckState.Checked);
                        break;
                    case CardType.Pendulum:
                        CardTypeList.SetItemCheckState(22, CheckState.Checked);
                        break;
                }
            }
        }

        private int GetCategoryNumber()
        {
            int selectedIndex = 0;
            int num2 = 1;
            int num3 = 0;
            while (num3 < CategoryList.Items.Count)
            {
                if (CategoryList.GetItemCheckState(num3) == CheckState.Checked)
                {
                    selectedIndex |= num2;
                }
                num3++;
                num2 = num2 << 1;
            }

            return selectedIndex;
        }

        private void SetCategoryCheckBoxs(long categorynumber)
        {
            int index = 0;
            int num;
            for (num = 1; index < CategoryList.Items.Count; num = num << 1)
            {
                CategoryList.SetItemCheckState(index,
                                                    (num & categorynumber) != 0L
                                                        ? CheckState.Checked
                                                        : CheckState.Unchecked);
                index++;
            }
        }

        private void LoadData(string dataloc)
        {
            string str = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string str2 = Path.Combine(str, dataloc);
            if (!File.Exists(str2))
            {
                MessageBox.Show(dataloc + " not found.");
                return;
            }
            //LoadData(cdbdir, "SELECT id, ot, alias, setcode, type, level, race, attribute, atk, def, category FROM datas", cdbdata);
            //LoadData(cdbdir, "SELECT id, name, desc, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, str11, str12, str13, str14, str15, str16 FROM texts", cdbenglishtext);



            var connection = new SqliteConnection("Data Source=" + str2);
            connection.Open();

            SqliteCommand datacommand = new SqliteCommand("SELECT id, ot, alias, setcode, type, level, race, attribute, atk, def, category FROM datas", connection);
            SqliteCommand textcommand = new SqliteCommand("SELECT id, name, desc, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, str11, str12, str13, str14, str15, str16 FROM texts", connection);
            List<string[]> datas = DatabaseHelper.ExecuteStringCommand(datacommand, 11);
            List<string[]> texts = DatabaseHelper.ExecuteStringCommand(textcommand, 0x13);

            foreach (string[] row in datas)
            {
                if (!Program.CardData.ContainsKey(Int32.Parse(row[0])))
                    Program.CardData.Add(Int32.Parse(row[0]), new CardInfo(row));
            }
            foreach (string[] row in texts)
            {
                if (Program.CardData.ContainsKey(Int32.Parse(row[0])))
                    Program.CardData[Int32.Parse(row[0])].SetCardText(row);
            }
            connection.Close();
        }

        private void LoadExpansionData(string dataloc)
        {
            string filest = "";

            if (dataloc != null)
                {
            string str = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory + "/expansions/");
            filest = Path.Combine(str, dataloc);
                
            

            if (!File.Exists(filest))
            {
                MessageBox.Show(dataloc + " not found.");
                return;
            }
            //LoadData(cdbdir, "SELECT id, ot, alias, setcode, type, level, race, attribute, atk, def, category FROM datas", cdbdata);
            //LoadData(cdbdir, "SELECT id, name, desc, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, str11, str12, str13, str14, str15, str16 FROM texts", cdbenglishtext);



            var connection = new SqliteConnection("Data Source=" + filest);
            connection.Open();

            SqliteCommand datacommand = new SqliteCommand("SELECT id, ot, alias, setcode, type, level, race, attribute, atk, def, category FROM datas", connection);
            SqliteCommand textcommand = new SqliteCommand("SELECT id, name, desc, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, str11, str12, str13, str14, str15, str16 FROM texts", connection);
            List<string[]> datas = DatabaseHelper.ExecuteStringCommand(datacommand, 11);
            List<string[]> texts = DatabaseHelper.ExecuteStringCommand(textcommand, 0x13);

            foreach (string[] row in datas)
            {
                if (!Program.CardData.ContainsKey(Int32.Parse(row[0])))
                    Program.CardData.Add(Int32.Parse(row[0]), new CardInfo(row));
            }
            foreach (string[] row in texts)
            {
                if (Program.CardData.ContainsKey(Int32.Parse(row[0])))
                    Program.CardData[Int32.Parse(row[0])].SetCardText(row);
            }
            connection.Close();

                }
            

        }

        private void LoadCardImage(int id)
        {
            if (File.Exists("pics//" + id + ".jpg"))
            {
                using (var stream = new FileStream("pics//" + id + ".jpg", FileMode.Open, FileAccess.Read))
                {
                    CardImg.Image = Image.FromStream(stream);
                }
            }
            else if (File.Exists("expansions//pics//" + id + ".jpg"))
            {
                using (var stream = new FileStream("expansions//pics//" + id + ".jpg", FileMode.Open, FileAccess.Read))
                {
                    CardImg.Image = Image.FromStream(stream);
                }
            }

            else

            {
                CardImg.Image = null;
            }
        }

         private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
         private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
         private System.Windows.Forms.GroupBox groupBox2;
         private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
         private System.Windows.Forms.Label label2;
         private System.Windows.Forms.TextBox CardID;
         private System.Windows.Forms.TextBox Alias;
         private System.Windows.Forms.Label label1;
         private System.Windows.Forms.ComboBox CardFormats;
         private System.Windows.Forms.Label label3;
         private System.Windows.Forms.Label label4;
         private System.Windows.Forms.ComboBox SetCodeOne;
         private System.Windows.Forms.ComboBox SetCodeTwo;
         private System.Windows.Forms.Label label5;
         private System.Windows.Forms.Label label6;
         private System.Windows.Forms.Label label7;
         private System.Windows.Forms.Label label8;
         private System.Windows.Forms.Label label9;
         private System.Windows.Forms.ComboBox Level;
         private System.Windows.Forms.ComboBox Race;
         private System.Windows.Forms.ComboBox CardAttribute;
         private System.Windows.Forms.TextBox ATK;
         private System.Windows.Forms.MaskedTextBox DEF;
         private System.Windows.Forms.GroupBox groupBox4;
         private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
         private System.Windows.Forms.Label label10;
         private System.Windows.Forms.Label label11;
         private System.Windows.Forms.TextBox CardName;
         private System.Windows.Forms.TextBox CardDescription;
         private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
         private System.Windows.Forms.GroupBox groupBox1;
         private System.Windows.Forms.CheckedListBox CardTypeList;
         private System.Windows.Forms.GroupBox groupBox6;
         private System.Windows.Forms.CheckedListBox CategoryList;
         private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
         private System.Windows.Forms.PictureBox CardImg;
         private System.Windows.Forms.Button LoadImageBtn;
         private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
         private System.Windows.Forms.Button OpenScriptBtn;
         private System.Windows.Forms.ComboBox SetCodeThree;
         private System.Windows.Forms.ComboBox SetCodeFour;
         private System.Windows.Forms.Label label12;
         private System.Windows.Forms.Label label13;
         private System.Windows.Forms.ComboBox LScale;
         private System.Windows.Forms.ComboBox RScale;


         private void MoveEffectUp_Click(object sender, EventArgs e)
         {
             if (EffectList.SelectedItem == null)
                 return;
             int index = EffectList.SelectedIndex;
             string value = EffectList.SelectedItem.ToString();

             if (EffectList.SelectedIndex == 0)
                 return;

             EffectList.Items.RemoveAt(index);
             EffectList.Items.Insert(index - 1, value);
             EffectList.SelectedIndex = index - 1;
             EffectInput.Clear();
         }

         private void MoveEffectDown_Click(object sender, EventArgs e)
         {
             if (EffectList.SelectedItem == null)
                 return;


             int index = EffectList.SelectedIndex;
             string value = EffectList.SelectedItem.ToString();

             if (EffectList.SelectedIndex == EffectList.Items.Count - 1)
                 return;

             EffectList.Items.RemoveAt(index);
             EffectList.Items.Insert(index + 1, value);
             EffectList.SelectedIndex = index + 1;
             EffectInput.Clear();
         }

         private void DeleteEffectbtn_Click(object sender, EventArgs e)
         {
             if (EffectList.SelectedItem == null)
                 return;

             EffectList.Items.Remove(EffectList.SelectedItem);
         }

         private void AddEffectbtn_Click(object sender, EventArgs e)
         {
             if (string.IsNullOrEmpty(EffectInput.Text))
                 return;
             if (EffectList.Items.Count == 16)
             {
                 MessageBox.Show("No more items can be added.");
                 EffectInput.Clear();
                 return;
             }

             EffectList.Items.Insert(EffectList.Items.Count, EffectInput.Text);
             EffectList.SelectedIndex = EffectList.Items.Count - 1;
             EffectInput.Clear();
         }

         private int GetCardFormat()
         {
             return (CardFormats.SelectedItem == null ? 0 : m_formats[CardFormats.SelectedIndex]);
         }

         private long GetSetCode()
         {
             MemoryStream m_stream = new MemoryStream();
             BinaryWriter m_writer = new BinaryWriter(m_stream);
             m_writer.Write((short)((SetCodeOne.SelectedIndex > 0) ? GetSetCodeFromString(SetCodeOne.SelectedItem.ToString()) : 0));
             m_writer.Write((short)((SetCodeTwo.SelectedIndex > 0) ? GetSetCodeFromString(SetCodeTwo.SelectedItem.ToString()) : 0));
             m_writer.Write((short)((SetCodeThree.SelectedIndex > 0) ? GetSetCodeFromString(SetCodeThree.SelectedItem.ToString()) : 0));
             m_writer.Write((short)((SetCodeFour.SelectedIndex > 0) ? GetSetCodeFromString(SetCodeFour.SelectedItem.ToString()) : 0));
             return BitConverter.ToInt64(m_stream.ToArray(), 0);
         }

         private int GetSetCodeFromString(string name)
         {
             foreach (var item in m_setCodes)
                 if (item.Value == name)
                     return item.Key;
             return 0;
         }

         private int GetLevelCode()
         {
             MemoryStream m_stream = new MemoryStream();
             BinaryWriter m_writer = new BinaryWriter(m_stream);
             m_writer.Write((byte)(Level.SelectedItem == null ? 0 : Int32.Parse(Level.SelectedItem.ToString().Substring(1))));
             m_writer.Write((byte)0);
             m_writer.Write((byte)Int32.Parse(RScale.SelectedItem.ToString()));
             m_writer.Write((byte)Int32.Parse(LScale.SelectedItem.ToString()));
             return BitConverter.ToInt32(m_stream.ToArray(), 0);
         }

         private int GetTypeCode()
         {
             int code = 0;
             if (CardTypeList.GetItemCheckState(0) == CheckState.Checked)
                 code += (int)CardType.Monster;
             if (CardTypeList.GetItemCheckState(1) == CheckState.Checked)
                 code += (int)CardType.Spell;
             if (CardTypeList.GetItemCheckState(2) == CheckState.Checked)
                 code += (int)CardType.Trap;
             if (CardTypeList.GetItemCheckState(3) == CheckState.Checked)
                 code += (int)CardType.Normal;
             if (CardTypeList.GetItemCheckState(4) == CheckState.Checked)
                 code += (int)CardType.Effect;
             if (CardTypeList.GetItemCheckState(5) == CheckState.Checked)
                 code += (int)CardType.Fusion;
             if (CardTypeList.GetItemCheckState(6) == CheckState.Checked)
                 code += (int)CardType.Ritual;
             if (CardTypeList.GetItemCheckState(7) == CheckState.Checked)
                 code += (int)CardType.TrapMonster;
             if (CardTypeList.GetItemCheckState(8) == CheckState.Checked)
                 code += (int)CardType.Spirit;
             if (CardTypeList.GetItemCheckState(9) == CheckState.Checked)
                 code += (int)CardType.Union;
             if (CardTypeList.GetItemCheckState(10) == CheckState.Checked)
                 code += (int)CardType.Gemini;
             if (CardTypeList.GetItemCheckState(11) == CheckState.Checked)
                 code += (int)CardType.Tuner;
             if (CardTypeList.GetItemCheckState(12) == CheckState.Checked)
                 code += (int)CardType.Synchro;
             if (CardTypeList.GetItemCheckState(13) == CheckState.Checked)
                 code += (int)CardType.Token;
             if (CardTypeList.GetItemCheckState(14) == CheckState.Checked)
                 code += (int)CardType.QuickPlay;
             if (CardTypeList.GetItemCheckState(15) == CheckState.Checked)
                 code += (int)CardType.Continuous;
             if (CardTypeList.GetItemCheckState(16) == CheckState.Checked)
                 code += (int)CardType.Equip;
             if (CardTypeList.GetItemCheckState(17) == CheckState.Checked)
                 code += (int)CardType.Field;
             if (CardTypeList.GetItemCheckState(18) == CheckState.Checked)
                 code += (int)CardType.Counter;
             if (CardTypeList.GetItemCheckState(19) == CheckState.Checked)
                 code += (int)CardType.Flip;
             if (CardTypeList.GetItemCheckState(20) == CheckState.Checked)
                 code += (int)CardType.Toon;
             if (CardTypeList.GetItemCheckState(21) == CheckState.Checked)
                 code += (int)CardType.Xyz;
             if (CardTypeList.GetItemCheckState(22) == CheckState.Checked)
                 code += (int)CardType.Pendulum;
             return code;
         }



         private bool SaveCardtoCDB(string cdbpath)
         {
             int cardid;
             int cardalias;
             int atk;
             int def;
             bool overwrite = false;


             if (!Int32.TryParse(CardID.Text, out cardid))
             {
                 MessageBox.Show("Invalid card id");
                 return false;
             }

             int updatecard = m_loadedCard == 0 ? cardid : m_loadedCard;

             if (!Int32.TryParse(Alias.Text, out cardalias))
             {
                 cardalias = 0;
             }
             if (!Int32.TryParse(ATK.Text, out atk))
             {
                 MessageBox.Show("Invalid atk value");
                 return false;
             }
             if (!Int32.TryParse(DEF.Text, out def))
             {
                 MessageBox.Show("Invalid def value");
                 return false;
             }
             string str = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
             string str2 = Path.Combine(str, cdbpath);
             if (!File.Exists(str2))
             {
                 SqliteConnection.CreateFile(cdbpath);
             }
             var connection = new SqliteConnection("Data Source=" + str2);
             connection.Open();

             //check if card id exsists

             SqliteCommand checkcommand = DatabaseHelper.CreateCommand("SELECT COUNT(*) FROM datas WHERE id= @id", connection);
             checkcommand.Parameters.Add(new SqliteParameter("@id", updatecard));
             if (DatabaseHelper.ExecuteIntCommand(checkcommand) == 1)
             {
                 if (MessageBox.Show("Overwrite current card?", "Found", MessageBoxButtons.YesNo) == DialogResult.Yes)
                 {
                     overwrite = true;
                 }
                 else
                 {
                     return false;
                 }
             }


             SqliteCommand command;
             if (overwrite)
             {
                 command = DatabaseHelper.CreateCommand("UPDATE datas" +
          " SET id= @id, ot = @ot, alias= @alias, setcode= @setcode, type= @type, atk= @atk, def= @def, level= @level, race= @race, attribute= @attribute, category= @category WHERE id = @loadedid", connection);
             }
             else
             {
                 command = DatabaseHelper.CreateCommand("INSERT INTO datas (id,ot,alias,setcode,type,atk,def,level,race,attribute,category)" +
                          " VALUES (@id, @ot, @alias, @setcode, @type, @atk, @def, @level, @race, @attribute, @category)", connection);
             }
             command.Parameters.Add(new SqliteParameter("@loadedid", updatecard));
             command.Parameters.Add(new SqliteParameter("@id", cardid));
             command.Parameters.Add(new SqliteParameter("@ot", (CardFormats.SelectedItem == null ? 0 : GetCardFormat())));
             command.Parameters.Add(new SqliteParameter("@alias", cardalias));
             command.Parameters.Add(new SqliteParameter("@setcode", GetSetCode()));
             command.Parameters.Add(new SqliteParameter("@type", GetTypeCode()));
             command.Parameters.Add(new SqliteParameter("@atk", atk));
             command.Parameters.Add(new SqliteParameter("@def", def));
             command.Parameters.Add(new SqliteParameter("@level", GetLevelCode()));
             command.Parameters.Add(new SqliteParameter("@race", (Race.SelectedItem == null ? 0 : (Race.SelectedItem == null ? 0 : m_cardRaces[Race.SelectedIndex]))));
             command.Parameters.Add(new SqliteParameter("@attribute", (CardAttribute.SelectedItem == null ? 0 : (CardAttribute.SelectedItem == null ? 0 : m_cardAttributes[CardAttribute.SelectedIndex]))));
             command.Parameters.Add(new SqliteParameter("@category", GetCategoryNumber()));
             DatabaseHelper.ExecuteNonCommand(command);
             if (overwrite)
             {
                 command = DatabaseHelper.CreateCommand("UPDATE texts" +
                     " SET id= @id,name= @name,desc= @des,str1= @str1,str2= @str2,str3= @str3,str4= @str4,str5= @str5,str6= @str6,str7= @str7,str8= @str8,str9= @str9,str10= @str10,str11= @str11,str12= @str12,str13= @str13,str14= @str14,str15= @str15,str16= @str16 WHERE id= @loadedid", connection);
             }
             else
             {
                 command = DatabaseHelper.CreateCommand("INSERT INTO texts (id,name,desc,str1,str2,str3,str4,str5,str6,str7,str8,str9,str10,str11,str12,str13,str14,str15,str16)" +
                     " VALUES (@id,@name,@des,@str1,@str2,@str3,@str4,@str5,@str6,@str7,@str8,@str9,@str10,@str11,@str12,@str13,@str14,@str15,@str16)", connection);
             }
             command.Parameters.Add(new SqliteParameter("@loadedid", updatecard));
             command.Parameters.Add(new SqliteParameter("@id", cardid));
             command.Parameters.Add(new SqliteParameter("@name", CardName.Text));
             command.Parameters.Add(new SqliteParameter("@des", CardDescription.Text));
             var parameters = new List<SqliteParameter>();
             for (int i = 0; i < 16; i++)
             {
                 parameters.Add(new SqliteParameter("@str" + (i + 1), (i < EffectList.Items.Count ? EffectList.Items[i].ToString() : string.Empty)));
             }
             command.Parameters.AddRange(parameters.ToArray());
             DatabaseHelper.ExecuteNonCommand(command);
             connection.Close();
             MessageBox.Show("Card Saved");

             if (Program.CardData.ContainsKey(cardid))
             {
                 Program.CardData[cardid] = new CardInfo(new[] { cardid.ToString(CultureInfo.InvariantCulture), (CardFormats.SelectedItem == null ? "0" : GetCardFormat().ToString(CultureInfo.InvariantCulture)),cardalias.ToString(CultureInfo.InvariantCulture),GetSetCode().ToString(CultureInfo.InvariantCulture),GetTypeCode().ToString(CultureInfo.InvariantCulture),
                    GetLevelCode().ToString(), (Race.SelectedItem == null ? "0" : (Race.SelectedItem == null ? "0" : m_cardRaces[Race.SelectedIndex].ToString(CultureInfo.InvariantCulture))),
                (CardAttribute.SelectedItem == null ? "0" : (CardAttribute.SelectedItem == null ? "0" : m_cardAttributes[CardAttribute.SelectedIndex].ToString(CultureInfo.InvariantCulture))),atk.ToString(CultureInfo.InvariantCulture),def.ToString(CultureInfo.InvariantCulture),GetCategoryNumber().ToString(CultureInfo.InvariantCulture)});

                 var cardtextarray = new List<string> { cardid.ToString(CultureInfo.InvariantCulture), CardName.Text, CardDescription.Text };

                 for (var i = 0; i < 17; i++)
                 {
                     cardtextarray.Add((i < EffectList.Items.Count ? EffectList.Items[i].ToString() : string.Empty));
                 }

                 Program.CardData[cardid].SetCardText(cardtextarray.ToArray());
             }
             else
             {
                 Program.CardData.Add(cardid, new CardInfo(new[] { cardid.ToString(CultureInfo.InvariantCulture), (CardFormats.SelectedItem == null ? "0" : GetCardFormat().ToString(CultureInfo.InvariantCulture)),cardalias.ToString(CultureInfo.InvariantCulture),GetSetCode().ToString(CultureInfo.InvariantCulture),GetTypeCode().ToString(CultureInfo.InvariantCulture),
                    GetLevelCode().ToString(), (Race.SelectedItem == null ? "0" : (Race.SelectedItem == null ? "0" : m_cardRaces[Race.SelectedIndex].ToString(CultureInfo.InvariantCulture))),
                (CardAttribute.SelectedItem == null ? "0" : (CardAttribute.SelectedItem == null ? "0" : m_cardAttributes[CardAttribute.SelectedIndex].ToString(CultureInfo.InvariantCulture))),atk.ToString(CultureInfo.InvariantCulture),def.ToString(CultureInfo.InvariantCulture),GetCategoryNumber().ToString(CultureInfo.InvariantCulture)}));

                 var cardtextarray = new List<string> { cardid.ToString(CultureInfo.InvariantCulture), CardName.Text, CardDescription.Text };

                 for (int i = 0; i < 17; i++)
                 {
                     cardtextarray.Add((i < EffectList.Items.Count ? EffectList.Items[i].ToString() : string.Empty));
                 }

                 Program.CardData[cardid].SetCardText(cardtextarray.ToArray());
             }
             return true;
         }
         public void SaveImage(string cardid)
         {
             if (m_loadedImage != "")
             {
                 // Save card image
                 ImageResizer.SaveImage(CardImg.Image,
                         "pics\\" + cardid + ".jpg", 177, 254);
                 //Save card thumbnail
                 ImageResizer.SaveImage(CardImg.Image,
                         "pics\\thumbnail\\" + cardid + ".jpg", 44, 64);
             }
         }

         private void LoadImageBtn_Click(object sender, EventArgs e)
         {
             m_loadedImage = "";
             string imagepath = ImageResizer.OpenFileWindow("Set Image ", "", "Images|*.jpg;*.jpeg;*.png;");
             if (imagepath != null)
             {
                 if (File.Exists(imagepath))
                 {
                     using (var stream = new FileStream(imagepath, FileMode.Open, FileAccess.Read))
                     {
                         CardImg.Image = Image.FromStream(stream);
                     }
                     m_loadedImage = imagepath;
                 }
                 else
                 {
                     CardImg.Image = null;
                 }
             }
         }

         private void OpenScriptBtn_Click_1(object sender, EventArgs e)
         {
             if (m_loadedCard != 0)
             {
                 string file = "script\\c" + m_loadedCard + ".lua";
                 if (File.Exists(file))
                     Process.Start(file);
                 
                 
             }
         }


         private void checkBox1_CheckedChanged(object sender, EventArgs e)
         {

             checkBox1.ThreeState = false;
             if (checkBox1.CheckState == CheckState.Unchecked || checkBox1.CheckState == CheckState.Indeterminate)
             {
                 expansionload = false;
                 checkBox1.CheckState = CheckState.Unchecked;
                 
             }

             else if (checkBox1.CheckState == CheckState.Checked)
             {


                 if (expansionload == false && currentexpansiondb != String.Empty)
                     {
                 LoadExpansionData(currentexpansiondb);
                 //SQLiteCommands.ExtractEntries(AppDomain.CurrentDomain.BaseDirectory, zip);
                 expansionload = true;
                 MessageBox.Show("Expansion cards loaded " + currentexpansiondb);
                     }
             }
             
         }

         private void DeleteBtn_Click(object sender, EventArgs e)
         {
             int cardid;
             Int32.TryParse(CardID.Text, out cardid);

             if (cardid == 0)
             {
                 MessageBox.Show("Invalid card id.", "Error", MessageBoxButtons.OK);
                 return;
             }

             if (!Program.CardData.ContainsKey(cardid))
             {
                 MessageBox.Show("Unable to find card to delete.", "Error", MessageBoxButtons.OK);
                 return;
             }

             string str = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
             string str2 = Path.Combine(str, Cdbdir);
             if (!File.Exists(str2))
             {
                 SqliteConnection.CreateFile(Cdbdir);
             }
             var connection = new SqliteConnection("Data Source=" + str2);
             connection.Open();

             SqliteCommand checkcommand = DatabaseHelper.CreateCommand("SELECT COUNT(*) FROM datas WHERE id= @id", connection);
             checkcommand.Parameters.Add(new SqliteParameter("@id", cardid));
             if (DatabaseHelper.ExecuteIntCommand(checkcommand) == 1)
             {
                 if (MessageBox.Show("Are you sure you want to delete " + Program.CardData[cardid].Name + "?", "Found", MessageBoxButtons.YesNo) == DialogResult.No)
                 {
                     return;
                 }
             }

             SqliteCommand command = DatabaseHelper.CreateCommand("DELETE FROM datas WHERE id= @id", connection);
             command.Parameters.Add(new SqliteParameter("@id", cardid));
             DatabaseHelper.ExecuteIntCommand(command);

             command = DatabaseHelper.CreateCommand("DELETE FROM texts WHERE id= @id", connection);
             command.Parameters.Add(new SqliteParameter("@id", cardid));
             DatabaseHelper.ExecuteIntCommand(command);

             Program.CardData.Remove(cardid);
             Clearbtn_Click(null, EventArgs.Empty);
         }

         private void SaveCardbtn_Click(object sender, EventArgs e)
         {
             if (SaveCardtoCDB(Cdbdir))
             {
                 SaveImage(CardID.Text);
                 m_loadedCard = Convert.ToInt32(CardID.Text);
             }
         }

         private void Clearbtn_Click(object sender, EventArgs e)
         {
             CardID.Clear();
             Alias.Text = "0";
             CardFormats.SelectedIndex = -1;
             SetCodeOne.SelectedIndex = -1;
             SetCodeTwo.SelectedIndex = -1;
             SetCodeThree.SelectedIndex = -1;
             SetCodeFour.SelectedIndex = -1;
             Level.SelectedIndex = -1;
             RScale.SelectedIndex = 0;
             LScale.SelectedIndex = 0;
             Race.SelectedIndex = -1;
             CardAttribute.SelectedIndex = -1;
             ATK.Text = "0";
             DEF.Text = "0";
             CardName.Clear();
             CardDescription.Clear();
             EffectList.Items.Clear();

             for (int i = 0; i < CardTypeList.Items.Count; i++)
             {
                 CardTypeList.SetItemCheckState(i, CheckState.Unchecked);
             }
             for (int i = 0; i < CategoryList.Items.Count; i++)
             {
                 CategoryList.SetItemCheckState(i, CheckState.Unchecked);
             }
             m_loadedCard = 0;
             CardImg.Image = null;
             m_loadedImage = "";
         }

         private void CreateScriptBtn_Click(object sender, EventArgs e)
         {
             
             string file = "script\\c" + m_loadedCard + ".lua";

             if (File.Exists(file))
                 Process.Start(file);
             else

             if (Program.CardData.ContainsKey(m_loadedCard))
             {
                 CardInfo card = Program.CardData[m_loadedCard];
                 string[] lines = { "--" + card.Name, 
                                              "function c" + m_loadedCard + ".initial_effect(c)", 
                                              string.Empty, 
                                              "end" };
                 File.WriteAllLines(file, lines);
                 Process.Start(file);
             }


                 
             switch (Effect1List.SelectedIndex){

                 case 0: //TODO Add basic scripts

                     
                     break;


                 case 1:

                     break;

             }

         }

         private void CardTypeList_SelectedIndexChanged(object sender, EventArgs e)
         {
             switch (CardTypeList.SelectedIndex){

                 case 0: //Monster

                     Effect1List.Items.Clear();
                     Effect2List.Items.Clear();
                     Effect1List.Items.AddRange(Enum.GetNames(typeof(EffectType)));
                     break;


                 case 1: //Spell

                     Effect1List.Items.Clear();
                     Effect2List.Items.Clear();
                     Effect2List.Items.AddRange(Enum.GetNames(typeof(EffectType2)));
                     break;

                 case 2: //Spell

                     Effect1List.Items.Clear();
                     Effect2List.Items.Clear();
                     Effect2List.Items.AddRange(Enum.GetNames(typeof(EffectType2)));
                     break;

             }
         }

         private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
         {
             if (comboBox1.Items != null)
                 currentexpansiondb = comboBox1.SelectedItem.ToString();

         }

         private void ExtractScriptBtn_Click(object sender, EventArgs e)
         {
             if (m_loadedCard != 0)
             {
                 string file = "script\\c" + m_loadedCard + ".lua";
                 if (!File.Exists(file))
                 {
                     string zip = "expansions\\" + comboBox1.SelectedItem.ToString().Split('.')[0] + ".zip";
                     string card = "script/c" + m_loadedCard + ".lua";
                     string image = "pics/" + m_loadedCard + ".jpg";
                     SQLiteCommands.Extract(AppDomain.CurrentDomain.BaseDirectory,zip, card);
                     SQLiteCommands.Extract(AppDomain.CurrentDomain.BaseDirectory, zip, image);
                 }

             }
         }











         
       }  
    
   }

