using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMvc.Models;

namespace WebMvc.Controllers
{


    public class BooksController : Controller
    {



        // GET: 
        public async Task<ActionResult> Index()
        {

            IEnumerable<BookViewModel> books = await GetBooks();
            return View(books);

        }

        // GET: /Details/5
        public async Task<ActionResult> Details(int id)
        {
            BookViewModel book = await GetBookById(id);
            return View(book);
        }

        // GET: /Create
        public ActionResult Create( )
        {
            
            return View();
        }

        // POST: /Create
        [HttpPost]
        public async Task<ActionResult> Create(BookViewModel model)
        {

           
            try
            {
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        client.BaseAddress = new Uri("http://localhost:49578/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        content.Add(new StringContent(JsonConvert.SerializeObject(model)));

                        var response = await client.PostAsync("/api/Books/Create", content);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index", "Books");
                        }
                        else
                        {
                            return View("Error");
                        }
                    }
                }
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: /Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await GetBookById(id));
        }

        // POST: /Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(BookViewModel model)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    client.BaseAddress = new Uri("http://localhost:49578/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    content.Add(new StringContent(JsonConvert.SerializeObject(model)));

                    var response = await client.PutAsync("/api/Books", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Books");
                    }
                    else
                    {
                        return View("Error");
                    }

                }

            }

        }

        // GET: /Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View(await GetBookById(id));
        }
        // POST: /Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    client.BaseAddress = new Uri("http://localhost:49578/api/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    var response = await client.DeleteAsync("/api/Books/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Books");
                    }
                    else
                    {
                        return View("Error");
                    }

                }
            }
        }


        [HttpGet]
        public async Task<IEnumerable<BookViewModel>> GetBooks()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49578/");
                var response = await client.GetAsync("/api/books");
                if (response.IsSuccessStatusCode)
                {
                    var books = await response.Content.ReadAsAsync<IEnumerable<BookViewModel>>();
                    return books;
                }
                return null;
            }
        }
        [HttpGet]
        public async Task<BookViewModel> GetBookById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49578/");
                var response = await client.GetAsync("/api/Books/GetBookById/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var book = await response.Content.ReadAsAsync<BookViewModel>();
                    return book;
                }
                return null;
            }
        }
    }
}

