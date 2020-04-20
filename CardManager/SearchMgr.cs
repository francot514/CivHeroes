namespace CardManager
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    internal class SearchMgr
    {
        private int curPos;
        private TreeNode originRoot;
        private List<TreeNode> results;

        public SearchMgr(TreeNode origin)
        {
            this.originRoot = origin;
            this.results = new List<TreeNode>();
        }

        public List<TreeNode> getResults()
        {
            return this.results;
        }

        public bool isHasNext()
        {
            return (this.curPos != this.results.Count);
        }

        public TreeNode next()
        {
            if (this.results.Count == 0)
            {
                return null;
            }
            this.curPos++;
            if (this.curPos >= this.results.Count)
            {
                this.curPos = this.results.Count - 1;
            }
            return this.results[this.curPos];
        }

        public TreeNode previous()
        {
            if (this.results.Count == 0)
            {
                return null;
            }
            this.curPos--;
            if (this.curPos < 0)
            {
                this.curPos = 0;
            }
            return this.results[this.curPos];
        }

        public TreeNode search(string str)
        {
            List<TreeNode> list = new List<TreeNode>();
            string[] strArray = str.Split(new char[] { ' ' });
            this.results.Clear();
            this.searchNodes(this.originRoot, strArray[0]);
            for (int i = 1; i < strArray.Length; i++)
            {
                foreach (MyNode node in this.results)
                {
                    if (!node.ToString().Contains(strArray[i]))
                    {
                        list.Add(node);
                    }
                }
            }
            foreach (TreeNode node2 in list)
            {
                this.results.Remove(node2);
            }
            this.curPos = 0;
            if (this.results.Count == 0)
            {
                return null;
            }
            return this.results[0];
        }

        private void searchNodes(TreeNode node, string str)
        {
            MyNode item = (MyNode) node;
            if (item.Nodes.Count == 0)
            {
                if (item.ToString().ToLower().Contains(str.ToLower()))
                {
                    this.results.Add(item);
                }
            }
            else
            {
                foreach (TreeNode node3 in item.Nodes)
                {
                    this.searchNodes(node3, str);
                }
            }
        }
    }
}

