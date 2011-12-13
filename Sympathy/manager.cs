using System;
using System.Collections;
using System.Collections.Generic;

namespace Sympathy
{
	public class Manager<_Model, _Handler> : 
		System.Collections.Generic.IEnumerator <_Model> 
		where _Model: Model<_Handler>, new () 
		where _Handler: iDatabaseHandler, new ()
	{
		public Manager ()
		{
		}
		
		public _Model getObject ()
		{
			_Model model = null;
			
			_Handler handler = new _Handler ();
			QueryBuilder builder = handler.QueryBuilder;
			
			if (_criteria != null)
				builder.Criteria = _criteria;
			
			builder.Type = QueryBuilder.QueryType.Select;
			builder.Table = Table;
			
			handler.QueryString = builder.ToString ();
			
			if (handler.next ()) {
				Dictionary <string, object> row = handler.fetchRow ();
			
				model = populateModel (row);
				handler.close ();
			} else {
				handler.close ();
				throw new DoesNotExistException ();
			}
			
			return model;
		}
		
		protected _Model populateModel (Dictionary<string, object> values)
		{	
			_Model model = new _Model ();
			foreach (Column column in Table) {
				if (typeof (iModel).IsAssignableFrom (column.Type)) {
					try {
						Model<_Handler> fmodel = (Model<_Handler>)Activator.CreateInstance (column.Type.MakeGenericType (typeof (_Handler)));
						column.setValue (model, fmodel.getObject (values[column.Name.ToLower ()]));
					} catch (DoesNotExistException /* e */) {
						column.setValue (model, null);
					}
				} else {
					column.setValue (model, values[column.Name]);
				}
			}
			
			return model;
		}
		
		public Column PrimaryKey 
		{
			get 
			{
				if (_table == null) {
					_Model model = new _Model ();
				
					Reflector reflector = new Reflector (model);
					_table = reflector.getTable  ();
				}
				
				return _table.PrimaryKey;			
			}
		}
		
		public Table Table 
		{
			get
			{
				if (_table == null) {
					_Model model = new _Model ();
			
					Reflector reflector = new Reflector (model);
					_table = reflector.getTable ();	
				}
				
				return _table;
			}
		}
		
		public void setCriteria (Criteria filter)
		{
			_criteria = filter;
		}
		
		public void prepareHandler ()
		{
			 _handler = new _Handler ();
			
			_handler.QueryBuilder.Type = QueryBuilder.QueryType.Select;
			
			if (_criteria != null)
				_handler.QueryBuilder.Criteria = _criteria;
			
			_handler.QueryBuilder.Table = Table;
		}
		
		protected string getSelectQuery ()
		{
			SqliteQueryBuilder builder = new SqliteQueryBuilder (Table);
			
			return builder.ToString ();
		}
		
		protected _Handler getInsertQuery (iModel model)
		{

			_Handler handler = new _Handler ();
			QueryBuilder builder = handler.QueryBuilder;
			builder.Table = Table;
			
			IDictionary<string, object> values = new Criteria ();
			foreach (Column column in Table) {
				if (column != Table.PrimaryKey || Table.PrimaryKey.AccessType != Sympathy.Attributes.AccessTypes.ReadOnly)
					values.Add (column.Name, column.getValue (model));
			} 
			
			if (Table.PrimaryKey.AccessType == Sympathy.Attributes.AccessTypes.ReadOnly &&
				 !Table.PrimaryKey.DefaultValue.Equals (Table.PrimaryKey.getValue (model)))
			{
				builder.Type = QueryBuilder.QueryType.Update;
				builder.Criteria = new Criteria () { {Table.PrimaryKey.Name, Table.PrimaryKey.getValue (model)} };
			}
			else
			{
				builder.Type = QueryBuilder.QueryType.Insert;
			}
			
			builder.Values = values;
			return handler;
		}
		
		public iModel save (iModel model)
		{
			_Handler handler;
			handler = getInsertQuery (model);			
			int id = handler.execute ();
	
			if (Table.PrimaryKey.AccessType == Sympathy.Attributes.AccessTypes.ReadOnly && 
				handler.QueryBuilder.Type == QueryBuilder.QueryType.Insert )
				Table.PrimaryKey.setValue (model, id);
			
			return model;
		}
		
		public void delete (iModel model)
		{
			Criteria criteria = new Criteria () { {"id", Table.PrimaryKey.getValue (model) } };
			_Handler handler = new _Handler ();
			
			handler.QueryBuilder.Table = Table;
			handler.QueryBuilder.Values = criteria;
			handler.QueryBuilder.Type = QueryBuilder.QueryType.Delete;
			
			handler.execute ();
		}
		
		public _Model Current {
			get {
				_Model model = null;
				Dictionary <string, object> row = _handler.fetchRow ();
				model = populateModel (row);
				
				return model;
			}
		}
		
		object IEnumerator.Current {
			get {
				return Current;
			}
		}
		
		public void Dispose ()
		{
			_handler.close ();
		}
		
		public bool MoveNext ()
		{
			if (_handler == null) {
				prepareHandler ();
			}
			
			return _handler.next ();
		}
		
		public void Reset ()
		{
			throw new NotImplementedException ();
		}
		
		protected Criteria _criteria;
		protected _Handler _handler;
		protected Table _table;
		protected _Model _model;
	}
	
	public class Manager<_Model> : Manager<_Model, Sympathy.SqliteHandler> where _Model: Model<Sympathy.SqliteHandler>, new () 
	{
		public Manager (): base () {}
	}
	
	public class DoesNotExistException : Exception {}
}