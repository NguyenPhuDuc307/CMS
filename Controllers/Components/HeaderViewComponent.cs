using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers.Components;

public class HeaderViewComponent : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult((IViewComponentResult)View("Default"));
    }
}