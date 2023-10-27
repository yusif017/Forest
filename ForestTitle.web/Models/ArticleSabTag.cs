namespace ForesTitle.web.Models
{
    public class ArticleSabTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public int ArticleSabId { get; set; }
        public ArticleSab ArticleSab { get; set; }
   

    }
}
