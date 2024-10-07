using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Utils.Constants;

namespace CMS.Data.Entities.Systems
{
    public class UserSetting
    {
        public UserSetting()
        {
            Theme = Theme.dark;
            Direction = Direction.LTR;
            Color = Color.BLUE_THEME;
            Layout = Layout.vertical;
            BoxLayout = true;
            Sidebar = Sidebar.full;
            CardBorder = false;
        }
        public int Id { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string? UserId { get; set; }
        public User? User { get; set; }

        public Theme Theme { get; set; }
        public Direction Direction { get; set; }
        public Color Color { get; set; }
        public Layout Layout { get; set; }
        public bool BoxLayout { get; set; }
        public Sidebar Sidebar { get; set; }
        public bool CardBorder { get; set; }
    }
}