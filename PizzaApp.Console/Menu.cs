using PizzaApp.DataAccess;
using PizzaApp.Services;
using PizzaApp.Services.Abstract;
using PizzaApp.Services.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace PizzaApp.Console
{

    public class Menu : UserService
    {
        private static readonly string token = "638892701:AAFuc9Da_o0-JQW3W1hc7J2sli-i6PAW_gI";

        private readonly TelegramBotClient telegramBot = new TelegramBotClient(token);

        private void SendSms()
        {
            var botName = telegramBot.GetMeAsync().Result;
            telegramBot.OnMessage += MessageReceived;
            telegramBot.OnMessageEdited += MessageReceived;

            telegramBot.StartReceiving(Array.Empty<UpdateType>());
            System.Console.WriteLine($"Телеграм бот -  {botName.Username}\n");
            System.Console.WriteLine("Нажмите Enter, после того как получите код у телеграмм бота");
            System.Console.ReadLine();
            telegramBot.StopReceiving();
        }

        private async void MessageReceived(object sender, MessageEventArgs e)
        {
            Message = new Random().Next(999, 10000).ToString();

            switch (e.Message.Text)
            {
                case "/getcode":
                    await telegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Код подтверждения: " + Message);
                    break;
                case "/start":
                    await telegramBot.SendTextMessageAsync(e.Message.Chat.Id, @"/getcode-получение кода подтверждения для регистрации");
                    break;
            }
        }

        private string Message { get; set; }

        private string PhoneNumber { get; set; }

        private string Login { get; set; }

        private string Password { get; set; }

        public void ShowRegistrationMenu()
        {
            System.Console.Clear();
            bool check = false;

            while (!check)
            {
                System.Console.Write("Введите Ф.И.О - ");
                string fullName = System.Console.ReadLine();

                System.Console.Write("Введите ваш адрес проживания - ");
                string address = System.Console.ReadLine();

                System.Console.Write("Введите ваш номер телефона - ");
                PhoneNumber = System.Console.ReadLine();

                System.Console.Write("Введите логин (максимум симвлов - 20) - ");
                Login = System.Console.ReadLine();

                System.Console.WriteLine("Требования к паролю - ");
                System.Console.WriteLine("Максимум символов - 16");
                System.Console.WriteLine("Минимум символов - 6");
                System.Console.WriteLine("Минимум одна строчная и заглавная буква");
                System.Console.WriteLine("Минимум одна цифра и символ(#, $ и. т. д.)");
                System.Console.Write("Введите пароль - ");

                Password = System.Console.ReadLine();
                System.Console.Clear();

                check = Registrate(fullName, Login, Password, address, PhoneNumber);
            }

            System.Console.WriteLine("Выберите способ подтверждения личности - ");
            System.Console.WriteLine("SMS на номер телефона - 1");
            System.Console.WriteLine("Телеграм бот - 2");
            string userNumber = System.Console.ReadLine();
            

            while (true)
            {
                if (userNumber == "1")
                {
                    try
                    {
                        SmsSender smsSender = new SmsSender();
                        smsSender.SendSms(PhoneNumber);

                        System.Console.WriteLine($"Вам на номер телефона {PhoneNumber} придет смс с 4-х значным кодом");
                        System.Console.Write("Введите код - ");

                        string messageCode = System.Console.ReadLine();

                        while (!smsSender.CheckMessage(messageCode))
                        {
                            System.Console.WriteLine("Ошибка, код не верный!");
                            System.Console.WriteLine("Введите код -");
                            messageCode = System.Console.ReadLine();
                        }

                        break;
                    }
                    catch (Twilio.Exceptions.ApiException)
                    {
                        System.Console.WriteLine("Регистрация прервана!");
                        System.Console.WriteLine("Ошибка отправки смс!");
                        System.Console.WriteLine("Нажмите Enter чтобы продолжить!");
                        System.Console.ReadKey();
                        break;
                    }
                }
                else if (userNumber == "2")
                {
                    try
                    {
                        SendSms();
                        
                        
                        System.Console.Write("Введите код - ");
                        string messageCode = System.Console.ReadLine();

                        while (messageCode != Message)
                        {
                            System.Console.WriteLine("Ошибка, код не верный!");
                            System.Console.WriteLine("Введите код -");
                            messageCode = System.Console.ReadLine();
                        }
                        break;
                    }
                    catch (Exception)
                    {
                        System.Console.WriteLine("Error");
                        break;
                    }
                }
                else
                {
                    System.Console.WriteLine("Ошибка ввода!");
                    System.Console.WriteLine("Повторите ввод!");
                }
            }


            SaveUser();
            System.Console.WriteLine("Регистрация прошла успешно!");
            System.Console.WriteLine("Нажмите Enter чтобы продолжить!");
            System.Console.ReadKey();
        }

        public void ShowLogIn()
        {
            System.Console.Clear();
            bool check = false;

            while (!check)
            {
                System.Console.Clear();
                System.Console.Write("Введите логин - ");
                Login = System.Console.ReadLine();

                System.Console.Write("Введите пароль - ");
                Password = System.Console.ReadLine();

                check = LogIn(Login, Password);
            }

            System.Console.WriteLine("Вход выполнен!");
            System.Console.WriteLine("Нажмите Enter чтобы продолжить!");
            System.Console.ReadKey();
        }

        public void ChooseProductMenuAsync()
        {
            System.Console.Clear();
            ProductAccessLayer productAccessLayer = new ProductAccessLayer();
            var products = productAccessLayer.SelectAll();
            bool check = true;
            int productId;
            int countProduct;

            while (check)
            {
                System.Console.Clear();

                foreach (var product in products)
                {
                    System.Console.WriteLine($"Номер    Наименование    Цена");
                    System.Console.WriteLine($"{product.Id}        {product.Name}       {product.Price} тг\n");
                }

                try
                {
                    System.Console.Write("Введите номер пиццы - ");
                    productId = int.Parse(System.Console.ReadLine());
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Ошибка ввода!");
                    System.Console.WriteLine("Нажмите Enter чтобы продолжить!");
                    System.Console.ReadKey();
                    break;
                }

                try
                {
                    System.Console.Write("Введите кол-во продукта - ");
                    countProduct = int.Parse(System.Console.ReadLine());
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Ошибка ввода!");
                    System.Console.WriteLine("Нажмите Enter чтобы продолжить!");
                    System.Console.ReadKey();
                    break;
                }

                try
                {
                    ChooseProduct(productId, countProduct);
                    System.Console.Clear();
                    System.Console.WriteLine("Хотите заказать что-нибудь еще?");
                    System.Console.WriteLine(" 1 - Да");
                    System.Console.WriteLine(" 2 - Нет");

                    while (true)
                    {
                        string userNumber = System.Console.ReadLine();
                        if (userNumber == "1")
                        {
                            check = true;
                            break;
                        }
                        else if (userNumber == "2")
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            System.Console.WriteLine("Ошибка ввода!");
                            System.Console.WriteLine("Повторите ввод!");
                        }
                    }
                }
                catch (Exception)
                {
                    System.Console.Clear();
                    System.Console.WriteLine("Нажмите Enter чтобы продолжить!");
                    System.Console.ReadKey();
                    break;
                }
            }

            try
            {
                ShowPayParoductAsync();
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception.Message);
                System.Console.WriteLine("Нажмите Enter чтобы продолжить!");
                System.Console.ReadKey();
            }
        }

        public async void ShowPayParoductAsync()
        {
            var baskets = GetBasket();
            ProductAccessLayer productAccessLayer = new ProductAccessLayer();
            var products = productAccessLayer.SelectAll();

            System.Console.Clear();
            System.Console.WriteLine("Ваш заказ: ");

            foreach (var basket in baskets)
            {
                foreach (var product in products)
                {
                    if (basket.ProductId == product.Id)
                    {
                        System.Console.WriteLine("Наименование    Цена  Количество");
                        System.Console.WriteLine($"{product.Name}       {product.Price}тг     {basket.Count}\n");
                    }
                }
            }

            int orderPrice = PayProduct();

            System.Console.WriteLine($"Общая сумма вашего заказа - {orderPrice} тг");
            System.Console.WriteLine("Нажмите 1 если вы согласны с заказом");
            System.Console.WriteLine("Нажмите 2 если вы отказываетсь от заказа");

            while (true)
            {
                string userNumber = System.Console.ReadLine();
                if (userNumber == "1")
                {
                    try
                    {
                        var result = await new PaymentSystem().PayPalPaymentAsync(orderPrice);
                        if (result == "Created")
                        {
                            System.Console.WriteLine("Уведомление - PayPal Платеж создан!");
                        }
                    }
                    catch (Exception)
                    {
                        System.Console.WriteLine("Уведомление -  ошибка создания PayPal платежа");
                    }
                }
                else if (userNumber == "2")
                {
                    System.Console.WriteLine("Очень жаль!");
                    System.Console.WriteLine("Нажмите Enter чтобы продолжить!");
                    System.Console.ReadKey();
                }
                else
                {
                    System.Console.WriteLine("Ошибка ввода!");
                    System.Console.WriteLine("Повторите ввод!");
                }
                break;
            }
       
            DeleteInfo();
        }
    }
}
