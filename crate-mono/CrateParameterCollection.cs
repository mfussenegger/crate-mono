using System;
using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace Crate
{
	public class CrateParameterCollection : List<CrateParameter>, IDataParameterCollection
	{
		public CrateParameterCollection ()
		{
		}

		public void RemoveAt (string parameterName)
		{
			this.RemoveAt(IndexOf(parameterName));
		}

		public int IndexOf (string parameterName)
		{
			return this.FindIndex(x => x.ParameterName == parameterName); 
		}

		public bool Contains (string parameterName)
		{
			return this.Any(x => x.ParameterName == parameterName);
		}

		public object this [string parameterName]
		{
			get {
				return this.FirstOrDefault(x => x.ParameterName == parameterName);
			}
			set {
				this[IndexOf(parameterName)] = (CrateParameter)value;
			}
		}
	}
}
