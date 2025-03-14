using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LiteAuthService.DataAccess.Interfaces;
using Microsoft.Data.SqlClient;

namespace LiteAuthService.DataAccess.Implementation
{
    public class DapperSqlExecutor : IDapperSqlExecutor, IDapperAsyncExecutor
    {
        private readonly string _connectionString;
        
        public DapperSqlExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection() => new(_connectionString);

        /// <summary>
        /// Запрос к БД через Dapper (с транзакцией)
        /// </summary>
        /// <param name="sql"> SQL-запрос для выполнения </param>
        /// <param name="dynamicParameters"> "A bag of parameters that can be passed to the Dapper Query and Execute methods" </param>
        /// <param name="commandType"> тип команды (SQL-команда / название хранимой процедуры / название таблицы ) </param>
        /// <returns> Количество затронутых строк </returns>
        public int Execute(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            int affectedRowsCount;

            using IDbConnection connection = GetConnection();

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            try
            {
                using var transaction = connection.BeginTransaction();

                try
                {
                    affectedRowsCount = connection.Execute(sql, dp, transaction);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return affectedRowsCount;
        }

        /// <summary>
        /// Запрос к БД через Dapper (с транзакцией)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"> SQL-запрос для выполнения </param>
        /// <param name="dynamicParameters"> "A bag of parameters that can be passed to the Dapper Query and Execute methods" </param>
        /// <param name="commandType"> тип команды (SQL-команда / название хранимой процедуры / название таблицы ) </param>
        /// <returns> IEnumerable(T) </returns>
        public IEnumerable<T> Query<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            IEnumerable<T> result;

            using IDbConnection db = GetConnection();

            if (db.State == ConnectionState.Closed)
                db.Open();

            try
            {
                using var transaction = db.BeginTransaction();

                try
                {
                    result = db.Query<T>(sql, dp, commandType: commandType, transaction: transaction);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }


        /// <summary>
        /// Асинхронный запрос к БД через Dapper (с транзакцией)
        /// </summary>
        /// <param name="sql"> SQL-запрос для выполнения </param>
        /// <param name="dynamicParameters"> "A bag of parameters that can be passed to the Dapper Query and Execute methods" </param>
        /// <param name="commandType"> тип команды (SQL-команда / название хранимой процедуры / название таблицы ) </param>
        /// <returns> Количество затронутых строк </returns>
        public async Task<int> ExecuteAsync(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            int affectedRowsCount;

            using IDbConnection connection = GetConnection();

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            try
            {
                using var transaction = connection.BeginTransaction();

                try
                {
                    affectedRowsCount = await connection.ExecuteAsync(sql, dp, transaction);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return affectedRowsCount;
        }

        /// <summary>
        /// Асинхронный запрос к БД через Dapper (с транзакцией)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"> SQL-запрос для выполнения </param>
        /// <param name="dynamicParameters"> "A bag of parameters that can be passed to the Dapper Query and Execute methods" </param>
        /// <param name="commandType"> тип команды (SQL-команда / название хранимой процедуры / название таблицы ) </param>
        /// <returns> Task(IEnumerable(T)) </returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            IEnumerable<T> result;

            using IDbConnection db = GetConnection();

            if (db.State == ConnectionState.Closed)
                db.Open();

            try
            {
                using var transaction = db.BeginTransaction();

                try
                {
                    result = await db.QueryAsync<T>(sql, dp, commandType: commandType, transaction: transaction);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
    }
}
