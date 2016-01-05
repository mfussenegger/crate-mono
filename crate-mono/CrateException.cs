using System;
using System.Data.Common;

namespace Crate.Client
{
	public class CrateException : DbException
	{
		public CrateException (String message) : base(message)
		{
		}
	}
}

