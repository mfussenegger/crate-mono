using System;
using System.Linq;
using System.Data;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Crate.Client
{
	[DebuggerDisplay("<CrateServer {Hostname}:{Port}>")]
	public class CrateServer
	{
		private readonly Regex serverRex = new Regex(@"^(https?)?(://)?([^:]*):?(\d*)$");

		public string Scheme { get; set; }
		public string Hostname { get; set; }
		public int Port { get; set; }

		public CrateServer () : this(null) {}

		public CrateServer(String server)
		{
            Hostname = "localhost";
            Scheme = "http";
            Port = 4200;
			if (server == null) {
				return;
			}

			var m = serverRex.Match(server);
			if (m.Success) {
				Scheme = string.IsNullOrEmpty(m.Groups[1].Value) ? "http" : m.Groups[1].Value;
				Hostname = string.IsNullOrEmpty(m.Groups[3].Value) ? "localhost" : m.Groups[3].Value;
				Port = int.Parse(string.IsNullOrEmpty(m.Groups[4].Value) ? "4200" : m.Groups[4].Value);
			}
		}

		public string sqlUri() {
			return string.Format("{0}://{1}:{2}/_sql", Scheme, Hostname, Port);
		}
	}

	public class CrateConnection : IDbConnection
	{
		private string connectionString;
		private ConnectionState state;
		private List<CrateServer> allServers;
		private int currentServer = 0;
		private object lockObj = new object();
		public List<CrateServer> activeServers { get; private set; }

		public CrateConnection () : this("localhost:4200") {}

		public CrateConnection (String connectionString)
		{
			allServers = new List<CrateServer>();
			foreach (var server in connectionString.Split (',')) {
				allServers.Add(new CrateServer(server.Trim()));
			}
			this.activeServers = allServers;
			this.connectionString = connectionString;
			this.state = ConnectionState.Closed;
		}

		public CrateServer nextServer() {
			lock (lockObj) {
				var server = activeServers[currentServer];
				currentServer++;
				if (currentServer >= activeServers.Count) {
					currentServer = 0;
				}
				return server;
			}
		}

		public void markAsFailed (CrateServer server)
		{
			lock (lockObj) {
				if (activeServers.Count == 1) {
					activeServers = allServers;
				}
				activeServers.Remove(server);
				Task.Delay(TimeSpan.FromMinutes(3)).ContinueWith(x => addServer(server));
				currentServer = 0;
			}
		}

		private void addServer (CrateServer server) {
			lock (lockObj) {
				if (!activeServers.Contains(server)) {
					activeServers.Add(server);
				}
			}
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

