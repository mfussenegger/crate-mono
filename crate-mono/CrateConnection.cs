using System;
using System.Data;
using System.Net.Http;

namespace Crate
{
	public class CrateConnection : IDbConnection
	{
		private string connectionString;
		private string schema = "http";
		private string hostname = "localhost";
		private int port = 4200;
		private ConnectionState state;

		public CrateConnection () : this("crate://localhost:4200") {}

		public CrateConnection (String connectionString)
		{
			this.connectionString = connectionString;
			this.state = ConnectionState.Closed;
		}

		public string sqlUri ()
		{
			return string.Format("{0}://{1}:{2}/_sql", schema, hostname, port);
		}

		#region IDbConnection implementation

		public IDbTransaction BeginTransaction ()
		{
			throw new NotImplementedException ();
		}

		public IDbTransaction BeginTransaction (IsolationLevel il)
		{
			throw new NotImplementedException ();
		}

		public void ChangeDatabase (string databaseName)
		{
			throw new NotImplementedException ();
		}

		public void Close ()
		{
			this.state = ConnectionState.Closed;
		}

		public IDbCommand CreateCommand ()
		{
			return new CrateCommand(null, this);
		}

		public void Open ()
		{
			this.state = ConnectionState.Connecting;
			using (var cmd = CreateCommand()) {
				cmd.CommandText = "select id from sys.cluster";
				var reader = cmd.ExecuteReader();
				reader.Read();
			}
			this.state = ConnectionState.Open;
		}

		public string ConnectionString {
			get {
				return connectionString;
			}
			set {
				connectionString = value;
			}
		}

		public int ConnectionTimeout {
			get {
				throw new NotImplementedException ();
			}
		}

		public string Database {
			get {
				throw new NotImplementedException ();
			}
		}

		public ConnectionState State {
			get {
				return state;
			}
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
			if (state != ConnectionState.Closed) {
				state = ConnectionState.Closed;
			}
		}

		#endregion
	}
}

