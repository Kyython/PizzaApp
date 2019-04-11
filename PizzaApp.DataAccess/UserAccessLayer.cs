using PizzaApp.DataAccess.Abstract;
using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace PizzaApp.DataAccess
{
    public class UserAccessLayer : DataAccessLayer<User>
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

                    command.CommandText = "delete from Users where u_id = @id";

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

        public override void Insert(User instance)
        {
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();

                    transaction = connection.BeginTransaction();

                    command.CommandText = "insert into Users values(@login, @password, @fullname, @address, @phonenumber)";
                    
                    var loginParameter = command.CreateParameter();
                    loginParameter.ParameterName = "@login";
                    loginParameter.DbType = System.Data.DbType.String;
                    loginParameter.Value = instance.Login;

                    var passwordParameter = command.CreateParameter();
                    passwordParameter.ParameterName = "@password";
                    passwordParameter.DbType = System.Data.DbType.String;
                    passwordParameter.Value = instance.Password;

                    var fullnameParameter = command.CreateParameter();
                    fullnameParameter.ParameterName = "@fullname";
                    fullnameParameter.DbType = System.Data.DbType.String;
                    fullnameParameter.Value = instance.Fullname;

                    var addressParameter = command.CreateParameter();
                    addressParameter.ParameterName = "@address";
                    addressParameter.DbType = System.Data.DbType.String;
                    addressParameter.Value = instance.Address;

                    var phoneNumberParameter = command.CreateParameter();
                    phoneNumberParameter.ParameterName = "@phonenumber";
                    phoneNumberParameter.DbType = System.Data.DbType.String;
                    phoneNumberParameter.Value = instance.PhoneNumber;

                    var parameters = new List<DbParameter>()
                    {
                        loginParameter,
                        passwordParameter,
                        fullnameParameter,
                        addressParameter,
                        phoneNumberParameter
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

        public int SelectId(string userLogin, string userPassword)
        {
            int userId;

            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();

                    command.CommandText = "select u_id from Users where u_login = @login COLLATE SQL_Latin1_General_CP1_CS_AS and u_password = @password COLLATE SQL_Latin1_General_CP1_CS_AS";

                    var loginParameter = command.CreateParameter();
                    loginParameter.ParameterName = "@login";
                    loginParameter.DbType = System.Data.DbType.String;
                    loginParameter.Value = userLogin;

                    var passwordParameter = command.CreateParameter();
                    passwordParameter.ParameterName = "@password";
                    passwordParameter.DbType = System.Data.DbType.String;
                    passwordParameter.Value = userPassword;

                    command.Parameters.Add(loginParameter);
                    command.Parameters.Add(passwordParameter);

                    userId = (int)command.ExecuteScalar();

                }
                catch (DbException)
                {
                    throw new Exception("Произошла ошибка на сервере! Повторите еще раз!");
                }
                catch (NullReferenceException)
                {
                    throw new Exception("Такого пользователя не существует!");
                }
                catch (Exception)
                {
                    throw new Exception("Ошибка! Повторите еще раз!");
                }
            }

            return userId;
        }
    }
}
