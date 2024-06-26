using BookStore.Domain;
using BookStore.Domain.Domain;
using BookStore.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BookStore.Repository
{
    public class ApplicationDbContext : IdentityDbContext<BookStoreUsers>
    {

        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<BooksInShoppingCart> BooksInShoppingCarts { get; set; }
        public virtual DbSet<BookInOrder> BookInOrder { get; set; } 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
