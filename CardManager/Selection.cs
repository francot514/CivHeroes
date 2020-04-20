namespace CardManager
{
    using System;
    using System.Runtime.CompilerServices;

    public class Selection
    {
        public bool isOneInCombines(string combines)
        {
            string str = combines;
            char[] separator = new char[] { ',' };
            foreach (string str2 in str.Split(separator))
            {
                if (this.ConstID.ToString() == str2)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return (this.ConstName + " " + this.Description);
        }

        public int CodeNeed { get; set; }

        public string Combine { get; set; }

        public int ConstID { get; set; }

        public string ConstName { get; set; }

        public int CountLimitNeed { get; set; }

        public string Description { get; set; }

        public int DlgType { get; set; }

        public int EffectType { get; set; }

        public int Enable { get; set; }

        public int RangeNeed { get; set; }

        public int TargetRangeNeed { get; set; }

        public int ValueNeed { get; set; }
    }
}

