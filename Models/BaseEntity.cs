namespace ContactManager.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTime.UtcNow;
        }
    }
}
