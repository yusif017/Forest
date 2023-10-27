
namespace ForesTitle.web.Models
{
    public class ArticleSab : BaseEntity
    {

        public string Title { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string SeoUrl { get; set; }
        public string PhotoUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ArticleSabTag> ArticleSabTag { get; set; }
    }
}
