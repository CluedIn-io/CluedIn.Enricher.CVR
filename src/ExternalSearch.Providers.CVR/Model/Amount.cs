using System.Diagnostics;

namespace CluedIn.ExternalSearch.Providers.CVR.Model
{
    [DebuggerDisplay("{Value} {UnitClean}")]
    public class Amount
    {
        public long Value { get; set; }
        public int Decimals { get; set; }
        public string Unit { get; set; }

        public string UnitClean
        {
            get
            {
                return this.Unit.Replace("iso4217:", string.Empty);
            }
        }


        public override string ToString()
        {
            return string.Format("{0} {1}", this.Value, this.Unit);
        }
    }
}