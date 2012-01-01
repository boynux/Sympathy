using System;
using System.Data;
using System.Collections.Generic;

namespace Sympathy
{
	public abstract class DatabaseHandler : iDatabaseHandler
	{
		public DatabaseHandler ()
		{
			_criteria = new Criteria ();
			_queryString = string.Empty;
			_connectionString = string.Empty;
		}
		
		~DatabaseHandler ()
		{
			if (_reader != null && !_reader.IsClosed) {
				_reader.Close ();
				
				// _reader.Dispose ();
			}
			
			if (_connection != null && _connection.State != ConnectionState.Closed) {
				_connection.Close ();
			}
		}
		
		public string QueryString 
		{
			get 
			{
				return _queryString;
			}
			set
			{
				_queryString = value;
			}
		}
		
		public Criteria Criteria 
		{
			set
			{
				_criteria = value;
			}
		}
		
		public int AffectedRows
		{
			get
			{
				return _affectedRows;
			}
		}
		
		public Dictionary<string, object> fetchRow ()
		{
			Dictionary<string, object> result = new Dictionary<string, object> ();
			
			if (_reader == null ||_reader.IsClosed) {
				throw new DataIsNotReady ();
			}
			
			for (int index = 0; index < _reader.FieldCount; ++index)
			{
				result[_reader.GetName (index).ToLower ()] = _reader.GetValue (index);
			}
			
			return result;
		}
		
		public void close ()
		{
			if (_reader != null && !_reader.IsClosed) {
				_reader.Close ();
			}
			
			if (_connection != null && _connection.State != ConnectionState.Closed) {
				_connection.Close ();
			}
		}
		
		public bool next ()
		{
			if (_reader == null)
				read ();
			
			return _reader.Read ();
		}
		
		protected void prepare ()
		{
			_connection.Open ();
			_command.Connection = _connection;
			
			if (_queryString == string.Empty) 
				_queryString = QueryBuilder.ToString ();
			
			_command.CommandText = _queryString;
			
			// Console.WriteLine (_queryString);
			
			applyFilters ();
			applyParameters ();
			
		}
		
		public void read ()
		{
			prepare ();
			
			_reader = _command.ExecuteReader (CommandBehavior.CloseConnection);
		}
		
		public int execute ()
		{
			prepare ();
			int result = Convert.ToInt32 (_command.ExecuteScalar ());
			this.close ();
			
			return result;
		}
		
		public QueryBuilder QueryBuilder
		{
			get {
				return _queryBuilder;
			}
		}
		
		public abstract void addParameter (string name, object value);
		protected abstract void applyFilters ();
		protected abstract void applyParameters ();
		
		protected IDbConnection _connection;
		protected IDbCommand _command;
		protected IDataReader _reader;
		protected string _queryString;
		protected string _queryStringCriteria;
		protected string _connectionString;
		protected Criteria _criteria;
		protected SqliteQueryBuilder _builder;
		protected int _affectedRows;
		protected QueryBuilder _queryBuilder; 
	}
	
	public class Criteria : System.Collections.Generic.Dictionary <string, object>
	{
		public Criteria (System.Collections.Generic.IDictionary <string, object> init) : base (init)
		{
		}
		
		public Criteria () : base ()
		{
		}
		
		public void each (Action<string, object> action)
		{
			foreach (KeyValuePair <string, object> item in this) 
			{
				action (item.Key, item.Value);
			}
		}
	}
	
	public class NotSingleRowException : Exception {}
	public class DataIsNotReady : Exception {}
}