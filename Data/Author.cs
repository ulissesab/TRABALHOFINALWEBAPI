using System;
using System.Collections.Generic;

namespace Data
{
    public class Author
    {
        public object Id;

        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Book> Books { get; set; }

        public static void Add(Author author)
        {
            throw new NotImplementedException();
        }
    }
}