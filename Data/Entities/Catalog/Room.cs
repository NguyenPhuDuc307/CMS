using CMS.Data.Entities.Systems;

namespace CMS.Data.Entities.Catalog
{
    public class Room
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public User? Admin { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
