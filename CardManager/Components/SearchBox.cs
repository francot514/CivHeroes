using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace CardManager.Components
{
    public sealed class SearchBox : GroupBox
    {
        private readonly object m_searchlock = new object();
        private readonly ListBox m_searchList = new ListBox 
        { 
            Dock = DockStyle.Fill, 
            IntegralHeight = false, 
            DrawMode = DrawMode.OwnerDrawFixed 
        };
        private readonly TextBox m_searchInput = new TextBox
        {
            Dock = DockStyle.Fill,
            Text = "Search",
            TextAlign = HorizontalAlignment.Center, 
            ForeColor = SystemColors.WindowFrame 
        };

        public SearchBox()
        {
            Dock = DockStyle.Fill;
            var panel = new TableLayoutPanel {ColumnCount = 1};
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.Controls.Add(m_searchList, 0, 0);
            panel.Controls.Add(m_searchInput, 0, 1);
            panel.Dock = DockStyle.Fill;
            panel.RowCount = 2;
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 86.60714F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));

            m_searchInput.Enter += SearchInput_Enter;
            m_searchInput.Leave += SearchInput_Leave;
            m_searchInput.TextChanged += SearchInput_TextChanged;

            m_searchList.DrawItem += SearchList_DrawItem;

            Controls.Add(panel);
        }

        private void SearchInput_Enter(object sender, EventArgs e)
        {
            if (m_searchInput.Text == "Search")
            {
                m_searchInput.Text = "";
                m_searchInput.ForeColor = SystemColors.WindowText;
            }
        }

        private void SearchInput_Leave(object sender, EventArgs e)
        {
            if (m_searchInput.Text == "")
            {
                m_searchInput.Text = "Search";
                m_searchInput.ForeColor = SystemColors.WindowFrame;
            }
        }

        private void SearchInput_TextChanged(object sender, EventArgs e)
        {
            lock (m_searchlock)
            {
                if (m_searchInput.Text == "")
                {
                    m_searchList.Items.Clear();
                    return;
                }
                if (m_searchInput.Text != "Search")
                {
                    m_searchList.Items.Clear();
                    foreach (int card in Program.CardData.Keys.Where(card => Program.CardData[card].Id.ToString(CultureInfo.InvariantCulture).ToLower().StartsWith(m_searchInput.Text.ToLower()) ||
                                                                             Program.CardData[card].Name.ToLower().Contains(m_searchInput.Text.ToLower())))
                    {
                        AddCardToList(Program.CardData[card].Id.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }

        private void AddCardToList(string id)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AddCardToList), id);
                return;
            }

            m_searchList.Items.Add(id);

        }

        private void SearchList_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            int index = e.Index;
            if (index >= 0 && index < m_searchList.Items.Count)
            {
                string text = m_searchList.Items[index].ToString();
                Graphics g = e.Graphics;
                if (!Program.CardData.ContainsKey(Int32.Parse(text)))
                    m_searchList.Items.Remove(text);
                else
                {
                    CardInfo card = Program.CardData[Int32.Parse(text)];

                    g.FillRectangle((selected) ? new SolidBrush(Color.Blue) : new SolidBrush(Color.White), e.Bounds);

                    // Print text
                    g.DrawString((card.Name == "" ? card.Id.ToString(CultureInfo.InvariantCulture) : card.Name), e.Font,
                                 (selected) ? Brushes.White : Brushes.Black,
                                 m_searchList.GetItemRectangle(index).Location);
                }
            }

            e.DrawFocusRectangle();
        }
        public ListBox List
        {
            get { return m_searchList; }
        }
    }
}
