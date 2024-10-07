using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers.Components;

public class SearchBarViewComponent : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult((IViewComponentResult)View("Default"));
    }
}