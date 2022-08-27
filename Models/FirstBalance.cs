using CsvHelper.Configuration.Attributes;

namespace GoodsStore.Models
{
    public class FirstBalance
    {
        [Index(0)]
        public int GoodId { get; set; }
        [Index(1)]
        public int FrstBalance { get; set; }
    }
}
