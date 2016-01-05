using NUnit.Framework;
using System;

using Crate.Client;

namespace Crate.Client
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void testDefaultConnection ()
		{
			var server = new CrateServer();
			Assert.AreEqual("http", server.Scheme);
			Assert.AreEqual("localhost", server.Hostname);
			Assert.AreEqual(4200, server.Port);
		}

		[Test ()]
		public void testServerWithoutScheme ()
		{
			var server = new CrateServer("localhost:4200");
			Assert.AreEqual("http", server.Scheme);
			Assert.AreEqual("localhost", server.Hostname);
			Assert.AreEqual(4200, server.Port);
		}

		[Test ()]
		public void testServerWithoutPort ()
		{
			var server = new CrateServer("localhost");
			Assert.AreEqual("http", server.Scheme);
			Assert.AreEqual("localhost", server.Hostname);
			Assert.AreEqual(4200, server.Port);
		}



		[Test ()]
		public void testGetDateTime()
		{
			var reader = new CrateDataReader(new SqlResponse() { 
				rows = new object[][] { new object[] { 1388534400000 } },
				cols = new string[] { "dt" }
			});
			reader.Read();
			var dt = new DateTime(2014, 01, 01);
			Assert.AreEqual(dt, reader.GetDateTime(0));
		}
	}
}

