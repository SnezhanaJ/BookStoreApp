using BookStore.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Interface
{
    public interface IAuthorService
    {

        List<Author> GetAll();
        Author GetDetails(Guid? id);
        void CreateNewAuthor(Author p);
        void UpdateExistingAuthor(Author p);
        void DeleteAuthor(Guid id);
        Author GetDetailsWithBaseEntity(BaseEntity id);
        bool UpdateAuthor(Author p);
    }
}
