using System;

namespace Crate
{
	public class SqlRequest
	{
		public string stmt { get; set; }
		public object[] args { get; set; }

		public SqlRequest () {}

		public SqlRequest(string statement, params object[] args) {
			this.stmt = statement;
			this.args = args;
		}
	}
}
