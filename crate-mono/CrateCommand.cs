using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace Crate
{
	public class CrateCommand : IDbCommand
	{
		private CrateConnection connection;
		private CrateParameterCollection parameters = new CrateParameterCollection();

		public string CommandText { get; set; }
		public int CommandTimeout { get; set; }

		public IDbConnection Connection {
			get {
				return connection;
			}
			set {
				throw new InvalidOperationException();
			}
		}

		public CrateCommand (string commandText, CrateConnection connection)
		{
			CommandText = commandText;
			this.connection = connection;
		}

		#region IDbCommand implementation

		public void Cancel ()
		{
			throw new NotImplementedException ();
		}

		public IDbDataParameter CreateParameter ()
		{
			return new CrateParameter();
		}

		public int ExecuteNonQuery() 
		{
			var task = ExecuteNonQueryAsync();
			task.Wait();
			return task.Result;
		}

		public async Task<int> ExecuteNonQueryAsync ()
		{
			return (await execute()).rowcount;
		}

		public IDataReader ExecuteReader()
		{
			var task = ExecuteReaderAsync();
			task.Wait();
			return task.Result;
		}

		protected async Task<SqlResponse> execute(int currentRetry = 0)
		{
			using (var client = new HttpClient()) {
				var server = connection.nextServer();
				try {
					var resp = await client.PostAsJsonAsync(
						server.sqlUri(),
						new SqlRequest(CommandText, parameters.Select(x => x.Value).ToArray())
					);
					if (!resp.IsSuccessStatusCode) {
						throw new CrateException(resp.ReasonPhrase + " " + resp.Content.ReadAsStringAsync().Result);
					}
					return await resp.Content.ReadAsAsync<SqlResponse>();
				} catch (WebException) {
					connection.markAsFailed(server);
					if (currentRetry > 3) {
						throw ;
					}
				}
				return await execute(currentRetry++);
			}
		}


		public async Task<IDataReader> ExecuteReaderAsync ()
		{
			return new CrateDataReader(await execute());
		}

		public IDataReader ExecuteReader (CommandBehavior behavior)
		{
			return ExecuteReader();
		}

		public object ExecuteScalar ()
		{
			using (var reader = ExecuteReader()) {
				reader.Read();
				return reader[0];
			}
		}

		public void Prepare ()
		{
		}


		public CommandType CommandType { get; set; }

		public IDataParameterCollection Parameters {
			get {
				return parameters;
			}
		}

		public IDbTransaction Transaction {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public UpdateRowSource UpdatedRowSource {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
		}

		#endregion
	}
}

