using System;
using System.Collections.Generic;

namespace Sympathy.Attributes
{
	public class FieldAttribute : System.Attribute
	{
		public FieldAttribute ()
		{
			initialize ();
		}
		
		private void initialize ()
		{
		}
		
		// public abstract object Value { get; set; }
		
		/*
		public Type Type 
		{
			get
			{
				return this.GetType ();
			}
		}
		*/
		
		public ColumnTypes ColumnType 
		{
			get
			{
				return _columnType;
			}
			
			set 
			{
				this._columnType = value;
			}
		}
		
		public AccessTypes AccessType
		{
			get
			{
				return _accessType;
			}
			set 
			{
				this._accessType = value;
			}
			
		}

		protected object _value;
		protected ColumnTypes _columnType;
		protected AccessTypes _accessType;
	}
	
	public enum ColumnTypes
	{
		None,
		PrimaryKey,
		Reference
	}
	
	public enum AccessTypes
	{
		ReadWrite,
		ReadOnly
	}
	
	class InvalidFieldDataException : Exception {}
}

