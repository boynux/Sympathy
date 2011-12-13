using System;

namespace Sympathy.Attributes
{
	public class ModelAttribute : System.Attribute
	{
		public ModelAttribute ()
		{
		}
		
		public string Table 
		{
			get
			{
				return this._table;
			}
			set
			{
				this._table = value;
			}
		}
		
		protected string _table =  string.Empty;
	}
}

