using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace LiteAuthService.DataAccess.Interfaces
{
    public interface IDapperAsyncExecutor
    {
        /// <summary>
        /// Асинхронный запрос к БД через Dapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"> SQL-запрос для выполнения </param>
        /// <param name="dynamicParameters"> "A bag of parameters that can be passed to the Dapper Query and Execute methods" </param>
        /// <param name="commandType"> тип команды (SQL-команда / название хранимой процедуры / название таблицы ) </param>
        /// <returns> Task(IEnumerable(T)) </returns>
        Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            DynamicParameters dynamicParameters,
            CommandType commandType = CommandType.Text);

        /// <summary>
        /// Асинхронный запрос к БД через Dapper
        /// </summary>
        /// <param name="sql"> SQL-запрос для выполнения </param>
        /// <param name="dynamicParameters"> "A bag of parameters that can be passed to the Dapper Query and Execute methods" </param>
        /// <param name="commandType"> тип команды (SQL-команда / название хранимой процедуры / название таблицы ) </param>
        /// <returns> Количество затронутых строк </returns>
        Task<int> ExecuteAsync(
            string sql,
            DynamicParameters dynamicParameters,
            CommandType commandType = CommandType.Text);
    }
}
