using System;
using System.Data;
using System.Xml;
using System.Collections.Generic;

namespace Crate.Client
{
	public class CrateDataReader : IDataReader
	{
		private SqlResponse sqlResponse;
		private int currentRow = -1;
		private bool closed = false;
		private readonly DateTime UNIX_DT = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public CrateDataReader (SqlResponse sqlResponse)
		{
			this.sqlResponse = sqlResponse;
		}

		#region IDataReader implementation

		public void Close ()
		{
			closed = true;
		}

		public DataTable GetSchemaTable ()
		{
			throw new NotImplementedException ();
		}

		public bool NextResult ()
		{
			return sqlResponse.rows.Length > currentRow;
		}

		public bool Read ()
		{
			currentRow++;
			return NextResult();
		}

		public int Depth {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsClosed {
			get {
				return closed;
			}
		}

		public int RecordsAffected {
			get {
				return sqlResponse.rowcount;
			}
		}

		#endregion

		#region IDataRecord implementation

		public bool GetBoolean (int i)
		{
			return (bool)sqlResponse.rows[currentRow][i];
		}

		public byte GetByte (int i)
		{
			return (byte)sqlResponse.rows[currentRow][i];
		}

		public long GetBytes (int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException ();
		}

		public char GetChar (int i)
		{
			return (char)sqlResponse.rows[currentRow][i];
		}

		public long GetChars (int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException ();
		}

		public IDataReader GetData (int i)
		{
			throw new NotImplementedException ();
		}

		public string GetDataTypeName (int i)
		{
			throw new NotImplementedException ();
		}

		public DateTime GetDateTime (int i)
		{
			return UNIX_DT.AddMilliseconds(GetInt64(i));
		}

		public decimal GetDecimal (int i)
		{
			return (decimal)sqlResponse.rows[currentRow][i];
		}

		public double GetDouble (int i)
		{
			return (double)sqlResponse.rows[currentRow][i];
		}

		public Type GetFieldType (int i)
		{
			throw new NotImplementedException ();
		}

		public float GetFloat (int i)
		{
			return (float)sqlResponse.rows[currentRow][i];
		}

		public Guid GetGuid (int i)
		{
			return Guid.Parse((string)sqlResponse.rows[currentRow][i]);
		}

		public short GetInt16 (int i)
		{
			return (short)sqlResponse.rows[currentRow][i];
		}

		public int GetInt32 (int i)
		{
			return (int)sqlResponse.rows[currentRow][i];
		}

		public long GetInt64 (int i)
		{
			return (long)sqlResponse.rows[currentRow][i];
		}

		public string GetName (int i)
		{
			return sqlResponse.cols[i];
		}

		public int GetOrdinal (string name)
		{
			return Array.BinarySearch(sqlResponse.cols, name);
		}

		public string GetString (int i)
		{
			return (string)sqlResponse.rows[currentRow][i];
		}

		public object GetValue (int i)
		{
			return sqlResponse.rows[currentRow][i];
		}

		public int GetValues (object[] values)
		{
			int i = 0;
			int j = 0;
			for (; i < values.Length && j < sqlResponse.cols.Length; i++, j++) {
				values[i] = sqlResponse.rows[currentRow][j];
			}
			return i;
		}

		public bool IsDBNull (int i)
		{
			return sqlResponse.rows[currentRow][i] == null;
		}

		public int FieldCount {
			get {
				return sqlResponse.cols.Length;
			}
		}

		public object this [string name] {
			get {
				return sqlResponse.rows[currentRow][GetOrdinal(name)];
			}
		}

		public object this [int index] {
			get {
				return sqlResponse.rows[currentRow][index];
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

