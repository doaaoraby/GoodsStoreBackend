namespace GoodsStore.Models
{
    public class Goods
    {
        //[Index(0)]
        public int GoodId { get; set; }
        public int TransactionId { get; set; }
        public string TransactionDate { get; set; }
        public string Amount { get; set; }
        public string Direction { get; set; }
        public string? Comments { get; set; }

    }
    public class ReturnedData
    {
        public List<Goods> goodsIDs;
        public int firstBalance;
    }
}
