using System;
using System.Collections.Generic;

namespace Sympathy
{
	public abstract class QueryBuilder
	{
		public QueryBuilder (Table table, QueryType type = QueryType.Select)
		{
			Table = table;
			Type = type;
			_criteria = new Criteria ();
			_values =  new Criteria ();
		}
		
		public QueryType Type { get; set; }
		public Table Table { get; set; }
		
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
		
		public Dictionary <string, KeyValuePair<Operators, object>> parseKeys (Criteria criteria)
		{
			Dictionary<string, KeyValuePair<Operators, object>> condition = new Dictionary<string, KeyValuePair<Operators, object>> ();
			
			foreach (KeyValuePair<string, object> item in criteria) {
				string[] keys = item.Key.Split (new string[] { "__"}, StringSplitOptions.None);
				
				Operators oper = Operators.Equl;
				
				if (keys.Length > 1) {
					// for now we just consider simple query operators
					switch (keys[1]) {
					case "lt":
						oper = Operators.LessThan;
						break;
					case "gt":
						oper = Operators.GreaterThan;
						break;
					case "lte":
						oper = Operators.LessThanOrEqual;
						break;
					case "gte":
						oper = Operators.GreaterThanOrEqual;
						break;
					case "in":
						oper = Operators.In;
						break;
					default:
						break;
					}
				}
				
				// we have found nothing special ...
				condition [keys[0]] = new KeyValuePair<Operators, object> (oper, item.Value);
			}
			
			return condition;
		}
		
		public override string ToString ()
		{
			switch (Type)
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
		
		public enum Operators 
		{
			Equl,
			LessThan,
			GreaterThan,
			LessThanOrEqual,
			GreaterThanOrEqual,
			In,
			Like,
			Between,
			NotEqual
		}
		
		protected Dictionary<Operators, string> OperatorsString  = new Dictionary<Operators, string> () {
			{ Operators.Equl, "=" },
			{ Operators.LessThan, "<" },
			{ Operators.GreaterThan, ">" },
			{ Operators.In, "in" }
		};
		
		protected Criteria _criteria;
		protected Criteria _values;
	}
}

