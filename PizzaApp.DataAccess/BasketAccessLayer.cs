using PizzaApp.DataAccess.Abstract;
using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace PizzaApp.DataAccess
{
    public class BasketAccessLayer : DataAccessLayer<Basket>
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

                    command.CommandText = "delete from Basket where b_user_id = @id";

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

        public override void Insert(Basket instance)
        {
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();

                    transaction = connection.BeginTransaction();

                    command.CommandText = "insert into Basket values(@userId, @productId, @date, @productCount)";

                    var userIdParameter = command.CreateParameter();
                    userIdParameter.ParameterName = "@userId";
                    userIdParameter.DbType = System.Data.DbType.Int32;
                    userIdParameter.Value = instance.UserId;

                    var productIdParameter = command.CreateParameter();
                    productIdParameter.ParameterName = "@productId";
                    productIdParameter.DbType = System.Data.DbType.Int32;
                    productIdParameter.Value = instance.ProductId;

                    var dateParameter = command.CreateParameter();
                    dateParameter.ParameterName = "@date";
                    dateParameter.DbType = System.Data.DbType.Date;
                    dateParameter.Value = instance.Date;

                    var productCountParameter = command.CreateParameter();
                    productCountParameter.ParameterName = "@productCount";
                    productCountParameter.DbType = System.Data.DbType.Int32;
                    productCountParameter.Value = instance.Count;

                    var parameters = new List<DbParameter>()
                    {
                        userIdParameter,
                        productIdParameter,
                        dateParameter,
                        productCountParameter
                    };

                    command.Parameters.AddRange(parameters.ToArray());

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

        public List<Basket> SelectAll(int userId)
        {
            var basketData = new List<Basket>();

            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();

                    command.CommandText = "select * from Basket where b_user_id = @id";

                    var idParameter = command.CreateParameter();
                    idParameter.ParameterName = "@id";
                    idParameter.DbType = System.Data.DbType.Int32;
                    idParameter.Value = userId;

                    command.Parameters.Add(idParameter);

                    var dbDataReader = command.ExecuteReader();

                    while (dbDataReader.Read())
                    {
                        int id = (int)dbDataReader["b_id"];
                        int productId = (int)dbDataReader["b_product_id"];
                        DateTime date = (DateTime)dbDataReader["b_date"];
                        int productCount = (int)dbDataReader["b_product_count"];

                        basketData.Add(new Basket
                        {
                            Id = id,
                            UserId = userId,
                            ProductId = productId,
                            Date = date,
                            Count = productCount
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

            return basketData;
        }
    }
}
