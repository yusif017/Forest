namespace ForesTitle.web.Models
{
    public class BaseEntity
    {   public int Id { get; set; }
        public DateTime CreateData { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
