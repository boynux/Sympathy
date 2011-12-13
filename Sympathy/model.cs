using System;
using System.Collections;
using System.Collections.Generic;

namespace Sympathy
{
	public class Model<_Handler> : iModel, IEnumerable where _Handler : iDatabaseHandler, new ()
	{
		public Model ()
		{
		}
		
		public dynamic objects 
		{
			get
			{
				return Manager;
			}
		}
		
		protected dynamic Manager
		{
			get
			{
				if (_manager == null) {
					Type[] modelType = { GetType (), typeof (_Handler) };
					Type managerType = typeof (Manager<,>).MakeGenericType (modelType);
					
					_manager = Activator.CreateInstance (managerType);
				}
				
				return _manager;
			}
		}
		
		public dynamic getObject (Criteria filter = null)
		{
			if (filter != null)
				Manager.setCriteria (filter);
			
			return Manager.getObject ();
		}
		
		public dynamic getObject (object id)
		{
			return getObject (new Criteria { {_manager.PrimaryKey.Name, id} });
		}
		
		public dynamic filter (Criteria filter = null) 
		{
			Manager.setCriteria (filter);
			return this;
		}
		
		public Model<_Handler> save ()
		{
			Manager.save (this);
			
			return this;
		}
		
		public void delete ()
		{
			if (!Manager.Table.PrimaryKey.getValue (this).Equals (0))
			{
				Manager.delete (this);
			}
		}
		
		public IEnumerator GetEnumerator ()
		{
			return Manager;
		}
		
		protected dynamic _manager;
		protected _Handler _handler;
	}
	
	public class Model : Model<Sympathy.SqliteHandler>
	{
		public Model () : base ()
		{
		}
	}
	
}