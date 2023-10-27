namespace ForesTitle.web.Models
{
    public class Category :BaseEntity
    {
        public string CategoryName { get; set; }
        public List<Article>? Article { get; set; }
        public List<ArticleSab>? ArticleSab { get; set; }




    }
}
