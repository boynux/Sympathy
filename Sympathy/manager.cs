using System;
using System.Collections;
using System.Collections.Generic;

namespace Sympathy
{
	public class Manager<_Handler> 
		where _Handler: DatabaseHandler, new ()
	{
		public Manager ()
		{
		}
		
		public _Model getObjectFromQuery<_Model> (string queryString) where _Model: iModel, new ()
		{
			_Handler handler = new _Handler ();
			handler.QueryString = queryString;
			
			_Model model;
				
			if (handler.next ()) {
				Dictionary <string, object> row = handler.fetchRow ();
			
				model = populateModel<_Model> (row);
				handler.close ();
			} else {
				handler.close ();
				throw new DoesNotExistException ();
			}
			
			return model;
		}
		
		public _Model getObject<_Model> () where _Model: iModel, new ()
		{
			_Model model = new _Model ();
			
			_Handler handler = new _Handler ();
			QueryBuilder builder = handler.QueryBuilder;
			builder.Table = Table (model);
			
			if (_criteria != null)
				builder.Criteria = _criteria;
			
			return getObjectFromQuery<_Model> (builder.ToString ());
		}
		
		public _Model getObject<_Model> (Criteria filter) where _Model: iModel, new ()
		{
			if (filter != null)
				setCriteria (filter);
			
			return getObject <_Model> ();
		}
		
		public _Model getObject <_Model> (object id) where _Model: iModel, new ()
		{
			setCriteria (new Criteria { {PrimaryKey<_Model> ().Name, id} });
			return getObject<_Model> ();
		}
		
		protected _Model populateModel<_Model> (Dictionary<string, object> values) where _Model: iModel, new ()
		{	
			_Model model = new _Model ();
			
			foreach (Column column in Table (model)) {
				if (column.ColumnType == Sympathy.Attributes.ColumnTypes.OneToMany) {
					throw new NotImplementedException ();
				} else if (typeof (iModel).IsAssignableFrom (column.Type)) {
					try {
						dynamic fmodel = Activator.CreateInstance (column.Type);
						
						dynamic value = this.GetType ().GetMethod ("getObject", new Type [] { typeof (Criteria) }).MakeGenericMethod (column.Type).Invoke (this, new object[] { new Criteria () { {Table(fmodel).PrimaryKey.Name.ToLower (), values[column.Name.ToLower ()] } } });
						column.setValue (model, value);
					} catch (DoesNotExistException /* e */) {
						// column.setValue (model, null);
					}
				} else {
					column.setValue (model, values[column.Name.ToLower ()]);
				}
			}
			
			return model;
		}
		/*
		protected string getOneToManyQuery<_Model> (Column column) where _Model: iModel, new ()
		{
			Type[] genericType = column.Type.GetGenericArguments ();
		
			if (column.Type.IsAssignableFrom (typeof (IList<>)))
			{
				//Column id = new Column (new System.Reflection.FieldInfo ())
				//Table = new Table (
				//	string.Format ("{0}_{1}", Utils.genrateDBNameFromType (typeof (_Model)), column.Name.ToLower ()),
					
			}
			_Handler handler = new _Handler ();
			// handler.QueryBuilder.
			
			return string.Empty;

		}
		*/	
		
		public Column PrimaryKey<_Model> () where _Model: iModel, new ()
		{
			return Table <_Model> ().PrimaryKey;	
		}
		
		public Column PrimaryKey (iModel model)
		{
			return Table (model).PrimaryKey;	
		}
		
		public Table Table<_Model> () where _Model: iModel, new ()
		{
			iModel model = new _Model ();
		
			Reflector reflector = new Reflector (model);
			return reflector.getTable ();	
		}
		
		public Table Table (iModel model)
		{
			Reflector reflector = new Reflector (model);
			return reflector.getTable ();	
		}
		
		public void setCriteria (Criteria filter)
		{
			_criteria = filter;
		}
		
		public IList<_Model> filterFromQuery <_Model> (string queryStrng) where _Model: iModel, new ()
		{
			_Handler handler = new _Handler ();
			handler.QueryString = queryStrng;
			
			List<_Model> list = new List<_Model> ();
			
			while (handler.next ()) {
				Dictionary <string, object> row = handler.fetchRow ();
			
				list.Add (populateModel<_Model> (row));
			} 
			
			handler.close ();
			
			return list;
		}
		
		public IList<_Model> filter <_Model> (Criteria criteria = null) where _Model: iModel, new ()
		{
			setCriteria (criteria);
			prepareHandler<_Model> ();
			
			return filterFromQuery <_Model> (_handler.QueryBuilder.ToString ());
		}
		
		public void prepareHandler <_Model> () where _Model: iModel, new ()
		{
			 _handler = new _Handler ();
			_handler.QueryBuilder.Table = Table <_Model>();
			
			if (_criteria != null)
				_handler.QueryBuilder.Criteria = _criteria;
		}
		
		protected string getSelectQuery<_Model> () where _Model: iModel, new ()
		{
			_Handler handler = new _Handler ();
			QueryBuilder builder = handler.QueryBuilder;
			builder.Table = Table<_Model> ();
			
			return builder.ToString ();
		}
		
		protected _Handler getInsertQuery (iModel model)
		{
			_Handler handler = new _Handler ();
			QueryBuilder builder = handler.QueryBuilder;
			builder.Table = Table (model);
			builder.Type = QueryBuilder.QueryType.Insert;
			
			IDictionary<string, object> values = new Criteria ();
			foreach (Column column in builder.Table) {
				if (column != builder.Table.PrimaryKey || builder.Table.PrimaryKey.AccessType != Sympathy.Attributes.AccessTypes.ReadOnly) {
					if (typeof (iModel).IsAssignableFrom (column.Type)) {
						dynamic fmodel = column.getValue (model);
						
						values.Add (column.Name.ToLower (), Table (fmodel).PrimaryKey.getValue (fmodel));
					} else {
						values.Add (column.Name.ToLower (), column.getValue (model));
					}
				}
			}
			
			if (/* builder.Table.PrimaryKey.AccessType == Sympathy.Attributes.AccessTypes.ReadOnly && */
				 !builder.Table.PrimaryKey.DefaultValue.Equals (builder.Table.PrimaryKey.getValue (model)))
			{
				builder.Type = QueryBuilder.QueryType.Update;
				builder.Criteria = new Criteria () { {builder.Table.PrimaryKey.Name, builder.Table.PrimaryKey.getValue (model)} };
			}
			else
			{
				builder.Type = QueryBuilder.QueryType.Insert;
			}
			
			builder.Values = values;
			return handler;
		}
		
		public dynamic save (iModel model)
		{
			_Handler handler = getInsertQuery (model);
			int id = handler.execute ();
			
			if (Table (model).PrimaryKey.AccessType == Sympathy.Attributes.AccessTypes.ReadOnly && 
				handler.QueryBuilder.Type == QueryBuilder.QueryType.Insert )
				Table (model).PrimaryKey.setValue (model, id);
			
			return model;
		}
		
		public void delete (iModel model)
		{
			Criteria criteria = new Criteria () { {"id", Table(model).PrimaryKey.getValue (model) } };
			_Handler handler = new _Handler ();
			
			handler.QueryBuilder.Table = Table (model);
			handler.QueryBuilder.Type = QueryBuilder.QueryType.Delete;
			handler.QueryBuilder.Criteria = criteria;
			
			handler.execute ();
		}
		
		public void Dispose ()
		{
			_handler.close ();
		}

		protected Criteria _criteria;
		protected _Handler _handler;
		protected Table _table;
		protected iModel _model;
	}
	
	public class Manager: Manager<Sympathy.SqliteHandler>
	{
		public Manager (): base () {}
	}
	
	public class DoesNotExistException : Exception {}
}