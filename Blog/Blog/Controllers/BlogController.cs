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
        private IBlogItemRepository repository;
        public BlogController(IBlogItemRepository repository)
        {
            this.repository = repository;
        }
        public ViewResult List()
        {
            return View(repository.BlogItems);
        }

        List<BlogItem> items = new List<BlogItem>()
        {
            new BlogItem(){
                Id = 1,
                Title="Programowanie w ASP.NET",
                Content="ASP.NET – zbiór technologii opartych na frameworku zaprojektowanym przez firmę Microsoft. Przeznaczony jest do budowy różnorodnych aplikacji internetowych, a także aplikacji typu XML Web Services. ",
                CreationTimestamp=DateTime.Now
            },
            new BlogItem(){
                Id = 2,
                Title="Programowanie w C#",
                Content="wieloparadygmatowy język programowania zaprojektowany w latach 1998–2001 przez zespół pod kierunkiem Andersa Hejlsberga dla firmy Microsoft.",
                CreationTimestamp=DateTime.Now
            }
        };

        static int index = 2;

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddForm()
        {
            return View();
        }

        public IActionResult Add(BlogItem item)
        {
            if (ModelState.IsValid)
            {
                item.Id = ++index;
                item.CreationTimestamp = DateTime.Now;
                items.Add(item);
                return View("ConfirmBlogItem", item);
            } 
            else
            {
                return View("AddForm"); //nie ma widoku o nazwie Add 
            }
        }

        //public IActionResult List()
        //{
        //    return View(items);
        //}


        public IActionResult Edit(int id)
        {
            // potrzebujemy obiektu 
            BlogItem editedItem = null;

            foreach (var item in items)
            {
                if(item.Id == id)
                {
                    editedItem = item;
                    break;
                }
            }

            return View("EditForm", editedItem);
        }

        [HttpPost] //chociaz 2 metody akcji sie tak samo nazywają to obsługiwane sa przez inne żądania
        public IActionResult Edit(BlogItem itemFromForm)
        {
            int id = itemFromForm.Id;
            BlogItem originItem = items.Find(item => item.Id == id);
            originItem.Content = itemFromForm.Content; // kopiujemy pola z formularzu
            originItem.Title = itemFromForm.Title;

            return View("List", items);
        }

        /*
         * 
         *    Dodaj metodę akcji Delete, która usuwa wpis z items o podanym id i
         *    wyświetla liste wszystkich wpisów
         *    i szczegóły
         *    
         */

        public IActionResult Delete(int id)
        {
            BlogItem deletedItem = items.Find(item => item.Id == id);
            items.Remove(deletedItem);
            return View("List", items);
        }

        public IActionResult Details(int id)
        {
            BlogItem detailItem = items.Find(item => item.Id == id);

            return View("Details", detailItem);
        }
    }
}
