namespace Goods.API.Contracts
{
    public class Product
    {
        public uint Id { get; set; }
        public string ArticleNumber { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Params { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}
