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
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _repository;
        
        public AuthorService(IRepository<Author> repository)
        {
            _repository = repository;
        }
        public void CreateNewAuthor(Author p)
        {
            _repository.Insert(p);
        }

        public void DeleteAuthor(Guid id)
        {
            var author = _repository.Get(id);   
            _repository.Delete(author);
        }

        public List<Author> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public Author GetDetails(Guid? id)
        {
            return _repository.Get(id);
        }

        public Author GetDetailsWithBaseEntity(BaseEntity id)
        {
            return _repository.GetDetailsWithBaseEntity(id);
        }

        public bool UpdateAuthor(Author p)
        {
            try
            {
                _repository.Update(p);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void UpdateExistingAuthor(Author p)
        {
            _repository.Update(p);
        }
    }
}
