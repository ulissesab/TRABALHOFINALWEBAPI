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
        public AuthorBindModel Get(int id)
        {
            var model = _datacontext.Authors.Where(a => a.Id == id).FirstOrDefault();

            AuthorBindModel authorSelecionado = new AuthorBindModel()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,

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
                LastName = model.LastName,
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
            var AuthorToUpdate = _datacontext.Authors.Where(a => a.Id == model.Id).FirstOrDefault();
            if (AuthorToUpdate != null)
            {
                AuthorToUpdate.FirstName = model.FirstName;
                AuthorToUpdate.LastName = model.LastName;

            }
            _datacontext.SaveChanges();
              return Ok();
        }

        // DELETE: 
        public void Delete(int id)
        {
            var authorRemovido = _datacontext.Authors.Where(a => a.Id == id).FirstOrDefault();
             if (authorRemovido != null)
            {
                _datacontext.Authors.Remove(authorRemovido);

                _datacontext.SaveChanges();
            }
        }
    


          public async Task<AuthorBindModel> DesserializeObject()
          {
              var result = await Request.Content.ReadAsMultipartAsync();
              var requestJson = await result.Contents[0].ReadAsStringAsync();
              var model = JsonConvert.DeserializeObject<AuthorBindModel>(requestJson);
          
              return model;
        }
    }
}





