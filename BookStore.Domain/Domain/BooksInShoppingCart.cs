using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Domain
{
    public class BooksInShoppingCart : BaseEntity
    {
        public Guid BookId { get; set; }
        public Guid ShoppingCartId { get; set; }
        public Books? Book { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        public int Quantity { get; set; }
    }
}
