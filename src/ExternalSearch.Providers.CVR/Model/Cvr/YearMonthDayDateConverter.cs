using Newtonsoft.Json.Converters;

namespace CluedIn.ExternalSearch.Providers.CVR.Model.Cvr
{
    public class YearMonthDayDateConverter : IsoDateTimeConverter
    {
        public YearMonthDayDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
