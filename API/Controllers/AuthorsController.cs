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
    [RoutePrefix("api/Authors")]
    public class AuthorsController : ApiController
    {
        private DataContext db = new DataContext();
        private DataContext _datacontext;

        public AuthorsController()
        {
            _datacontext = new DataContext();

        }




        // GET: 
        public List<Author> Get()
        {
          
            
            return _datacontext.Authors.ToList();
        }

        // GET: 
        [Route("GetAuthorById/{id}")]
        public AuthorBindModel Get(int AuthorId)
        {
            Author p = _datacontext.Authors.Where(author =>AuthorId == AuthorId).FirstOrDefault();
            AuthorBindModel authorSelecionado = new AuthorBindModel()
            {
                AuthorId = p.AuthorId,

            };
            return authorSelecionado;
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
            var AuthorCriado = new Author()
            {
                FirstName = model.FirstName,

            };
            _datacontext.Authors.Add(AuthorCriado);
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
            var AuthorToUpdate = _datacontext.Authors.Where(a => a.AuthorId == model.AuthorId).FirstOrDefault();
            if (AuthorToUpdate != null)
            {
                AuthorToUpdate.FirstName = model.FirstName;

            }
            _datacontext.SaveChanges();
            return Ok();
        }

        // DELETE: 
        public void Delete(int Authorid)
        {
            var AuthorToDelete = _datacontext.Authors.Where(a => a.AuthorId == Authorid).FirstOrDefault();
            if (AuthorToDelete != null)
            {
                _datacontext.Authors.Remove(AuthorToDelete);
            }
            _datacontext.SaveChanges();
        }

        public async Task<AuthorBindModel> DesserializeObject()
        {
            var result = await Request.Content.ReadAsMultipartAsync();
            var requestJson = await result.Contents[0].ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<AuthorBindModel>(requestJson);
            if (result.Contents.Count > 1)
            {

            }
            return model;
        }
    }
}




