using PizzaApp.DataAccess.Abstract;
using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace PizzaApp.DataAccess
{
    public class ProductAccessLayer : DataAccessLayer<Product>
    {
        public override void Delete(int id)
        {
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();

                    transaction = connection.BeginTransaction();

                    command.CommandText = "delete from Products where p_id = @id";

                    var idParameter = command.CreateParameter();
                    idParameter.ParameterName = "@id";
                    idParameter.DbType = System.Data.DbType.Int32;
                    idParameter.Value = id;

                    command.Parameters.Add(idParameter);

                    command.Transaction = transaction;

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows < 1)
                    {
                        throw new Exception("Процесс удаления завершился ошибкой");
                    }

                    transaction.Commit();
                }
                catch (DbException)
                {
                    transaction?.Rollback();
                    throw new Exception("Произошла ошибка на сервере! Повторите еще раз!");
                }
                catch (Exception)
                {
                    transaction?.Rollback();
                    throw new Exception("Ошибка! Повторите еще раз!");
                }
            }
        }

        public override void Insert(Product instance)
        {
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();

                    transaction = connection.BeginTransaction();

                    command.CommandText = "insert into Products values(@name, @price)";
                
                    var nameParameter = command.CreateParameter();
                    nameParameter.ParameterName = "@name";
                    nameParameter.DbType = System.Data.DbType.String;
                    nameParameter.Value = instance.Name;

                    var priceParameter = command.CreateParameter();
                    priceParameter.ParameterName = "@price";
                    priceParameter.DbType = System.Data.DbType.Int32;
                    priceParameter.Value = instance.Price;

                    command.Parameters.Add(nameParameter);
                    command.Parameters.Add(priceParameter);

                    command.Transaction = transaction;

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows < 1)
                    {
                        throw new Exception("Процесс вставки завершился ошибкой");
                    }

                    transaction.Commit();
                }
                catch (DbException)
                {
                    transaction?.Rollback();
                    throw new Exception("Произошла ошибка на сервере! Повторите еще раз!");
                }
                catch (Exception)
                {
                    transaction?.Rollback();
                    throw new Exception("Ошибка! Повторите еще раз!");
                }
            }
        }

        public List<Product> SelectAll()
        {
            var productData = new List<Product>();

            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    command.CommandText = "select * from Products";

                    var dbDataReader = command.ExecuteReader();

                    while (dbDataReader.Read())
                    {
                        int id = (int)dbDataReader["p_id"];
                        string name = dbDataReader["p_name"].ToString();
                        int price = (int)dbDataReader["p_price"];

                        productData.Add(new Product
                        {
                            Id = id,
                            Name = name,
                            Price = price
                        });
                    }
                    dbDataReader.Close();
                }
                catch (DbException)
                {
                    throw new Exception("Произошла ошибка на сервере! Повторите еще раз!");
                }
                catch (Exception)
                {
                    throw new Exception("Ошибка! Повторите еще раз!");
                }
            }

            return productData;
        }
    }
}
