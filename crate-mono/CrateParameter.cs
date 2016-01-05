using System;
using System.Data;

namespace Crate.Client
{
	public class CrateParameter : IDbDataParameter
	{
		public CrateParameter () {
			Direction = ParameterDirection.Input;
			SourceVersion = DataRowVersion.Current;
		}

		public CrateParameter (string parameterName, DbType type)
		{
			ParameterName = parameterName;
			DbType = type;
			Direction = ParameterDirection.Input;
			SourceVersion = DataRowVersion.Current;
		}

		public CrateParameter (string parameterName, object value)
		{
			ParameterName = parameterName;
			Direction = ParameterDirection.Input;
			SourceVersion = DataRowVersion.Current;
			DbType = inferType(value);
			Value = value;
		}

		public byte Precision { get; set; }
		public byte Scale { get; set; }

		public int Size { get; set; }

		#region IDataParameter implementation

		public DbType DbType { get; set; }
		public string ParameterName { get; set; }
		public string SourceColumn { get; set; }
		public DataRowVersion SourceVersion { get; set; }
		public object Value { get; set; }

		public ParameterDirection Direction { get; set; }

		public bool IsNullable {
			get {
				return true;
			}
		}

		#endregion

		private DbType inferType(object value) {
			switch (Type.GetTypeCode (value.GetType())) {
			case TypeCode.Empty:
				throw new ArgumentException();
			case TypeCode.Object:
			case TypeCode.DBNull:
				return DbType.Object;
			case TypeCode.Boolean:
				return DbType.Boolean;
			case TypeCode.Char:
				return DbType.String;
			case TypeCode.SByte:
				return DbType.Byte;
			case TypeCode.Byte:
				return DbType.Byte;
			case TypeCode.Int16:
				return DbType.Int16;
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
				throw new ArgumentException();
			case TypeCode.Int32:
				return DbType.Int32;
			case TypeCode.Int64:
				return DbType.Int64;
			case TypeCode.Single:
				throw new ArgumentException();
			case TypeCode.Double:
				return DbType.Double;
			case TypeCode.Decimal:
				return DbType.Decimal;
			case TypeCode.DateTime:
				return DbType.DateTime;
			case TypeCode.String:
				return DbType.String;
			default:
				throw new ArgumentOutOfRangeException ();
			}
		}
	}
}

