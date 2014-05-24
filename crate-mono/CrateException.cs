using System;
using System.Data.Common;

namespace Crate
{
	public class CrateException : DbException
	{
		public CrateException (String message) : base(message)
		{
		}
	}
}

