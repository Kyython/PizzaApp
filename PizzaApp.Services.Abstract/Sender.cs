using System;

namespace PizzaApp.Services.Abstract
{
    public abstract class Sender : ISender
    {
        public string Message { get; set; }

        public void CreateMessage()
        {
            const int MINIMUM_NUMBER = 999;
            const int MAXIMUM_NUMBER = 10000;
            Random random = new Random();
            Message = random.Next(MINIMUM_NUMBER, MAXIMUM_NUMBER).ToString();
        }

        public bool CheckMessage(string messageCode)
        {
            if (messageCode == Message)
            {
                return true;
            }

            return false;
        }
    }
}
