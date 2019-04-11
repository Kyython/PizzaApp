namespace PizzaApp.DataAccess.Abstract
{
    public interface IDataAccessLayer<T>
    {
        void Insert(T instance);

        void Delete(int id);
    }
}
