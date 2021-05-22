using WebApp.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApp.Server.Database
{
	public class SqlRunner : ServiceBase, ISqlRunner
	{
		public SqlRunner(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			JsonConvert = GetService<IJsonConvert>();
		}

		public Task<T> RunSqlQuery<T>(string rawSQL, SqlParameter[] parameters = null)
		{
			return TryProcess("RunSqlQuery", async () =>
			{
				IDictionary<string, object>[][] rawResults = await RunSqlQuery(rawSQL, parameters).ConfigureAwait(false);
				return ConvertResultSet<T>(rawResults);
			});
		}

		public Task<IDictionary<string, object>[][]> RunSqlQuery(string rawSql, SqlParameter[] parameters = null)
		{
			return TryProcess("RunSqlQuery", async () =>
			{
				string[] batches = rawSql.Split(new[] { "\nGO\n" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string batch in batches)
				{
					LastResultSet = await ConnectAndRunCommand(CommandType.Text, batch, parameters).ConfigureAwait(false);
				}
				if (LastResultSet.Length == 0)
				{
					LastRunMessage = "Completed with no results returned.";
					// Nothing to return so return empty array
					return Array.Empty<IDictionary<string, object>[]>();
				}
				LastRunMessage = $"Completed with {TotalResultCount:N0} result sets.";
				return LastResultSet;
			});
		}

		/// <summary>
		/// Run SQL procedure and return first result set converted into type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="rawSQL"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public Task<T> RunSqlProcedure<T>(string procedureName, SqlParameter[] parameters = null)
		{
			return TryProcess($"RunSqlProcedure:{procedureName}", async () =>
			{
				IDictionary<string, object>[][] rawResults = await RunSqlProcedure(procedureName, parameters);
				return ConvertResultSet<T>(rawResults);
			});
		}

		/// <summary>
		/// Run SQL procedure and return all result sets returned by procedure.
		/// </summary>
		/// <param name="rawSql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public Task<IDictionary<string, object>[][]> RunSqlProcedure(string procedureName, SqlParameter[] parameters = null)
		{
			return TryProcess($"RunSqlProcedure:{procedureName}", async () =>
			{
				LastResultSet = await ConnectAndRunCommand(CommandType.StoredProcedure, procedureName, parameters);
				if (LastResultSet.Length == 0)
				{
					LastRunMessage = "No Results Found";
					// Nothing to return so return empty array
					return Array.Empty<IDictionary<string, object>[]>();
				}
				LastRunMessage = $"Completed with {TotalResultCount:N0} result sets.";
				return LastResultSet;
			});
		}

		/// <summary>
		/// Convert the response of a SQL resultset to the desired T object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resultSet"></param>
		/// <returns></returns>
		public T ConvertResultSet<T>(IDictionary<string, object>[][] resultSet)
		{
			// Need to convert from IDictionary[] or IDictionary[][] to Type T - 1st step is to serialize into JSON string
			string resultJSON;
			if (typeof(Array[]).IsAssignableFrom(typeof(T)))
			{
				if (resultSet.Length == 0 || resultSet[0].Length == 0) { return JsonConvert.Deserialize<T>("[]"); }
				resultJSON = JsonConvert.Serialize(resultSet);
			}
			else if (typeof(Array).IsAssignableFrom(typeof(T)))
			{
				if (resultSet.Length == 0 || resultSet[0].Length == 0) { return JsonConvert.Deserialize<T>("[]"); }
				resultJSON = JsonConvert.Serialize(resultSet[0]);
			}
			else
			{
				if (resultSet.Length == 0 || resultSet[0].Length == 0) { return default; }
				resultJSON = JsonConvert.Serialize(resultSet[0][0]);
			}
			return JsonConvert.Deserialize<T>(resultJSON);
		}

		private Task<IDictionary<string, object>[][]> ConnectAndRunCommand(CommandType commandType, string commandText, SqlParameter[] parameters)
		{
			using SqlConnection dataConnection = GetService<SqlConnection>();
			return TryProcess("ConnectAndRunCommand", async () =>
			{
				dataConnection.Open();
				using SqlCommand command = new(commandText, dataConnection)
				{
					CommandType = commandType,
					CommandTimeout = 10
				};
				if (parameters?.Length > 0)
				{
					command.Parameters.AddRange(parameters);
				}
				List<IDictionary<string, object>[]> results = new();
				using SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
				do
				{
					results.Add(GetResultSet(reader));
				}
				while (reader.NextResult());
				return results.ToArray();
			}, _ =>
			{
				dataConnection.Close();
			});
		}


		public string LastRunMessage { get; private set; }
		public IDictionary<string, object>[][] LastResultSet { get; private set; }
		public long TotalResultCount
		{
			get
			{
				if (LastResultSet == null || LastResultSet.Length == 0) { return 0; }
				long count = 0;
				foreach (object[] resultSet in LastResultSet)
				{
					count += resultSet.Length;
				}
				return count;
			}
		}

		private IDictionary<string, object>[] GetResultSet(SqlDataReader reader)
		{
			List<IDictionary<string, object>> resultSet = new List<IDictionary<string, object>>();
			while (reader.Read())
			{
				resultSet.Add(GetRow(reader));
			}
			return resultSet.ToArray();
		}

		private IDictionary<string, object> GetRow(SqlDataReader reader)
		{
			IDictionary<string, object> row = new Dictionary<string, object>();
			for (int index = 0; index < reader.FieldCount; ++index)
			{
				row.Add(reader.GetName(index), reader.GetValue(index));
			}
			return row;
		}

		private IJsonConvert JsonConvert { get; }
	}
}
