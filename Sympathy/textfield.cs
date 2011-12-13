using System;
using System.Collections.Generic;

namespace Sympathy
{
	public class TextField : Field
	{
		public TextField (params object[] arguments) : base (arguments)
		{
		}
		
		public override object Value {
			get {
				return _value;
			}
			set {
				_value = value.ToString ();
			}
		}
	}
}

