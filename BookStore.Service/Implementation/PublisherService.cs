using BookStore.Domain.Domain;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Implementation
{
    public class PublisherService : IPublisherService
    {
        private readonly IRepository<Publisher> _publisherRepository;
        public PublisherService(IRepository<Publisher> publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }
        public void CreateNewPublisher(Publisher p)
        {
             _publisherRepository.Insert(p);
        }

        public void DeletePublisher(Guid id)
        {
            var publisher = _publisherRepository.Get(id);
             _publisherRepository.Delete(publisher);
        }

        public List<Publisher> GetAll()
        {
            return _publisherRepository.GetAll().ToList();
        }

        public Publisher GetDetails(Guid? id)
        {
            return _publisherRepository.Get(id);
        }

        public void UpdateExistingPublisher(Publisher p)
        {
            _publisherRepository.Update(p);
        }
    }
}
