namespace PizzaApp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();

            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("Регистрация - 1");
                System.Console.WriteLine("Вход - 2");
                System.Console.WriteLine("Выход - 3");

                string UserNumber = System.Console.ReadLine();

                if (UserNumber == "1")
                {
                    menu.ShowRegistrationMenu();
                }
                else if (UserNumber == "2")
                {
                    menu.ShowLogIn();
                    menu.ChooseProductMenuAsync();
                }
                else if (UserNumber == "3")
                {
                    break;
                }
                else
                {
                    System.Console.Clear();
                    System.Console.WriteLine("Такого варианта не существует!");
                    System.Console.WriteLine("Нажмите Enter чтобы продолжить!");
                    System.Console.ReadKey();
                }
            }
        }
    }
}
