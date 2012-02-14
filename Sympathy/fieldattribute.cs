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
		
		public ColumnTypes ColumnType { get; set; }
		public AccessTypes AccessType { get; set; }
		public System.Data.DbType DbType { get; set; }

		protected object _value;
		protected ColumnTypes _columnType;
		protected AccessTypes _accessType;
	}
	
	public enum ColumnTypes
	{
		None,
		PrimaryKey,
		OneToMany
	}
	
	public enum AccessTypes
	{
		ReadWrite,
		ReadOnly
	}
	
	class InvalidFieldDataException : Exception {}
}

