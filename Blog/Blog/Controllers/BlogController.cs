using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddForm()
        {
            return View();
        }

        List<BlogItem> items = new List<BlogItem>()
        {
            new BlogItem(){
                Title="Programowanie w ASP.NET",
                Content="ASP.NET – zbiór technologii opartych na frameworku zaprojektowanym przez firmę Microsoft. Przeznaczony jest do budowy różnorodnych aplikacji internetowych, a także aplikacji typu XML Web Services. ",
                CreationTimestamp=DateTime.Now
            },
            new BlogItem(){
                Title="Programowanie w C#",
                Content="wieloparadygmatowy język programowania zaprojektowany w latach 1998–2001 przez zespół pod kierunkiem Andersa Hejlsberga dla firmy Microsoft.",
                CreationTimestamp=DateTime.Now
            }
        };
        public IActionResult Add(BlogItem item)
        {
            items.Add(item);
            return View("ConfirmBlogItem", item);
        }

        public IActionResult List()
        {
            return View(items);
        }
    }
}
