using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class BookBindModel
    {
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public ICollection<Author> Authors { get; set; }
        
    }
}