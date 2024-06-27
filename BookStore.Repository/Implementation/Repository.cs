using BookStore.Domain.Domain;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;
        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            if(typeof(T) == typeof(Books))
            {
                return entities.Include("Publisher").Include("Author").AsEnumerable();
            }
            return entities.AsEnumerable();
        }

        public T Get(Guid? id)
        {
            if (typeof(T) == typeof(Books))
            {
                return entities.Include("Publisher").Include("Author").SingleOrDefault(s => s.Id == id);
            }
            if (typeof(T) == typeof(BooksInShoppingCart))
            {
                return entities.Include("Book").SingleOrDefault(s => s.Id == id);
            }

            return entities.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public T GetDetailsWithBaseEntity(BaseEntity id)
        {

            if (typeof(T) == typeof(Books))
            {
                return entities.Include("Publisher").Include("Author")
                .SingleOrDefaultAsync(z => z.Id == id.Id).Result;
            }
            if (typeof(T) == typeof(BooksInShoppingCart))
            {
                return entities.Include("Book")
                .SingleOrDefaultAsync(z => z.Id == id.Id).Result;
            }

            return entities
                .SingleOrDefaultAsync(z => z.Id == id.Id).Result;
        }
    }
}
