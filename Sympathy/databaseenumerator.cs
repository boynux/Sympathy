using System;
using System.Collections;
using System.Data;

namespace Sympathy
{
	public class DatabaseEnumerator : IEnumerator
	{
		public DatabaseEnumerator(IDataReader reader) 
		{
			// _reader = reader;
		}
		
		public object Current {
			get {
				throw new NotImplementedException ();
			}
		}
		
		public bool MoveNext ()
		{
			throw new NotImplementedException ();
		}
		
		public void Reset ()
		{
			throw new NotImplementedException ();
		}
		
		// IDataReader _reader;
	}
}