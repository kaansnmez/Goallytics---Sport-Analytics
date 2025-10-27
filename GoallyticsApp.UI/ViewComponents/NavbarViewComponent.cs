using Microsoft.AspNetCore.Mvc;

namespace GoallyticsApp.UI.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
