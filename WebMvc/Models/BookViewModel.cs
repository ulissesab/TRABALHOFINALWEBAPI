using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMvc.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public ICollection<Author> Authors { get; set; }

      
        }
    }
