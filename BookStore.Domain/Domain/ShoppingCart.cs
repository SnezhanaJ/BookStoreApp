using BookStore.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Domain
{
    public class ShoppingCart : BaseEntity
    {
        public string? OwnerId { get; set; }
        public BookStoreUsers? Owner { get; set; }
        public virtual ICollection<BooksInShoppingCart>? BooksInShoppingCarts { get; set; }
    }
}
