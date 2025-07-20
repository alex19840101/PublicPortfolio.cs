using System.Collections.Generic;
using System.Data;
using Dapper;

namespace LiteAuthService.DataAccess.Interfaces
{
    public interface IDapperSqlExecutor
    {
        /// <summary>
        /// Запрос к БД через Dapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"> SQL-запрос для выполнения </param>
        /// <param name="dynamicParameters"> "A bag of parameters that can be passed to the Dapper Query and Execute methods" </param>
        /// <param name="commandType"> тип команды (SQL-команда / название хранимой процедуры / название таблицы ) </param>
        /// <returns> IEnumerable(T) </returns>
        IEnumerable<T> Query<T>(
            string sql,
            DynamicParameters dynamicParameters,
            CommandType commandType = CommandType.Text);

        /// <summary>
        /// Запрос к БД через Dapper
        /// </summary>
        /// <param name="sql"> SQL-запрос для выполнения </param>
        /// <param name="dynamicParameters"> "A bag of parameters that can be passed to the Dapper Query and Execute methods" </param>
        /// <param name="commandType"> тип команды (SQL-команда / название хранимой процедуры / название таблицы ) </param>
        /// <returns> Количество затронутых строк </returns>
        int Execute(
            string sql,
            DynamicParameters dynamicParameters,
            CommandType commandType = CommandType.Text);
    }
}
