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

        private DataContext _datacontext;

        public BooksController()
        {
            _datacontext = new DataContext();
        }


        // GET: 
        public List<Book> Get()
        {
            List<Book> books = new List<Book>();
            foreach (var e in _datacontext.Books)
            {
                books.Add(e);
            }
            return books;
        }

        // GET: 
        public BookBindModel Get(int id)
        {
            Book e = _datacontext.Books.Where(p => p.BookId == id).FirstOrDefault();
            BookBindModel book = new BookBindModel()
            {
                BookId = e.BookId,
                Isbn = e.Isbn,
                Title = e.Title,


            };
            return book;
        }

        // POST: 
        [Route("Create")]
        public async Task<IHttpActionResult> Post(BookBindModel bindBook)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }
            var model = await DesserializeObject();
            var book = new Book()
            {
                Isbn = bindBook.Isbn,
                Title = bindBook.Title,



            };
            _datacontext.Books.Add(book);
            _datacontext.SaveChanges();

            foreach (var authId in bindBook.AuthorsIdList)
            {
                Author author = _datacontext.Authors.Find(authId);
                book.Authors.Add(author);

            }

            _datacontext.Entry(book).State = System.Data.Entity.EntityState.Modified;
            _datacontext.SaveChanges();




            return Ok();
        }

        // PUT: 
        [Route("Edit")]
        public async Task<IHttpActionResult> Put()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }
            var model = await DesserializeObject();
            var bookAlterado = _datacontext.Books.Where(e => e.BookId == model.BookId).FirstOrDefault();
            if (bookAlterado != null)
            {
                bookAlterado.Title = model.Title;
               

            }
            _datacontext.SaveChanges();
            return Ok();
        }

        // DELETE: 
        public void Delete(int id)
        {
            var bookDeletado = _datacontext.Books.Where(e => e.BookId == id).FirstOrDefault();
            if (bookDeletado != null)
            {
                _datacontext.Books.Remove(bookDeletado);
            }
            _datacontext.SaveChanges();
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



