using System;
using System.Collections;
using System.Collections.Generic;

namespace Sympathy
{
	public class Table : System.Collections.Generic.IEnumerator<Column>, IEnumerable
	{
		public Table (string tableName, Dictionary<string, Column> columns)
		{
			_tableName = tableName;
			_columns = columns;
			enumerator = columns.GetEnumerator ();
			index = -1;
		}
		
		public string Name
		{
			get
			{
				return _tableName;
			}
		}
		
		public Dictionary<string, Column> Columns 
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
				foreach (KeyValuePair<string, Column> col in _columns) {
					if (col.Value.ColumnType == Sympathy.Attributes.ColumnTypes.PrimaryKey)
						return col.Value;
				}
				
				return null;
			}
		}
		
		public Column this[string columnName]
		{
			get
			{
				if (_columns.ContainsKey (columnName))
					return _columns[columnName];
				
				
				foreach (KeyValuePair<string, Column> col in _columns) {
					if (col.Value.Name.ToLower () == Utils.genrateNameFromType (columnName)) {
						return col.Value;
					}
				}
				
				throw new ColumnDoesNotExist (columnName);
			}
		}
		
		public Column Current {
			get {
				return ((KeyValuePair<string, Column>) enumerator.Current).Value;
			}
		}
		
		object IEnumerator.Current {
			get {
				return Current;
			}
		}
		
		public bool MoveNext ()
		{
			if (enumerator.MoveNext ())
				return true;
		
			return false;
				
		}
		
		public void Reset ()
		{
			enumerator.Reset ();
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
		protected Dictionary<string, Column> _columns;
		protected int index;
		protected IDictionaryEnumerator enumerator;
	}
	
	public class ColumnDoesNotExist : Exception 
	{
		public ColumnDoesNotExist (string message) : base (message) {}
	}
}

