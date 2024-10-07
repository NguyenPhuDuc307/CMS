using CMS.Utils.Constants;
namespace CMS.Models.Systems
{
    public class UserSettingViewModel
    {
        public UserSettingViewModel()
        {
            Theme = CMS.Utils.Constants.Theme.light.ToString();
            Direction = CMS.Utils.Constants.Direction.LTR.ToString();
            Color = CMS.Utils.Constants.Color.BLUE_THEME.ToString();
            Layout = CMS.Utils.Constants.Layout.vertical.ToString();
            BoxLayout = true;
            Sidebar = CMS.Utils.Constants.Sidebar.full.ToString();
            CardBorder = false;
        }

        public UserSettingViewModel(Theme theme, Direction direction, Color color, Layout layout, bool boxLayout, Sidebar sidebar, bool cardBorder)
        {
            Theme = theme.ToString();
            Direction = direction.ToString();
            Color = color.ToString();
            Layout = layout.ToString();
            BoxLayout = boxLayout;
            Sidebar = sidebar.ToString();
            CardBorder = cardBorder;
        }

        public string Theme { get; set; }
        public string Direction { get; set; }
        public string Color { get; set; }
        public string Layout { get; set; }
        public bool BoxLayout { get; set; }
        public string Sidebar { get; set; }
        public bool CardBorder { get; set; }
    }
}

