using System;
using System.Data.Common;
using System.Configuration;

namespace PizzaApp.DataAccess.Abstract
{
    public abstract class DataAccessLayer<T> : IDataAccessLayer<T>
    {
        public readonly string _connectionString;
        public readonly string _providerName;
        public readonly DbProviderFactory _providerFactory;

        public DbTransaction transaction;

        public DataAccessLayer()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["appConnectionString"].ConnectionString;
            _providerName = ConfigurationManager.ConnectionStrings["appConnectionString"].ProviderName;
            _providerFactory = DbProviderFactories.GetFactory(_providerName);
        }
       
        public abstract void Delete(int id);

        public abstract void Insert(T instance);
    }
}
