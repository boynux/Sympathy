using System;
using Sympathy.Attributes;

namespace Sympathy
{
	public class Column
	{
		public Column (System.Reflection.FieldInfo fi)
		{
			_fieldInfo = fi;
			
			initialize ();
		}
		
		protected void initialize ()
		{
		}
		
		public AccessTypes AccessType 
		{
			get
			{
				return _accessType;
			}
			set
			{
				_accessType = value;
			}
		}
		
		public ColumnTypes ColumnType
		{
			get
			{
				return _columnType;
			}
			set
			{
				_columnType = value;
			}
		}
		
		public object DefaultValue
		{
			get
			{
				return Type.IsValueType ? Activator.CreateInstance (Type) : null;
			}
		}
		
		public string Name 
		{
			get
			{
				return _fieldInfo.Name;
			}
		}
		
		public Type Type
		{
			get
			{
				return _fieldInfo.FieldType;
			}
		}
		
		public void setValue (iModel model, object val)
		{
			System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter (val.GetType ());
			// System.Reflection.MethodInfo converter = typeof(System).GetMethod ("convert").MakeGenericMethod (val.GetType (), _fieldInfo.ReflectedType);
			
			
			_fieldInfo.SetValue (model, converter.ConvertTo (val, _fieldInfo.FieldType));
		}
		
		public object getValue (iModel model)
		{
			return _fieldInfo.GetValue (model);
		} 
		
		public void setAttribute (object attr)
		{
			if (attr is AccessTypes)
				AccessType = (AccessTypes)attr;
			else if (attr is ColumnTypes)
				ColumnType = (ColumnTypes)attr;
		}
		
		public override string ToString ()
		{
			return string.Format ("[Column: AccessType={0}, ColumnType={1}, Name={2}, Type={3}]", AccessType, ColumnType, Name, Type.Name);
		}
		
		protected System.Reflection.FieldInfo _fieldInfo;
		protected AccessTypes _accessType;
		protected ColumnTypes _columnType;
	}
}

