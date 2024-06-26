using BookStore.Domain.Identity;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext context;
        private DbSet<BookStoreUsers> entities;
        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<BookStoreUsers>(); 
        }

        public void Delete(BookStoreUsers entity)
        {

            entities.Remove(entity);
            context.SaveChanges();
        }

        public BookStoreUsers Get(string? id)
        {
            return entities
            .Include(z => z.ShoppingCart)
            .Include("ShoppingCart.BooksInShoppingCarts")
            .Include("ShoppingCart.BooksInShoppingCarts.Book")
            .SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<BookStoreUsers> GetAll()
        {
            return entities.AsEnumerable();
        }

        public void Insert(BookStoreUsers entity)
        {

            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(BookStoreUsers entity)
        {

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }
    }
}
