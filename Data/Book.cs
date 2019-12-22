using System.Collections.Generic;

namespace Data
{
    public class Book
    {

        public int  Id { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public ICollection<Author> Authors { get; set; }
        
    }
}