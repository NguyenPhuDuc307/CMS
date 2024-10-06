using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Utils.Constants;

namespace CMS.Data.Entities.Systems
{
    public class UserSetting
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string? UserId { get; set; }
        public User? User { get; set; }

        public Theme Theme { get; set; }
        public Direction Direction { get; set; }
        public Color Color { get; set; }
        public Layout Layout { get; set; }
        public Container Container { get; set; }
        public Sidebar Sidebar { get; set; }
        public Card Card { get; set; }
    }
}