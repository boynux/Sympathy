using System;
using System.Collections;
using System.Collections.Generic;

namespace Sympathy
{
	public class IntegerField : Field
	{
		public IntegerField (params object[] arguments) : base (arguments)
		{
		}
		
		public override object Value {
			get {
				return _value;
			}
			set {
				int result;
				if (int.TryParse (value.ToString (), out result))
					_value = result;
				else
					throw new InvalidFieldDataException ();
			}
		}
	}
}

