using BookStore.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Interface
{
    public interface IPublisherService
    {
        List<Publisher> GetAll();
        Publisher GetDetails(Guid? id);
        void CreateNewPublisher(Publisher p);
        void UpdateExistingPublisher(Publisher p);
        void DeletePublisher(Guid id);

    }
}
