using BookStore.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<BookStoreUsers> GetAll();
        BookStoreUsers Get(string? id);
        void Insert(BookStoreUsers entity);
        void Update(BookStoreUsers entity);
        void Delete(BookStoreUsers entity);
    }
}
