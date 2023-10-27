using System.ComponentModel.DataAnnotations;

namespace ForesTitle.web.Models
{
    public class ArticleComment
    {
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }
       // public  string UserId { get; set; }
        public DateTime PublishData { get; set; }
        public User User { get; set; }
        public int ArticleId { get; set; }
        public  Article  Article { get; set; }
      

    }
}
