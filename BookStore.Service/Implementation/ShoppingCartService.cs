using BookStore.Domain;
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
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<BooksInShoppingCart> _bookInShoppingCartRepository;
        private readonly IRepository<BookInOrder> _bookInOrderRepository;
        private readonly IEmailService _emailService;
        public ShoppingCartService(IRepository<ShoppingCart> cartRepository, IRepository<Books> booksRepository, IUserRepository userRepository, IRepository<BookInOrder> bookInOrderRepository, IEmailService emailService)
        {
            _cartRepository = cartRepository;
            _booksRepository = booksRepository;
            _userRepository = userRepository;
            _bookInOrderRepository = bookInOrderRepository;
            _emailService = emailService;
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
            if (userId == null)
            {
                Console.WriteLine("userId is null");
                return false;
            }

            var loggedInUser = _userRepository.Get(userId);
            if (loggedInUser == null)
            {
                Console.WriteLine("loggedInUser is null");
                return false;
            }

            var userShoppingCart = loggedInUser?.ShoppingCart;
            if (userShoppingCart == null)
            {
                Console.WriteLine("userShoppingCart is null");
                return false;
            }

            if (userShoppingCart.BooksInShoppingCarts == null)
            {
                Console.WriteLine("BooksInShoppingCarts is null");
                return false;
            }

            EmailMessage message = new EmailMessage();
            message.Subject = "Successful order";
            message.MailTo = loggedInUser.Email;

            Order order = new Order
            {
                Id = Guid.NewGuid(),
                userId = userId,
                User = loggedInUser
            };

            List<BookInOrder> booksInOrder = new List<BookInOrder>();

            var rez = userShoppingCart.BooksInShoppingCarts.Select(
                z => new BookInOrder
                {
                    Id = Guid.NewGuid(),
                    BookId = z.Book.Id,
                    Book = z.Book,
                    OrderId = order.Id,
                    Order = order,
                    Quantity = z.Quantity
                }).ToList();

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            sb.AppendLine("Your order is completed. The order contains: ");

            for (int i = 1; i <= rez.Count; i++)
            {
                var currentItem = rez[i - 1];
                totalPrice += currentItem.Quantity * currentItem.Book.Price;
                sb.AppendLine(i.ToString() + ". " + currentItem.Book.Title + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.Book.Price);
            }

            sb.AppendLine("Total price for your order: " + totalPrice.ToString());
            message.Content = sb.ToString();

            booksInOrder.AddRange(rez);

            if (_bookInOrderRepository == null)
            {
                Console.WriteLine("_bookInOrderRepository is null");
                return false;
            }

            foreach (var product in booksInOrder)
            {
                _bookInOrderRepository.Insert(product);
            }

            loggedInUser.ShoppingCart.BooksInShoppingCarts.Clear();
            _userRepository.Update(loggedInUser);
            //_cartRepository.Update(userShoppingCart); 
            this._emailService.SendEmailAsync(message);

            return true;
        }


    }
}

