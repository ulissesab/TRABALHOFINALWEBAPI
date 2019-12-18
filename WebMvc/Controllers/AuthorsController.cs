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
    public class AuthorsController : Controller
    {



        // GET: 
        public async Task<ActionResult> Index()
        {
            IEnumerable<AuthorViewModel> authors = await GetAuthors();
            return View(authors);
        }

        // GET: /Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await GetAuthorById(id));
        }

        // GET: /Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Create
        [HttpPost]
        public async Task<ActionResult> Create(AuthorViewModel model)
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

                        var response = await client.PostAsync("/api/Authors/Create", content);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index", "Authors");
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
            return View(await GetAuthorById(id));
        }

        // POST: /Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(AuthorViewModel model)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    client.BaseAddress = new Uri("http://localhost:49578/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    content.Add(new StringContent(JsonConvert.SerializeObject(model)));

                    var response = await client.PutAsync("/api/Authors", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Authors");
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
            return View(await GetAuthorById(id));
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
                    var response = await client.DeleteAsync("/api/Authors/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Authors");
                    }
                    else
                    {
                        return View("Error");
                    }

                }
            }
        }


        [HttpGet]
        public async Task<IEnumerable<AuthorViewModel>> GetAuthors()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49578/");
                var response = await client.GetAsync("/api/authors");
                if (response.IsSuccessStatusCode)
                {
                    var authors = await response.Content.ReadAsAsync<IEnumerable<AuthorViewModel>>();
                    return authors;
                }
                return null;
            }
        }  
            [HttpGet]
        public async Task<AuthorViewModel> GetAuthorById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49578/");
                var response = await client.GetAsync("/api/Authors/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var AuthorById = await response.Content.ReadAsAsync<AuthorViewModel>();
                    return AuthorById;
                }
                return null;
            }
        }
    }
}