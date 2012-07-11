using System;

namespace Sympathy
{
	public interface iDatabaseHandler
	{
		bool next ();
		System.Collections.Generic.Dictionary <string, object> fetchRow ();
		void close ();
		void addParameter (string name, object value);
		object execute ();
		
		Criteria Criteria { set; }
		string QueryString { get; set; }
		QueryBuilder QueryBuilder { get; }
	}
}

