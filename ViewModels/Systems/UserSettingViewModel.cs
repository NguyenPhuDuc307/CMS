using CMS.Utils.Constants;
namespace CMS.ViewModels.Systems
{
    public class UserSettingViewModel
    {
        public UserSettingViewModel()
        {
            Theme = CMS.Utils.Constants.Theme.light.ToString();
            Direction = CMS.Utils.Constants.Direction.ltr.ToString();
            ColorTheme = CMS.Utils.Constants.ColorTheme.Purple_Theme.ToString();
            Layout = CMS.Utils.Constants.Layout.vertical.ToString();
            BoxedLayout = true;
            SidebarType = CMS.Utils.Constants.SidebarType.full.ToString();
            CardBorder = false;
        }

        public UserSettingViewModel(Theme theme, Direction direction, ColorTheme color, Layout layout, bool boxedLayout, SidebarType sidebar, bool cardBorder)
        {
            Theme = theme.ToString();
            Direction = direction.ToString();
            ColorTheme = color.ToString();
            Layout = layout.ToString();
            BoxedLayout = boxedLayout;
            SidebarType = sidebar.ToString();
            CardBorder = cardBorder;
        }

        public string Theme { get; set; }
        public string Direction { get; set; }
        public string ColorTheme { get; set; }
        public string Layout { get; set; }
        public bool BoxedLayout { get; set; }
        public string SidebarType { get; set; }
        public bool CardBorder { get; set; }
    }
}

