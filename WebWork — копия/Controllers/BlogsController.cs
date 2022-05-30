using Microsoft.AspNetCore.Mvc;

namespace WebWork.Controllers;

public class BlogsController : Controller
{
    public IActionResult Index() => View(); // должен вернуть представление списка блогов - blog.html

    public IActionResult ShopBlog() => View(); // blog-single.html
}
