using System;
using System.Collections.Generic;

namespace Sympathy
{
	public class MSSqlQueryBuilder : QueryBuilder
	{
		public MSSqlQueryBuilder (Table table = null, QueryType type = QueryType.Select) : base (table, type)
		{
		}
		
		protected override string selectQuery ()
		{
			string query = "SELECT {0} FROM {1}";
			
			List<string> cols = new List<string> ();
			foreach (Column column in Table) 
			{
				cols.Add (column.Name);
			}
			
			query = string.Format (query, string.Join (", ", cols), Table.Name);
			
			List<string> where = new List<string> ();
			Dictionary <string, KeyValuePair<Operators, object>> criteria = parseKeys (_criteria);
			
			foreach (KeyValuePair<string, KeyValuePair<Operators, object>> item in criteria)
			{
				if (Table[item.Key] != null) {
					string _operator = OperatorsString [item.Value.Key];
					object value = item.Value.Value;
					
					if (Table[item.Key] != null) {
						if (typeof (iModel).IsAssignableFrom (value.GetType ())) {
							Reflector reflector = new Reflector ((iModel)value);
							Table table = reflector.getTable ();
							
							value = table.PrimaryKey.getValue ((iModel)value);
						}
						
						if (item.Value.Value.GetType ().Equals (typeof (int)) ||
							item.Value.Value.GetType ().Equals (typeof (long)))
							where.Add (string.Format ("{0} {1} {2}", item.Key, _operator, value));
						else
							where.Add (string.Format ("{0} {1} '{2}'", item.Key, _operator, value));
					}
				}
			}
			
			if (where.Count > 0) {
				query += string.Format (" WHERE {0}", string.Join (" AND ", where));
			}
			
			return query;
		}
		
		protected override string deleteQuery ()
		{
			string query = selectQuery ();
			
			query = string.Format ("DELETE {0}", query.Substring (query.IndexOf ("FROM")));
			return query;
		}
		
		protected override string insertQuery (System.Collections.Generic.IDictionary<string, object> values)
		{
			string query = "INSERT INTO {0} ({1}) VALUES ({2}); SELECT CAST(Scope_Identity() AS INT);";
			
			List<string> cols = new List<string> ();
			List<string> vals = new List<string> ();
			
			
			foreach (KeyValuePair<string, object> item in values) {
				if (item.Value != null && Table[item.Key] != null) {
					cols.Add (item.Key.ToLower ());
					
					//if (typeof (iModel).IsAssignableFrom (Table[item.Key].Type)) {
				//		Reflector reflector = new Reflector ((iModel)item.Value);
					//	Table table = reflector.getTable ();
						
				//		vals.Add (string.Format ("'{0}'", table.PrimaryKey.getValue ((iModel)item.Value)));
				//	} else {
						vals.Add (string.Format ("'{0}'", item.Value.ToString ()));
				//	}
				}
			}
			
			return string.Format (query, Table.Name, string.Join (", ", cols), string.Join (", ", vals));
		}
		
		protected override string updateQuery ()
		{
			string query = "UPDATE {0} SET {1} WHERE {2}";
			
			List<string> update = new List<string>();
			List<string> where  = new List<string>();
			
			_values.each ( (item, value) => update.Add (string.Format ("{0}='{1}'", item, value)) );
			_criteria.each ( (item, value) => where.Add (string.Format ("{0}='{1}'", item, value)) );
			
			query = string.Format (query, Table.Name, string.Join (", ", update), string.Join (", ", where));
			return query;
		}
	}
}

