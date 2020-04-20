namespace CardManager
{
    using CardEditor;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml;

    public partial class LuaEdit : Form
    {
        private string argExCDB;
        private Button button1;
        public static Dictionary<int, CardInfo> CardDataeff = new Dictionary<int, CardInfo>();
        private Dictionary<object, string> cdbDic;
        private string[] cdbs;
        private ColorText colorText;
        private IContainer components;
        private string configPath;
        private ContextMenuStrip contextMenuStrip1;
        private ContextMenuStrip contextMenuStrip2;
        private int count;
        private ToolStripMenuItem effectWizardToolStripMenuItem;
        private EffectWizzard ew;
        private List<string> fileList;
        private string filePath;
        private FileSystemWatcher fileSystemWatcher1;
        private FolderBrowserDialog folderBrowserDialog1;
        private List<string> funcLine;
        private List<string> funcList;
        private string gamePath;
        private ListBox listBox1;
        private ListView listBox2;
        private MenuStrip menuStrip1;
        private OpenFileDialog openFileDialog1;
        private OpenFileDialog openFileDialog2;
        private TreeNode originCmdRoot;
        private TreeNode originConstRoot;
        private List<int> resultIndexics;
        private RichTextBox richTextBox1;
        private SaveFileDialog saveFileDialog1;
        private string scriptFolderPath;
        private object Searchlock;
        private SearchMgr smgrCmd;
        private SearchMgr smgrConst;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private StatusStrip statusStrip1;
        private TabControl tabControl1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TextBox textBox1;
        private TextBox textBox2;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStrip toolStrip3;
        private ToolStrip toolStrip4;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton10;
        private ToolStripButton toolStripButton11;
        private ToolStripButton toolStripButton12;
        private ToolStripButton toolStripButton15;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private ToolStripButton toolStripButton5;
        private ToolStripButton toolStripButton6;
        private ToolStripButton toolStripButton7;
        private ToolStripButton toolStripButton8;
        private ToolStripButton toolStripButton9;
        private ToolStripContainer toolStripContainer1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripLabel toolStripLabel3;
        private ToolStripMenuItem toolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripTextBox toolStripTextBox2;
        private ToolStripTextBox toolStripTextBox3;
        private ToolStripTextBox toolStripTextBox4;
        private ToolTip toolTip1;
        private TreeView treeView1;
        private TreeView treeView2;
        private string xmlCmdPath;
        private string xmlConstPath;
        private string xmlKeyWPath;
        private ToolStripMenuItem 保存脚本ToolStripMenuItem;
        private ToolStripMenuItem 全选ToolStripMenuItem;
        private ToolStripMenuItem 另存为ToolStripMenuItem;
        private ToolStripMenuItem 复制ToolStripMenuItem;
        private ToolStripMenuItem 定位到函数ToolStripMenuItem;
        private ToolStripMenuItem 打开脚本ToolStripMenuItem;
        private ToolStripMenuItem 插入ToolStripMenuItem;
        private ToolStripMenuItem 文件ToolStripMenuItem;
        private ToolStripMenuItem 新建效果ToolStripMenuItem;
        private ToolStripMenuItem 新建脚本ToolStripMenuItem;
        private ToolStripMenuItem 查找ToolStripMenuItem;
        private ToolStripMenuItem 粘贴ToolStripMenuItem;
        private ToolStripMenuItem 编辑ToolStripMenuItem;
        private ToolStripMenuItem 脚本文件夹ToolStripMenuItem;
        private ToolStripMenuItem 自动换行ToolStripMenuItem;
        private ToolStripMenuItem 视图ToolStripMenuItem;
        private ToolStripMenuItem 设置ToolStripMenuItem;
        private ToolStripMenuItem 设置游戏路径ToolStripMenuItem;
        private ToolStripMenuItem 跳转ToolStripMenuItem;
        private ToolStripMenuItem 过滤函数ToolStripMenuItem;
        private ToolStripMenuItem 隐藏左侧ToolStripMenuItem;

        public LuaEdit()
        {
            this.ew = new EffectWizzard("");
            this.xmlCmdPath = Application.StartupPath + @"\data\Command.xml";
            this.xmlConstPath = Application.StartupPath + @"\data\Constant.xml";
            this.xmlKeyWPath = Application.StartupPath + @"\data\Format.xml";
            this.scriptFolderPath = "";           
            this.resultIndexics = new List<int>();
            this.cdbDic = new Dictionary<object, string>();
            this.cdbs = Directory.GetFiles(Application.StartupPath, "*.cdb");
            this.argExCDB = "";
            this.fileList = new List<string>();
            this.funcList = new List<string>();
            this.funcLine = new List<string>();
            this.Searchlock = new object();
            this.InitializeComponent();
            base.TopLevel = false;
            this.Dock = DockStyle.Fill;
            base.Visible = true;
            this.colorText = new ColorText(this.richTextBox1);
            this.colorText.loadKeywordXml(this.xmlKeyWPath);
            this.loadConfig();
            this.textBox2.Text = "Search";
            this.textBox2.TextAlign = HorizontalAlignment.Center;
            this.textBox2.ForeColor = SystemColors.WindowFrame;
            this.listBox2.View = View.List;
        }

        

        private TreeNode builtTree(XmlElement xe)
        {
            MyNode node = new MyNode {
                Description = xe.GetAttribute("des"),
                InsertInfo = xe.GetAttribute("insertInfo"),
                Tips = xe.GetAttribute("tips")
            };
            if (xe.ChildNodes != null)
            {
                foreach (XmlElement element in xe.ChildNodes)
                {
                    node.Nodes.Add(this.builtTree(element));
                }
            }
            return node;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lock (this.Searchlock)
            {
                if (this.textBox2.Text == "")
                {
                    this.listBox2.Items.Clear();
                }
                else if (this.textBox2.Text != "Search")
                {
                    this.listBox2.Items.Clear();
                    foreach (int num in CardDataeff.Keys)
                    {
                        if (CardDataeff[num].Id.ToString().ToLower().StartsWith(this.textBox2.Text.ToLower()) || CardDataeff[num].Name.ToLower().Contains(this.textBox2.Text.ToLower()))
                        {
                            ListViewItem item = new ListViewItem {
                                Text = CardDataeff[num].Name,
                                Tag = CardDataeff[num].Id
                            };
                            this.listBox2.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void cdbItemClick(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.cdbDic.Keys)
            {
                if (item != sender)
                {
                    item.Checked = false;
                }
            }
            this.argExCDB = "";
            ToolStripMenuItem item2 = (ToolStripMenuItem) sender;
            if (item2.Checked)
            {
                this.argExCDB = "-e" + this.cdbDic[item2];
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

        private void effectWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ew.setCID(this.getFileName());
            if (this.ew.ShowDialog(this) == DialogResult.OK)
            {
                Clipboard.SetData(DataFormats.StringFormat, this.ew.getScript());
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.richTextBox1.Text))
            {
                switch (MessageBox.Show(" whether to save the current text?", "Prompt", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;

                    case DialogResult.Yes:
                        this.保存脚本ToolStripMenuItem_Click(sender, e);
                        break;
                }
            }
            
        }



        private string getFileName()
        {
            if (string.IsNullOrEmpty(this.filePath))
            {
                return "";
            }
            int num = this.filePath.LastIndexOf('\\');
            int num2 = this.filePath.LastIndexOf('.');
            return this.filePath.Substring(num + 1, (num2 - num) - 1);
        }

        private void gotoFunction()
        {
            this.selectLine(int.Parse(this.funcLine[this.listBox1.SelectedIndex]) + 1);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode(" Node 1 ");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode(" Node 19");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode(" Node 20");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode(" Node 21");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode(" Node 2", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode(" Node 3");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode(" Node 4");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode(" Node 5");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode(" Node 6");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode(" Node 7");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Library", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode(" Node 9");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode(" Node 10");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode(" Node 11");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode(" Node 12");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode(" Node 13");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode(" Node 15");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode(" Node 16");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode(" Node 17");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode(" Node 18");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode(" Node 14", new System.Windows.Forms.TreeNode[] {
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20});
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode(" Node 8", new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode21});
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存为ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.查找ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏左侧ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.自动换行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.脚本文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置游戏路径ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.effectWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.跳转ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.定位到函数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBox4 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listBox2 = new System.Windows.Forms.ListView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox3 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton15 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.插入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建效果ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.过滤函数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.粘贴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.编辑ToolStripMenuItem,
            this.视图ToolStripMenuItem,
            this.设置ToolStripMenuItem,
            this.effectWizardToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(880, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建脚本ToolStripMenuItem,
            this.打开脚本ToolStripMenuItem,
            this.保存脚本ToolStripMenuItem,
            this.另存为ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.文件ToolStripMenuItem.Text = "File";
            // 
            // 新建脚本ToolStripMenuItem
            // 
            this.新建脚本ToolStripMenuItem.Name = "新建脚本ToolStripMenuItem";
            this.新建脚本ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.新建脚本ToolStripMenuItem.Text = "New";
            this.新建脚本ToolStripMenuItem.Click += new System.EventHandler(this.新建脚本ToolStripMenuItem_Click);
            // 
            // 打开脚本ToolStripMenuItem
            // 
            this.打开脚本ToolStripMenuItem.Name = "打开脚本ToolStripMenuItem";
            this.打开脚本ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.打开脚本ToolStripMenuItem.Text = "Open";
            this.打开脚本ToolStripMenuItem.Click += new System.EventHandler(this.打开脚本ToolStripMenuItem_Click);
            // 
            // 保存脚本ToolStripMenuItem
            // 
            this.保存脚本ToolStripMenuItem.Name = "保存脚本ToolStripMenuItem";
            this.保存脚本ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.保存脚本ToolStripMenuItem.Text = "Save";
            this.保存脚本ToolStripMenuItem.Click += new System.EventHandler(this.保存脚本ToolStripMenuItem_Click);
            // 
            // 另存为ToolStripMenuItem
            // 
            this.另存为ToolStripMenuItem.Name = "另存为ToolStripMenuItem";
            this.另存为ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.另存为ToolStripMenuItem.Text = "Save As";
            this.另存为ToolStripMenuItem.Click += new System.EventHandler(this.另存为ToolStripMenuItem_Click);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripSeparator3,
            this.查找ToolStripMenuItem});
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.编辑ToolStripMenuItem.Text = "Edit";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(143, 22);
            this.toolStripMenuItem1.Text = "Undo";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(143, 22);
            this.toolStripMenuItem2.Text = "Redo";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(140, 6);
            // 
            // 查找ToolStripMenuItem
            // 
            this.查找ToolStripMenuItem.Name = "查找ToolStripMenuItem";
            this.查找ToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.查找ToolStripMenuItem.Text = "Find/Replace";
            this.查找ToolStripMenuItem.Click += new System.EventHandler(this.查找ToolStripMenuItem_Click);
            // 
            // 视图ToolStripMenuItem
            // 
            this.视图ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.隐藏左侧ToolStripMenuItem,
            this.自动换行ToolStripMenuItem});
            this.视图ToolStripMenuItem.Name = "视图ToolStripMenuItem";
            this.视图ToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.视图ToolStripMenuItem.Text = "View";
            // 
            // 隐藏左侧ToolStripMenuItem
            // 
            this.隐藏左侧ToolStripMenuItem.CheckOnClick = true;
            this.隐藏左侧ToolStripMenuItem.Name = "隐藏左侧ToolStripMenuItem";
            this.隐藏左侧ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.隐藏左侧ToolStripMenuItem.Text = "Hide the left side";
            this.隐藏左侧ToolStripMenuItem.Click += new System.EventHandler(this.隐藏左侧ToolStripMenuItem_Click);
            // 
            // 自动换行ToolStripMenuItem
            // 
            this.自动换行ToolStripMenuItem.CheckOnClick = true;
            this.自动换行ToolStripMenuItem.Name = "自动换行ToolStripMenuItem";
            this.自动换行ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.自动换行ToolStripMenuItem.Text = "Wrap";
            this.自动换行ToolStripMenuItem.ToolTipText = " wrap affect the positioning line , when debug try not to use";
            this.自动换行ToolStripMenuItem.Click += new System.EventHandler(this.自动换行ToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.脚本文件夹ToolStripMenuItem,
            this.设置游戏路径ToolStripMenuItem,
            this.toolStripSeparator9,
            this.toolStripMenuItem,
            this.toolStripSeparator10});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.设置ToolStripMenuItem.Text = "Setting";
            // 
            // 脚本文件夹ToolStripMenuItem
            // 
            this.脚本文件夹ToolStripMenuItem.Name = "脚本文件夹ToolStripMenuItem";
            this.脚本文件夹ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.脚本文件夹ToolStripMenuItem.Text = "Set Folder Path";
            this.脚本文件夹ToolStripMenuItem.Visible = false;
            this.脚本文件夹ToolStripMenuItem.Click += new System.EventHandler(this.脚本文件夹ToolStripMenuItem_Click);
            // 
            // 设置游戏路径ToolStripMenuItem
            // 
            this.设置游戏路径ToolStripMenuItem.Name = "设置游戏路径ToolStripMenuItem";
            this.设置游戏路径ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.设置游戏路径ToolStripMenuItem.Text = "Set Ygopro Path";
            this.设置游戏路径ToolStripMenuItem.Click += new System.EventHandler(this.设置游戏路径ToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(156, 6);
            // 
            // toolStripMenuItem
            // 
            this.toolStripMenuItem.Enabled = false;
            this.toolStripMenuItem.Name = "toolStripMenuItem";
            this.toolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.toolStripMenuItem.Text = "File";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(156, 6);
            // 
            // effectWizardToolStripMenuItem
            // 
            this.effectWizardToolStripMenuItem.Name = "effectWizardToolStripMenuItem";
            this.effectWizardToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.effectWizardToolStripMenuItem.Text = "Effect Wizard";
            this.effectWizardToolStripMenuItem.Click += new System.EventHandler(this.effectWizardToolStripMenuItem_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(880, 449);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(880, 495);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(880, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(94, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Commissioning:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.IsLink = true;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(27, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "null";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripStatusLabel2.ToolTipText = " click to jump.";
            this.toolStripStatusLabel2.Click += new System.EventHandler(this.toolStripStatusLabel2_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip3);
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(880, 449);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(264, 447);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listBox1);
            this.tabPage2.Controls.Add(this.toolStrip1);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(256, 421);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = " Function List ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip2;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 28);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(250, 390);
            this.listBox1.TabIndex = 3;
            this.listBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseClick);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.跳转ToolStripMenuItem,
            this.定位到函数ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(203, 48);
            // 
            // 跳转ToolStripMenuItem
            // 
            this.跳转ToolStripMenuItem.Name = "跳转ToolStripMenuItem";
            this.跳转ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.跳转ToolStripMenuItem.Text = " insert function";
            this.跳转ToolStripMenuItem.Click += new System.EventHandler(this.跳转ToolStripMenuItem_Click);
            // 
            // 定位到函数ToolStripMenuItem
            // 
            this.定位到函数ToolStripMenuItem.Name = "定位到函数ToolStripMenuItem";
            this.定位到函数ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.定位到函数ToolStripMenuItem.Text = " positioning to function.";
            this.定位到函数ToolStripMenuItem.Click += new System.EventHandler(this.定位到函数ToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Gray;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.toolStripButton1,
            this.toolStripButton9,
            this.toolStripButton10});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(250, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStrip1_KeyDown);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(150, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(46, 22);
            this.toolStripButton1.Text = "Search";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton9.Text = "▼";
            this.toolStripButton9.Click += new System.EventHandler(this.toolStripButton9_Click);
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton10.Text = "▲";
            this.toolStripButton10.Click += new System.EventHandler(this.toolStripButton10_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.Controls.Add(this.toolStrip2);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(256, 421);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "library ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.textBox1);
            this.splitContainer2.Size = new System.Drawing.Size(256, 396);
            this.splitContainer2.SplitterDistance = 321;
            this.splitContainer2.TabIndex = 3;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = " Node 1 ";
            treeNode1.Text = " Node 1 ";
            treeNode2.Name = " Node 19                                         ";
            treeNode2.Text = " Node 19";
            treeNode3.Name = " Node 20";
            treeNode3.Text = " Node 20";
            treeNode4.Name = " Node 21";
            treeNode4.Text = " Node 21";
            treeNode5.Name = " Node 2";
            treeNode5.Text = " Node 2";
            treeNode6.Name = " Node 3";
            treeNode6.Text = " Node 3";
            treeNode7.Name = " Node 4";
            treeNode7.Text = " Node 4";
            treeNode8.Name = " Node 5";
            treeNode8.Text = " Node 5";
            treeNode9.Name = " Node 6";
            treeNode9.Text = " Node 6";
            treeNode10.Name = " Node 7";
            treeNode10.Text = " Node 7";
            treeNode11.Name = "Library";
            treeNode11.Text = "Library";
            treeNode12.Name = " Node 9";
            treeNode12.Text = " Node 9";
            treeNode13.Name = " Node 10";
            treeNode13.Text = " Node 10";
            treeNode14.Name = " Node 11";
            treeNode14.Text = " Node 11";
            treeNode15.Name = " Node 12";
            treeNode15.Text = " Node 12";
            treeNode16.Name = " Node 13";
            treeNode16.Text = " Node 13";
            treeNode17.Name = " Node 15";
            treeNode17.Text = " Node 15";
            treeNode18.Name = " Node 16";
            treeNode18.Text = " Node 16";
            treeNode19.Name = " Node 17";
            treeNode19.Text = " Node 17";
            treeNode20.Name = " Node 18";
            treeNode20.Text = " Node 18";
            treeNode21.Name = " Node 14";
            treeNode21.Text = " Node 14";
            treeNode22.Name = " Node 8";
            treeNode22.Text = " Node 8";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode22});
            this.treeView1.Size = new System.Drawing.Size(256, 321);
            this.treeView1.TabIndex = 2;
            this.treeView1.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.treeView1_NodeMouseHover);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDoubleClick);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(256, 71);
            this.textBox1.TabIndex = 0;
            // 
            // toolStrip2
            // 
            this.toolStrip2.AutoSize = false;
            this.toolStrip2.BackColor = System.Drawing.Color.Gray;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox2,
            this.toolStripButton2,
            this.toolStripSeparator4,
            this.toolStripButton5,
            this.toolStripButton6});
            this.toolStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(256, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(150, 25);
            this.toolStripTextBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox2_KeyDown);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(49, 22);
            this.toolStripButton2.Text = "Search ";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "▼";
            this.toolStripButton5.ToolTipText = "下一个";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Text = "▲";
            this.toolStripButton6.ToolTipText = "Next";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.treeView2);
            this.tabPage3.Controls.Add(this.toolStrip4);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(256, 421);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "constant";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // treeView2
            // 
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView2.Location = new System.Drawing.Point(3, 28);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(250, 390);
            this.treeView2.TabIndex = 4;
            this.treeView2.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.treeView2_NodeMouseHover);
            this.treeView2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView2_MouseDoubleClick);
            // 
            // toolStrip4
            // 
            this.toolStrip4.BackColor = System.Drawing.Color.Gray;
            this.toolStrip4.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox4,
            this.toolStripButton4,
            this.toolStripSeparator5,
            this.toolStripButton7,
            this.toolStripButton8});
            this.toolStrip4.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip4.Location = new System.Drawing.Point(3, 3);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(250, 25);
            this.toolStrip4.TabIndex = 3;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // toolStripTextBox4
            // 
            this.toolStripTextBox4.Name = "toolStripTextBox4";
            this.toolStripTextBox4.Size = new System.Drawing.Size(150, 25);
            this.toolStripTextBox4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox4_KeyDown);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(46, 22);
            this.toolStripButton4.Text = "Search";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton7.Text = "▼";
            this.toolStripButton7.ToolTipText = "Next";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(23, 19);
            this.toolStripButton8.Text = "▲";
            this.toolStripButton8.ToolTipText = "Previous";
            this.toolStripButton8.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tableLayoutPanel1);
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(256, 421);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Card Scripts";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.listBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.83848F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.16152F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(256, 421);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // listBox2
            // 
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.Location = new System.Drawing.Point(3, 36);
            this.listBox2.MultiSelect = false;
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(250, 382);
            this.listBox2.TabIndex = 3;
            this.listBox2.UseCompatibleStateImageBehavior = false;
            this.listBox2.View = System.Windows.Forms.View.List;
            this.listBox2.DoubleClick += new System.EventHandler(this.listBox2_DoubleClick);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.8F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.2F));
            this.tableLayoutPanel2.Controls.Add(this.textBox2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.button1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(250, 27);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(3, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(161, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.Enter += new System.EventHandler(this.textBox2_Enter);
            this.textBox2.Leave += new System.EventHandler(this.textBox2_Leave);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(170, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 21);
            this.button1.TabIndex = 4;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolStrip3
            // 
            this.toolStrip3.AutoSize = false;
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripLabel2,
            this.toolStripLabel3,
            this.toolStripSeparator1,
            this.toolStripTextBox3,
            this.toolStripButton3,
            this.toolStripSeparator7,
            this.toolStripButton11,
            this.toolStripButton15,
            this.toolStripButton12});
            this.toolStrip3.Location = new System.Drawing.Point(0, 422);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(608, 25);
            this.toolStrip3.TabIndex = 1;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(115, 22);
            this.toolStripLabel1.Text = " current line number";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(12, 22);
            this.toolStripLabel2.Text = "/";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(93, 22);
            this.toolStripLabel3.Text = " number of lines";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox3
            // 
            this.toolStripTextBox3.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.toolStripTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolStripTextBox3.Name = "toolStripTextBox3";
            this.toolStripTextBox3.Size = new System.Drawing.Size(50, 25);
            this.toolStripTextBox3.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolStripTextBox3.Click += new System.EventHandler(this.toolStripTextBox3_Click);
            this.toolStripTextBox3.TextChanged += new System.EventHandler(this.toolStripTextBox3_TextChanged);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(43, 22);
            this.toolStripButton3.Text = "Jump ";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton11.BackColor = System.Drawing.SystemColors.ControlDark;
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(135, 22);
            this.toolStripButton11.Text = " Save and run the game";
            this.toolStripButton11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripButton11.Click += new System.EventHandler(this.toolStripButton11_Click);
            // 
            // toolStripButton15
            // 
            this.toolStripButton15.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton15.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton15.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton15.Name = "toolStripButton15";
            this.toolStripButton15.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton15.Text = "Save";
            this.toolStripButton15.Click += new System.EventHandler(this.toolStripButton15_Click);
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton12.ToolTipText = "If the same words in the root directory.";
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.Color.Silver;
            this.richTextBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBox1.EnableAutoDragDrop = true;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.HideSelection = false;
            this.richTextBox1.Location = new System.Drawing.Point(0, -1);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(609, 426);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.TabStop = false;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseClick);
            this.richTextBox1.ForeColorChanged += new System.EventHandler(this.richTextBox1_ForeColorChanged);
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            this.richTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
            this.richTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox1_KeyPress);
            this.richTextBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyUp);
            this.richTextBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDown);
            this.richTextBox1.MouseHover += new System.EventHandler(this.richTextBox1_MouseHover);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.插入ToolStripMenuItem,
            this.toolStripSeparator8,
            this.toolStripMenuItem3,
            this.复制ToolStripMenuItem,
            this.粘贴ToolStripMenuItem,
            this.toolStripSeparator6,
            this.toolStripMenuItem4,
            this.全选ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(143, 170);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(142, 22);
            this.toolStripMenuItem5.Text = "Current Card";
            this.toolStripMenuItem5.ToolTipText = "c + Card ID";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // 插入ToolStripMenuItem
            // 
            this.插入ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建效果ToolStripMenuItem,
            this.过滤函数ToolStripMenuItem});
            this.插入ToolStripMenuItem.Name = "插入ToolStripMenuItem";
            this.插入ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.插入ToolStripMenuItem.Text = "Insert";
            // 
            // 新建效果ToolStripMenuItem
            // 
            this.新建效果ToolStripMenuItem.Name = "新建效果ToolStripMenuItem";
            this.新建效果ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.新建效果ToolStripMenuItem.Text = "New Effect";
            this.新建效果ToolStripMenuItem.Click += new System.EventHandler(this.新建效果ToolStripMenuItem_Click);
            // 
            // 过滤函数ToolStripMenuItem
            // 
            this.过滤函数ToolStripMenuItem.Name = "过滤函数ToolStripMenuItem";
            this.过滤函数ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.过滤函数ToolStripMenuItem.Text = "Filter Function";
            this.过滤函数ToolStripMenuItem.Visible = false;
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(139, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(142, 22);
            this.toolStripMenuItem3.Text = "Cut";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.复制ToolStripMenuItem.Text = "Copy";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // 粘贴ToolStripMenuItem
            // 
            this.粘贴ToolStripMenuItem.Name = "粘贴ToolStripMenuItem";
            this.粘贴ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.粘贴ToolStripMenuItem.Text = "Paste";
            this.粘贴ToolStripMenuItem.Click += new System.EventHandler(this.粘贴ToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(139, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(142, 22);
            this.toolStripMenuItem4.Text = "Delete";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // 全选ToolStripMenuItem
            // 
            this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
            this.全选ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.全选ToolStripMenuItem.Text = "Select";
            this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItem_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "lua|*.lua";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "*.lua";
            this.saveFileDialog1.Filter = "lua|*.lua";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            this.openFileDialog2.Filter = "exe|*.exe";
            // 
            // LuaEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(880, 495);
            this.Controls.Add(this.toolStripContainer1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "LuaEdit";
            this.Text = "EffectEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.LuaEdit_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        private void InitialView()
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(this.xmlCmdPath);
                TreeNode node = this.builtTree(document.DocumentElement);
                node.Expand();
                this.treeView1.Nodes.Clear();
                this.treeView1.Nodes.Add(node);
                this.originCmdRoot = node;
            }
            catch (Exception exception)
            {
                this.treeView1.Nodes.Clear();
                MessageBox.Show(exception.Message, "Warning", MessageBoxButtons.OK);
            }
            try
            {
                document.Load(this.xmlConstPath);
                TreeNode node2 = this.builtTree(document.DocumentElement);
                node2.Expand();
                this.treeView2.Nodes.Clear();
                this.treeView2.Nodes.Add(node2);
                this.originConstRoot = node2;
            }
            catch (Exception)
            {
                this.treeView2.Nodes.Clear();
                MessageBox.Show(@"\data\Constant.xml 不存在", "Warning ", MessageBoxButtons.OK);
            }
            if (!string.IsNullOrEmpty(this.filePath))
            {
                this.openFile(this.filePath);
            }
            else
            {
                this.newLua();
            }
            foreach (string str in this.cdbs)
            {
                int num = str.LastIndexOf('\\');
                string text = str.Substring(num + 1);
                if (!(text == "cards.cdb"))
                {
                    ToolStripMenuItem key = new ToolStripMenuItem(text) {
                        CheckOnClick = true
                    };
                    key.Click += new EventHandler(this.cdbItemClick);
                    this.cdbDic.Add(key, text);
                    this.设置ToolStripMenuItem.DropDownItems.Add(key);
                }
            }
            this.updateFuncList();
        }

        private void insertFunction()
        {
            string str = this.funcList[this.listBox1.SelectedIndex];
            int index = str.IndexOf('(');
            int num2 = str.IndexOf(')');
            this.richTextBox1.SelectedText = str.Remove(index, (num2 - index) + 1);
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.gotoFunction();
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedItems.Count != 0)
            {
                int cardid = Convert.ToInt32(this.listBox2.SelectedItems[0].Tag);
                this.LoadCard(cardid);
            }
        }

        private bool LoadCard(int cardid)
        {
            if (!CardDataeff.ContainsKey(cardid))
            {
                return false;
            }
            CardInfo local1 = CardDataeff[cardid];
            if (!string.IsNullOrEmpty(this.richTextBox1.Text) && (MessageBox.Show("do you want to save? ", "Prompt", MessageBoxButtons.YesNoCancel) == DialogResult.Yes))
            {
                if (string.IsNullOrEmpty(this.filePath))
                {
                    this.saveFileDialog1.InitialDirectory = this.scriptFolderPath;
                    this.saveFileDialog1.FileName = this.getFileName();
                    if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
                    {
                        this.saveFile(this.saveFileDialog1.FileName);
                    }
                }
                else
                {
                    this.saveFile(this.filePath);
                }
            }
            this.openFile(string.Concat(new object[] { cdbdir, @"\script\c", cardid, ".lua" }));
            return true;
        }

        private void loadConfig()
        {
            this.filePath = AppDomain.CurrentDomain.BaseDirectory;
            this.gamePath = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void loadScriptFile(string path)
        {
            try
            {
                this.listBox1.Items.Clear();
                string[] files = Directory.GetFiles(path);
                this.fileList = new List<string>((IEnumerable<string>) files);
                foreach (string str in files)
                {
                    int startIndex = str.LastIndexOf(@"\") + 1;
                    this.listBox1.Items.Add(str.Substring(startIndex));
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void newLua()
        {
            this.richTextBox1.Text = "";
            this.filePath = "";
            this.Text = "EffectEditor--New Script.lua";
        }

        private void openFile(string path)
        {
            try
            {
                StreamReader reader = new StreamReader(path);
                StringBuilder builder = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    builder.Append(reader.ReadLine());
                    builder.AppendLine();
                }
                this.richTextBox1.Text = builder.ToString();
                reader.Close();
                this.filePath = path;
                this.Text = "EffectEditor--" + this.filePath;
                this.colorText.colorAll();
                this.updateLineState();
            }
            catch (Exception)
            {
                MessageBox.Show("cannot open " + path, " open failed! ", MessageBoxButtons.OK);
                this.filePath = "";
            }
        }

        private void richTextBox1_ForeColorChanged(object sender, EventArgs e)
        {
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar != '\x0001') && (e.KeyChar != '\x0003')) && ((e.KeyChar != '\x0016') && (e.KeyChar != '\x001a')))
            {
                char keyChar = e.KeyChar;
            }
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.toolStripLabel1.Text = (this.richTextBox1.GetLineFromCharIndex(this.richTextBox1.SelectionStart) + 1).ToString();
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void richTextBox1_MouseHover(object sender, EventArgs e)
        {
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.updateLineState();
            this.updateFuncList();
            if (this.colorText != null)
            {
                this.colorText.stepColor();
            }
        }

        

        private void saveFile(string path)
        {
            try
            {
                StreamWriter writer = new StreamWriter(path);
                writer.Write(this.richTextBox1.Text);
                writer.Close();
                this.filePath = path;
                this.Text = "EffectEditor--" + this.filePath;
            }
            catch (Exception)
            {
                MessageBox.Show(" save failed! ");
            }
        }

        private void searchFile(string fileName)
        {
            this.resultIndexics.Clear();
            foreach (string str in this.listBox1.Items)
            {
                if (str.Contains(fileName))
                {
                    this.resultIndexics.Add(this.listBox1.Items.IndexOf(str));
                }
            }
        }

        private void selectLine(int value)
        {
            this.richTextBox1.Select(this.richTextBox1.GetFirstCharIndexFromLine(value - 1), this.richTextBox1.Lines[value - 1].Length);
            this.richTextBox1.Focus();
            this.toolStripLabel1.Text = value.ToString();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (this.textBox2.Text == "Search")
            {
                this.textBox2.Text = "";
                this.textBox2.ForeColor = SystemColors.WindowText;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (this.textBox2.Text == "")
            {
                this.textBox2.Text = "Search";
                this.textBox2.ForeColor = SystemColors.WindowFrame;
            }
        }

        private void toolStrip1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.searchFile(this.toolStripTextBox1.Text);
            this.listBox1.ClearSelected();
            if (this.resultIndexics.Count != 0)
            {
                this.listBox1.SetSelected(this.resultIndexics[0], true);
            }
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            if (this.resultIndexics.Count != 0)
            {
                int num = this.resultIndexics.IndexOf(this.listBox1.SelectedIndex) - 1;
                if (num < 0)
                {
                    num = 0;
                }
                this.listBox1.SetSelected(this.resultIndexics[num], true);
            }
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            this.保存脚本ToolStripMenuItem_Click(sender, e);
            if (!string.IsNullOrEmpty(this.gamePath))
            {
                Process process = new Process();
                try
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = this.gamePath;
                    process.StartInfo.WorkingDirectory = this.gamePath.Substring(0, this.gamePath.LastIndexOf('\\'));
                    process.StartInfo.CreateNoWindow = false;
                    process.StartInfo.Arguments = "-debug " + this.argExCDB;
                    process.Start();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
            else
            {
                MessageBox.Show(" first set ygopro game path! ");
            }
        }


        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = Application.StartupPath + @"\PuzzleMaker.exe";
                process.StartInfo.WorkingDirectory = Application.StartupPath;
                process.StartInfo.CreateNoWindow = false;
                process.Start();
            }
            catch (Exception)
            {
                MessageBox.Show(" Unable to open PuzzleMaker, please check whether the root directory of the program , or program name is not right");
            }
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            this.保存脚本ToolStripMenuItem_Click(sender, e);
            Process process = new Process();
            try
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = Application.StartupPath + @"\lua.exe";
                process.StartInfo.WorkingDirectory = Application.StartupPath;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Arguments = this.filePath + " run";
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                string str = process.StandardError.ReadToEnd().Replace(process.StartInfo.FileName + ":", "").Replace(this.filePath + ":", "").Replace("\r\n", "");
                if (str.Contains(this.getFileName()))
                {
                    str = "";
                }
                if (str != "")
                {
                    this.statusStrip1.BackColor = Color.Crimson;
                }
                else
                {
                    str = "compile";
                    this.statusStrip1.BackColor = Color.Teal;
                }
                this.toolStripStatusLabel2.Text = str;
            }
            catch (Exception)
            {
                MessageBox.Show(" Unable to open PuzzleMaker, please check whether the root directory of the program , or program name is not right");
            }
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            this.保存脚本ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.treeView1.SelectedNode = this.smgrCmd.search(this.toolStripTextBox2.Text);
            this.treeView1.Select();
            this.updateTipsBox((MyNode) this.treeView1.SelectedNode);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                this.selectLine(int.Parse(this.toolStripTextBox3.Text));
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.treeView2.SelectedNode = this.smgrConst.search(this.toolStripTextBox4.Text);
            this.treeView2.Select();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.treeView1.SelectedNode = this.smgrCmd.next();
            this.treeView1.Select();
            this.updateTipsBox((MyNode) this.treeView1.SelectedNode);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            this.treeView1.SelectedNode = this.smgrCmd.previous();
            this.treeView1.Select();
            this.updateTipsBox((MyNode) this.treeView1.SelectedNode);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            this.treeView2.SelectedNode = this.smgrConst.next();
            this.treeView2.Select();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            this.treeView2.SelectedNode = this.smgrConst.previous();
            this.treeView2.Select();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (this.resultIndexics.Count != 0)
            {
                int num = this.resultIndexics.IndexOf(this.listBox1.SelectedIndex) + 1;
                if (num >= this.resultIndexics.Count)
                {
                    num = this.resultIndexics.Count - 1;
                }
                this.listBox1.SetSelected(this.resultIndexics[num], true);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Undo();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Redo();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Rtf, this.richTextBox1.SelectedRtf);
            this.richTextBox1.SelectedRtf = "";
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectedText = "";
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectedText = this.getFileName();
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            try
            {
                string text = this.toolStripStatusLabel2.Text;
                int num = int.Parse(text.Split(new char[] { ':' })[0]) - 1;
                if (text.Contains("unexpected"))
                {
                    num++;
                }
                this.selectLine(num);
            }
            catch (Exception)
            {
            }
        }

        private void toolStripTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.toolStripButton2_Click(sender, e);
            }
        }

        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {
        }

        private void toolStripTextBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int length = int.Parse(this.toolStripTextBox3.Text);
                if (length > this.richTextBox1.Lines.Length)
                {
                    length = this.richTextBox1.Lines.Length;
                }
                else if (length <= 0)
                {
                    length = 1;
                }
                this.toolStripTextBox3.Text = length.ToString();
            }
            catch (Exception)
            {
                this.toolStripTextBox3.Text = "";
            }
        }

        private void toolStripTextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.toolStripButton4_Click(sender, e);
            }
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.treeView1.SelectedNode.IsExpanded)
            {
                MyNode selectedNode = (MyNode) this.treeView1.SelectedNode;
                RichTextBox box = this.richTextBox1;
                string str = box.SelectedText + selectedNode.InsertInfo + " ";
                box.SelectedText = str;
                int selectionStart = this.richTextBox1.SelectionStart;
                this.colorText.colorAll();
                this.richTextBox1.SelectionStart = selectionStart;
                this.richTextBox1.Focus();
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.treeView1.SelectedNode = e.Node;
            this.updateTipsBox((MyNode) e.Node);
        }

        private void treeView1_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            string tips = ((MyNode) e.Node).Tips;
            Point point = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
            point.X += 30;
            point.Y += 50;
            this.toolTip1.Show(tips, this, point, 0x1388);
        }

        private void treeView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.treeView2.SelectedNode.IsExpanded)
            {
                MyNode selectedNode = (MyNode) this.treeView2.SelectedNode;
                RichTextBox box = this.richTextBox1;
                string str = box.SelectedText + selectedNode.InsertInfo;
                box.SelectedText = str;
                this.richTextBox1.Focus();
                this.colorText.stepColor();
            }
        }

        private void treeView2_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            string tips = ((MyNode) e.Node).Tips;
            Point point = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
            point.X += 30;
            point.Y += 50;
            this.toolTip1.Show(tips, this, point, 0x1388);
        }

        private void updateFuncList()
        {
            Match match = new Regex(@"function \S*(?!=\()").Match(this.richTextBox1.Text);
            this.listBox1.Items.Clear();
            this.funcList.Clear();
            this.funcLine.Clear();
            while (match.Success)
            {
                string item = match.Value.Split(new char[] { ' ' })[1];
                this.listBox1.Items.Add(item);
                this.funcList.Add(item);
                this.funcLine.Add(this.richTextBox1.GetLineFromCharIndex(match.Index).ToString());
                match = match.NextMatch();
            }
        }

        private void updateLineState()
        {
            this.toolStripLabel3.Text = this.richTextBox1.Lines.Length.ToString();
            this.toolStripLabel1.Text = (this.richTextBox1.GetLineFromCharIndex(this.richTextBox1.SelectionStart) + 1).ToString();
        }

        private void updateTipsBox(MyNode node)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(node.InsertInfo);
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine(node.Tips);
                this.textBox1.Text = builder.ToString();
            }
            catch (Exception)
            {
                this.textBox1.Text = "";
            }
        }

        private void 保存脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.filePath))
            {
                this.另存为ToolStripMenuItem_Click(sender, e);
            }
            else
            {
                this.saveFile(this.filePath);
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectAll();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("program : Carew Los \n Email: 303359166@qq.com \n Translated: Outlaw1994", "Prompt", MessageBoxButtons.OK);
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.InitialDirectory = this.scriptFolderPath;
            this.saveFileDialog1.FileName = this.getFileName();
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.saveFile(this.saveFileDialog1.FileName);
            }
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.StringFormat, this.richTextBox1.SelectedText);
        }

        private void 定位到函数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gotoFunction();
        }

        private void 当前卡号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox box = this.richTextBox1;
            string str = box.SelectedText + this.getFileName();
            box.SelectedText = str;
        }

        private void 打开脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.richTextBox1.Text))
            {
                switch (MessageBox.Show(" whether to save the current text? ", "Prompt", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.OK:
                        this.保存脚本ToolStripMenuItem_Click(sender, e);
                        break;

                    case DialogResult.Cancel:
                        return;
                }
            }
            this.openFileDialog1.InitialDirectory = this.scriptFolderPath;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.openFile(this.openFileDialog1.FileName);
            }
        }

        private void 新建效果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ew.setCID(this.getFileName());
            if (this.ew.ShowDialog(this) == DialogResult.OK)
            {
                Clipboard.SetData(DataFormats.StringFormat, this.ew.getScript());
            }
        }

        private void 新建脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.filePath) && !string.IsNullOrEmpty(this.richTextBox1.Text))
            {
                DialogResult result = MessageBox.Show(" to save the file and then create?", "Prompt", MessageBoxButtons.YesNoCancel);
                if (DialogResult.OK == result)
                {
                    this.保存脚本ToolStripMenuItem_Click(sender, e);
                }
                else if (DialogResult.Cancel == result)
                {
                    return;
                }
            }
            this.newLua();
            this.updateLineState();
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SearchDlg(this.richTextBox1).Show(this);
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectionStart = this.richTextBox1.SelectionStart;
            string text = Clipboard.GetText(TextDataFormat.Text);
            this.richTextBox1.Paste();
            this.colorText.colorAll();
            this.richTextBox1.SelectionStart = selectionStart + text.Length;
        }

        private void 脚本文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.scriptFolderPath = this.folderBrowserDialog1.SelectedPath;
                this.loadScriptFile(this.scriptFolderPath);
            }
        }

        private void 自动换行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.WordWrap = this.自动换行ToolStripMenuItem.Checked;
        }

        private void 设置游戏路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                this.gamePath = this.openFileDialog2.FileName;
            }
        }

        private void 跳转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.insertFunction();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void 隐藏左侧ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.隐藏左侧ToolStripMenuItem.Checked)
            {
                this.splitContainer1.Panel1Collapsed = false;
            }
            else
            {
                this.splitContainer1.Panel1Collapsed = true;
            }
        }

        public static string cdbdir
        {
            [CompilerGenerated]
            get
            {
                return cdbdir;
            }
            [CompilerGenerated]
            set
            {
                cdbdir = value;
            }
        }

        private void LuaEdit_Load(object sender, EventArgs e)
        {
            this.InitialView();
            this.smgrCmd = new SearchMgr(this.originCmdRoot);
            this.smgrConst = new SearchMgr(this.originConstRoot);
        }

    }
}

