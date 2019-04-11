using PizzaApp.DataAccess;
using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PizzaApp.Services
{
    public class UserService
    {
        private readonly string passwordFormat = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%_/-]).{6,20}$";
        private readonly string phoneNumberFormat = @"^(8|\+7)[\- ]*([0-9]{3})[\- ]*([0-9]{3})[\- ]*([0-9]{4})$";

        private int UserId { get; set; }

        private User User { get; set; }

        public bool Registrate(string fullName, string login, string password, string address, string phoneNumber)
        {
            if (!Regex.IsMatch(password, passwordFormat) || !Regex.IsMatch(phoneNumber, phoneNumberFormat))
            {
                return false;
            }

            User = new User
            {
                Login = login,
                Password = password,
                PhoneNumber = phoneNumber,
                Fullname = fullName,
                Address = address
            };


            return true;
        }

        public void SaveUser()
        {
            new UserAccessLayer().Insert(User);
        }

        public bool LogIn(string login, string password)
        {
            try
            {
                UserId = new UserAccessLayer().SelectId(login, password);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ChooseProduct(int ProductId, int countProduct)
        {
            Basket basket = new Basket
            {
                UserId = this.UserId,
                Date = DateTime.Now,
                ProductId = ProductId,
                Count = countProduct
            };
            new BasketAccessLayer().Insert(basket);   
        }

        public int PayProduct()
        {
            List<Basket> baskets = new BasketAccessLayer().SelectAll(UserId);

            foreach (var basket in baskets)
            {
                Order order = new Order
                {
                    UserId = basket.UserId,
                    Date = basket.Date,
                    ProductId = basket.ProductId,
                    Count = basket.Count
                };
                new OrderAccessLayer().Insert(order);
            }

            return new OrderAccessLayer().SelectPrice(UserId);
        }

        public List<Basket> GetBasket()
        {
            return new BasketAccessLayer().SelectAll(UserId);
        }

        public void DeleteInfo()
        {
            new BasketAccessLayer().Delete(UserId);
            new OrderAccessLayer().Delete(UserId);
        }
    }
}