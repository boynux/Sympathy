using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Sympathy
{
	public class MSSqlHandler : DatabaseHandler
	{
        // TODO: connections string need to be moved to a proper place.
		const string ConnectionString = "Data Source=localhost;User ID=boynux; Initial Catalog=test; Password=secret";
		
		public MSSqlHandler () : base ()
		{
			_command = new System.Data.SqlClient.SqlCommand ();
			_connection = new System.Data.SqlClient.SqlConnection (ConnectionString);
			_queryBuilder = new MSSqlQueryBuilder ();
			// ((SqlConnection)_connection).StateChange += new StateChangeEventHandler(this.connectionEventHandler);
			
			_queryBuilder = new MSSqlQueryBuilder ();
		}
		
		public MSSqlHandler (string connectionString) : this ()
		{
			_connectionString = connectionString;
			_connection.ConnectionString = _connectionString;
		}
		
		public override void addParameter (string name, object value)
		{
			((SqlCommand)_command).Parameters.AddWithValue (string.Format ("@{0}", name), value);
		}
		
		protected override void applyFilters ()
		{
			if (_criteria == null)
				return;
			
			List<string> criteria = new List<string> ();
			_criteria.each ( (key, value) => criteria.Add (string.Format ("{0}='{1}'", key, value)) );
			
			if (_criteria.Count > 0)
				_queryString += string.Format (" WHERE {0}", string.Join (", ", criteria.ToArray ()));

		}
		
		protected override void applyOrdering ()
		{
			
		}
		
		protected override void applyParameters ()
		{
			if (_criteria == null)
				return;
			
			_criteria.each ( (key, value) => ((SqlCommand)_command).Parameters.AddWithValue (key, value) );
		}
		
		protected void connectionEventHandler (object sender, StateChangeEventArgs args) 
		{
			Console.WriteLine (string.Format ("connection satate changed from {0} to {1}.", args.OriginalState, args.CurrentState));
		}
	}
}
