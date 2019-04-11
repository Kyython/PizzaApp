namespace PizzaApp.Services.Abstract
{
    public interface ISender
    {
        void CreateMessage();

        bool CheckMessage(string messageCode);
    }
}
