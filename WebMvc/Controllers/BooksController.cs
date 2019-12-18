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

        public int IdBook { get; private set; }

        // GET: 
        public async Task<ActionResult> Index()
        {
            return View(await GetBooks() ?? new List<BookViewModel>());
        }



        // GET: /Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await GetBookById(id));
        }

        // GET: /Create
        public ActionResult Create()
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
            if (await GetBooks() != null)
            {
                ViewBag.BooksList = new SelectList(await GetBooks(), "Id", "Nome");
            }
            return View(await GetBookById(id));
        }

        // POST: /Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, BookViewModel model)
        {
            int IdBook = int.Parse(Request.Form["BooksList"].ToString());
            model.BookId = IdBook;
            try
            {
                return await CreateAndSendForm("Edit", model);
            }
            catch
            {
                return View("Index");
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
                    client.BaseAddress = new Uri("http://localhost:49578/");
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
        public async Task<BookViewModel> GetBookById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49578/");
                var response = await client.GetAsync("/api/Books/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var bookById = await response.Content.ReadAsAsync<BookViewModel>();
                    return bookById;
                }
                return null;
            }
        }

        [HttpGet]
        public async Task<IEnumerable<BookViewModel>> GetBooks()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49578/");
                var response = await client.GetAsync("/api/ books");
                if (response.IsSuccessStatusCode)
                {
                    var books = await response.Content.ReadAsAsync<IEnumerable<BookViewModel>>();
                    return books;
                }
                return null;
            }
        }

       

        public async Task<ActionResult> CreateAndSendForm(string path, BookViewModel model)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    client.BaseAddress = new Uri("http://localhost:49578/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    content.Add(new StringContent(JsonConvert.SerializeObject(model)));

                    var response = path == "Create" ? await client.PostAsync("/api/Books/" + path, content) :
                                                        await client.PutAsync("/api/Books/" + path, content); ;
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

    }

}


        