namespace CardManager
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class MyNode : TreeNode, IComparable
    {
        private string descr;

        public MyNode()
        {
            this.Description = "";
            this.InsertInfo = "";
            this.Tips = "";
        }

        public int CompareTo(object obj)
        {
            return base.Text.CompareTo(((TreeNode) obj).Text);
        }

        public string ToString()
        {
            return (this.Description + this.InsertInfo + this.Tips);
        }

        public string Description
        {
            get
            {
                return this.descr;
            }
            set
            {
                base.Text = value;
                this.descr = value;
            }
        }

        public string InsertInfo { get; set; }

        public string Tips { get; set; }
    }
}

