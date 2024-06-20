using BookStore.Domain.Domain;
using BookStore.Domain.DTO;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _cartRepository;
        private readonly IRepository<Books> _booksRepository;
        private readonly IUserRepository _userRepository;
        public ShoppingCartService(IRepository<ShoppingCart> cartRepository, IRepository<Books> booksRepository, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _booksRepository = booksRepository;
            _userRepository = userRepository;
        }

        public bool AddToShoppingConfirmed(BooksInShoppingCart model, string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser.ShoppingCart;

            if (userShoppingCart.BooksInShoppingCarts == null)
                userShoppingCart.BooksInShoppingCarts = new List<BooksInShoppingCart>(); ;

            userShoppingCart.BooksInShoppingCarts.Add(model);
            _cartRepository.Update(userShoppingCart);
            return true;
        }

        public bool deleteBookFromShoppingCart(string userId, Guid productId)
        {

            if (productId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;
                var product = userShoppingCart.BooksInShoppingCarts.Where(x => x.BookId == productId).FirstOrDefault();

                userShoppingCart.BooksInShoppingCarts.Remove(product);

                _cartRepository.Update(userShoppingCart);
                return true;
            }
            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser?.ShoppingCart;
            var allProduct = userShoppingCart?.BooksInShoppingCarts?.ToList();

            var totalPrice = allProduct.Select(x => (x.Book.Price * x.Quantity)).Sum();

            ShoppingCartDto dto = new ShoppingCartDto
            {
                Books = allProduct,
                TotalPrice = totalPrice
            };
            return dto;
        }

        public bool order(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
