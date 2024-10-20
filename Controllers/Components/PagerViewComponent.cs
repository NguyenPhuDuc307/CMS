using CMS.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers.Components;

public class PagerViewComponent : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(PaginationBase result)
    {
        return Task.FromResult((IViewComponentResult)View("Default", result));
    }
}