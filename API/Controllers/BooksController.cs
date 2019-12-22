using API.Models;
using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/Books")]
    public class BooksController : ApiController
    {
        private DataContext db = new DataContext();
        private DataContext _datacontext;

        public BooksController()
        {
            _datacontext = new DataContext();
        }


        // GET: 
        public List<Book> Get()
        {


            return _datacontext.Books.ToList();
        }

        // GET: 
        [Route("GetBookById/{id}")]
        public BookBindModel Get(int id)
        {
            var model = _datacontext.Books.Where(a => a.Id == id).FirstOrDefault();

            BookBindModel bookSelecionado = new BookBindModel()
            {
                Id = model.Id,
                Isbn = model.Isbn,
                Title= model.Title,

            };
            return bookSelecionado;
        }

        // POST: 
        [Route("Create")]
        public async Task<IHttpActionResult> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }
            var model = await DesserializeObject();
            var BookCriado = new Book()
            {
                Isbn = model.Isbn,
                Title= model.Title,
            };
            _datacontext.Books.Add(BookCriado);
            _datacontext.SaveChanges();
            return Ok();
        }

        // PUT: 
        public async Task<IHttpActionResult> Put()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }
            var model = await DesserializeObject();
            var BookToUpdate = _datacontext.Books.Where(a => a.Id == model.Id).FirstOrDefault();
            if (BookToUpdate != null)
            {
                BookToUpdate.Isbn = model.Isbn;
                BookToUpdate.Title= model.Title;

            }
            _datacontext.SaveChanges();
            return Ok();
        }

        // DELETE: 
        public void Delete(int id)
        {
            var bookRemovido = _datacontext.Books.Where(a => a.Id == id).FirstOrDefault();
            if (bookRemovido != null)
            {
                _datacontext.Books.Remove(bookRemovido);

                _datacontext.SaveChanges();
            }
        }



        public async Task<BookBindModel> DesserializeObject()
        {
            var result = await Request.Content.ReadAsMultipartAsync();
            var requestJson = await result.Contents[0].ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<BookBindModel>(requestJson);

            return model;
        }
    }
}






