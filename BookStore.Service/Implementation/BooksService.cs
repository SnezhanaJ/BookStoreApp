using BookStore.Domain.Domain;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Implementation
{
    public class BooksService : IBooksService
    {
        private readonly IRepository<Books> _repository;
        public BooksService(IRepository<Books> repository)
        {
            _repository = repository;
        }

        public void CreateNewBook(Books p)
        {
            _repository.Insert(p);
        }

        public void DeleteBook(Guid id)
        {
           var book = _repository.Get(id);
           _repository.Delete(book);
        }

        public List<Books> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public Books GetDetails(Guid? id)
        {
            return _repository.Get(id);
        }

        public void UpdateExistingBook(Books p)
        {
            _repository.Update(p);
        }
    }
}
