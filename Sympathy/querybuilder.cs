using System;
using System.Collections.Generic;

namespace Sympathy
{
	public abstract class QueryBuilder
	{
		public QueryBuilder (Table table, QueryType type = QueryType.Select)
		{
			_table = table;
			_type = type;
			_criteria = new Criteria ();
			_values =  new Criteria ();
		}
		
		public QueryType Type 
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}
		
		public Table Table 
		{
			get 
			{
				return _table;
			}
			set
			{
				_table = value;
			}
		}
		
		public IDictionary<string, object>Criteria
		{
			set
			{
				_criteria = (Criteria)value;
			}
		}
		
		public IDictionary<string, object>Values
		{
			set
			{
				_values = (Criteria)value;
			}
		}
		
		public override string ToString ()
		{
			switch (_type)
			{
			case QueryType.Select:
				return selectQuery ();
			case QueryType.Insert:
				return insertQuery (_values);
			case QueryType.Update:
				return updateQuery ();
			case QueryType.Delete:
				return deleteQuery ();
			default:
				return string.Empty;
			}
		}
			
		protected abstract string selectQuery ();
		protected abstract string insertQuery (IDictionary <string, object> values);
		protected abstract string updateQuery ();
		protected abstract string deleteQuery ();
		
		public enum QueryType 
		{
			Select,
			Insert,
			Update,
			Delete
		}
		
		protected Table _table;
		protected QueryType _type;
		protected Criteria _criteria;
		protected Criteria _values;
	}
}

