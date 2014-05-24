using System;

namespace Crate
{
	public class SqlResponse
	{
		public string[] cols { get; set; }
		public object[][] rows { get; set; }
		public int rowcount { get; set; }

		public SqlResponse ()
		{
		}
	}
}

