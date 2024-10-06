using CMS.Data.Entities.Systems;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.Models.Systems
{
    public class EditUserViewModel
    {
        public User? User { get; set; }

        public IList<SelectListItem>? Roles { get; set; }
    }
}
