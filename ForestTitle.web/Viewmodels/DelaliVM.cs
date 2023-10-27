

using ForesTitle.web.Models;

namespace ForesTitle.web.Viewmodels
{
    public class DelaliVM
    {
        public Article Articles { get; set; }
        public List<Article> SimilarArticle { get; set; }
        public List<ArticleComment> ArticelComments { get; set; }

    }
}
