using Microsoft.AspNetCore.Mvc;

public class BreadcrumbViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        // Lấy tiêu đề từ ViewData, nếu không có thì đặt giá trị mặc định
        var title = ViewData["title"]?.ToString() ?? "Trang chủ";

        return View((object)title);
    }
}