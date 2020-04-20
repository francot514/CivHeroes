using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardManager
{
    class CardData
    {
        public CardData(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public string Name { get; set; }
        public int ID { get; set; }
    }
}
