using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApp.Server
{
	public interface ISqlRunner
	{
		/// <summary>
		/// Run SQL query and return first result set converted into type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="rawSQL"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		Task<T> RunSqlQuery<T>(string rawSQL, SqlParameter[] parameters = null);

		/// <summary>
		/// Run SQL query and return all result sets returned by procedure.
		/// </summary>
		/// <param name="rawSql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		Task<IDictionary<string, object>[][]> RunSqlQuery(string rawSql, SqlParameter[] parameters = null);

		/// <summary>
		/// Run SQL procedure and return first result set converted into type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="rawSQL"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		Task<T> RunSqlProcedure<T>(string procedureName, SqlParameter[] parameters = null);

		/// <summary>
		/// Run SQL procedure and return all result sets returned by procedure.
		/// </summary>
		/// <param name="rawSql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		Task<IDictionary<string, object>[][]> RunSqlProcedure(string procedureName, SqlParameter[] parameters = null);

		string LastRunMessage { get; }
	}
}
