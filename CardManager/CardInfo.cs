using System.Linq;
using CardManager.Enums;
using System;
using System.Collections.Generic;

namespace CardManager
{
    public class CardInfo: ICloneable
    {

        public CardInfo(string[] carddata)
        {
            this.Name = "";
            this.Id = int.Parse(carddata[0]);
            this.Ot = int.Parse(carddata[1]);
            this.AliasId = int.Parse(carddata[2]);
            this.SetCode = long.Parse(carddata[3]);
            this.Type = int.Parse(carddata[4]);
            uint num = uint.Parse(carddata[5]);
            this.Level = num & 0xff;
            this.LScale = (num >> 0x18) & 0xff;
            this.RScale = (num >> 0x10) & 0xff;
            this.Race = int.Parse(carddata[6]);
            this.Attribute = int.Parse(carddata[7]);
            this.Atk = int.Parse(carddata[8]);
            this.Def = int.Parse(carddata[9]);
            this.Category = long.Parse(carddata[10]);
        }

        public void SetCardText(string[] cardtext)
        {
            Name = cardtext[1];
            Description = cardtext[2];
            var effects = new List<string>();

            for (var i = 3; i < cardtext.Length; i++)
            {
                if(cardtext[i] != "")
                    effects.Add(cardtext[i]);
            }
            EffectStrings = effects.ToArray();

        }

        public CardType[] GetCardTypes()
        {
            var typeArray = Enum.GetValues(typeof(CardType));
            return typeArray.Cast<CardType>().Where(type => ((Type & (int) type) != 0)).ToArray();
        }

        //public int[] GetCardSets(List<int>setArray)
        //{
        //    var sets = new List<int> {setArray.IndexOf(SetCode & 0xffff), setArray.IndexOf(SetCode >> 0x10)};
        //    return sets.ToArray();
        //}

        public object Clone()
        {
            return  MemberwiseClone();
        }

        public int AliasId { get; set; }

        public int Atk { get; set; }

        public int Attribute { get; set; }

        public int Def { get; set; }

        public string Description { get; set; }

        public int Id { get; private set; }

        public uint Level { get; set; }

        public uint LScale { get; set; }

        public uint RScale { get; set; }

        public string Name = "";

        public int Race { get; set; }

        public int Type { get; set; }

        public long Category { get; set; }

        public int Ot { get; set; }

        public string[] EffectStrings { get; set; }

        public long SetCode { get; set; }

    }
}