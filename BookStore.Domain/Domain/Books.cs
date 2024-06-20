using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Domain
{
    public class Books : BaseEntity
    {
        public string? BookImage { get; set; }
        public string? Title { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Guid PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }
        public virtual ICollection<BooksInShoppingCart>? BooksInShoppingCarts { get; set; }
    }
}
