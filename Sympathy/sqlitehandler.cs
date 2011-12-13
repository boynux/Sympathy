using System;
using Mono;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;

namespace Sympathy
{
	public class SqliteHandler : DatabaseHandler
	{
		public SqliteHandler () : base ()
		{
			string dataSource = "test.sqlite";
			_command = new SqliteCommand ();
			
			_connectionString = string.Format ("Data Source={0}", dataSource);
			_connection = new SqliteConnection (_connectionString);
			
			_queryBuilder = new SqliteQueryBuilder ();
		}
	
		protected override void applyFilters ()
		{
			if (_criteria == null)
				return;
			
			List<string> criteria = new List<string>();
			foreach (KeyValuePair<string, object> item in _criteria)
			{
				criteria.Add (string.Format ("{0}=@{0}", item.Key));
			}
			
			if (criteria.Count > 0)
				_queryString += string.Format (" WHERE {0}", string.Join (", ", criteria.ToArray ()));
		}
		
		protected override void applyParameters ()
		{
			if (_criteria == null)
				return ;
			
			foreach (KeyValuePair<string, object> item in _criteria) {
				((SqliteCommand)_command).Parameters.AddWithValue (string.Format ("@{0}", item.Key), item.Value);
			}
		}
		
		public override void addParameter (string name, object value)
		{
			((SqliteCommand)_command).Parameters.AddWithValue (string.Format ("@{0}", name), value);
		}
	}
}

