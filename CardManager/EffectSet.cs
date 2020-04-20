namespace CardManager
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class EffectSet
    {
        public string getScript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("       local " + this.Name + "=Effect.CreateEffect(c)");
            if (this.Description != -1)
            {
                builder.AppendLine(string.Concat(new object[] { "       ", this.Name, ":SetDescription(aux.Stringid(c", this.cID, ",", Description.ToString(), "))" }));
            }
            if (!string.IsNullOrEmpty(this.Type))
            {
                builder.AppendLine("       " + this.Name + ":SetType(" + this.Type + ")");
            }
            if (!string.IsNullOrEmpty(this.Code))
            {
                builder.AppendLine("       " + this.Name + ":SetCode(" + this.Code + ")");
            }
            if (!string.IsNullOrEmpty(this.Range))
            {
                builder.AppendLine("       " + this.Name + ":SetRange(" + this.Range + ")");
            }
            if (!string.IsNullOrEmpty(this.TargetRange1) && !string.IsNullOrEmpty(this.TargetRange2))
            {
                builder.AppendLine("       " + this.Name + ":SetTargetRange(" + this.TargetRange1 + "," + this.TargetRange2 + ")");
            }
            if (!string.IsNullOrEmpty(this.Category))
            {
                builder.AppendLine("       " + this.Name + ":SetCategory(" + this.Category + ")");
            }
            if (!string.IsNullOrEmpty(this.Reset))
            {
                if (this.ResetCount == -1)
                {
                    builder.AppendLine("       " + this.Name + ":SetReset(" + this.Reset + ")");
                }
                else
                {
                    builder.AppendLine(string.Concat(new object[] { "       ", this.Name, ":SetReset(", this.Reset, ",", this.ResetCount.ToString(), ")" }));
                }
            }
            if (!string.IsNullOrEmpty(this.Property))
            {
                builder.AppendLine("       " + this.Name + ":SetProperty(" + this.Property + ")");
            }
            if (this.CountLimit != -1)
            {
                builder.AppendLine(string.Concat(new object[] { "       ", this.Name, ":SetCountLimit(", this.CountLimit.ToString(), ")" }));
            }
            if (!string.IsNullOrEmpty(this.Condition))
            {
                builder.AppendLine("       " + this.Name + ":SetCondition(" + this.cID + "." + this.Condition + ")");
            }
            if (!string.IsNullOrEmpty(this.Target))
            {
                builder.AppendLine("       " + this.Name + ":SetTarget(" + this.cID + "." + this.Target + ")");
            }
            if (!string.IsNullOrEmpty(this.Cost))
            {
                builder.AppendLine("       " + this.Name + ":SetCost(" + this.cID + "." + this.Cost + ")");
            }
            if (!string.IsNullOrEmpty(this.Operation))
            {
                builder.AppendLine("       " + this.Name + ":SetOperation(" + this.cID + "." + this.Operation + ")");
            }
            if (!string.IsNullOrEmpty(this.Value))
            {
                builder.AppendLine("       " + this.Name + ":SetValue(" + this.cID + "." + this.Value + ")");
            }
            switch (this.RegistTo)
            {
                case 0:
                    builder.AppendLine("       c:RegisterEffect(" + this.Name + ")");
                    break;

                case 1:
                    builder.AppendLine("       Duel.RegisterEffect(Effect e,integer player)");
                    break;
            }
            return builder.ToString();
        }

        public override string ToString()
        {
            return (this.cID + "." + this.Name);
        }

        public string Category { get; set; }

        public string cID { get; set; }

        public string Code { get; set; }

        public string Condition { get; set; }

        public string Cost { get; set; }

        public int CountLimit { get; set; }

        public int Description { get; set; }

        public string Name { get; set; }

        public string Operation { get; set; }

        public string Property { get; set; }

        public string Range { get; set; }

        public int RegistTo { get; set; }

        public string Reset { get; set; }

        public int ResetCount { get; set; }

        public string Target { get; set; }

        public string TargetRange1 { get; set; }

        public string TargetRange2 { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}

