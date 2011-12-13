using System;
using System.Collections;

namespace Sympathy
{
	public class Table : System.Collections.Generic.IEnumerator<Column>, IEnumerable
	{
		public Table (string tableName, Column[] columns)
		{
			_tableName = tableName;
			_columns = columns;
			index = -1;
		}
		
		public string Name
		{
			get
			{
				return _tableName;
			}
		}
		
		public Column[] Columns 
		{
			get
			{
				return this._columns;
			}
		}
		
		public Column PrimaryKey 
		{
			get 
			{
				foreach (Column col in _columns) {
					if (col.ColumnType == Sympathy.Attributes.ColumnTypes.PrimaryKey)
						return col;
				}
				
				return null;
			}
		}
		
		public Column this[int index]
		{
			get
			{
				if (index < _columns.Length && index > -1)
					return _columns[index];
				
				throw new IndexOutOfRangeException ();
			}
		}
		
		public Column this[string columnName]
		{
			get
			{
				foreach (Column col in _columns) {
					if (col.Name.ToLower () == columnName.ToLower ()) {
						return col;
					}
				}
				
				throw new ColumnDoesNotExist ();
			}
		}
		
		public Column Current {
			get {
				return this[index];
			}
		}
		
		object IEnumerator.Current {
			get {
				return Current;
			}
		}
		
		public bool MoveNext ()
		{
			if (index < _columns.Length - 1) {
				++index;
				return true;
			}
			
			return false;
				
		}
		
		public void Reset ()
		{
			index = -1;
		}
		
		public void Dispose ()
		{
		}
		
		public IEnumerator GetEnumerator ()
		{
			index = -1;
			return this;
		}
		
		protected string _tableName;
		protected Column[] _columns;
		protected int index;
	}
	
	public class ColumnDoesNotExist : Exception {}
}

