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
		
		public AccessTypes AccessType { get; set; }
		public ColumnTypes ColumnType { get; set; }
		public System.Data.DbType DbType { get; set; }
		
		public void setValue (iModel model, object val)
		{
			// parse Enum values ...
			if (Type.IsEnum) {
				if (DbType == System.Data.DbType.String) {
					_fieldInfo.SetValue (model, Enum.Parse (Type, val.ToString ()));
				} else {
					_fieldInfo.SetValue (model, Enum.ToObject (Type, val));
				}
			} else if (!Type.Equals (val.GetType())) {
				System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter (val.GetType ());
				_fieldInfo.SetValue (model, converter.ConvertTo (val, Type));
			} else {
				_fieldInfo.SetValue (model, val);
			}
				
		}
		
		public object getValue (iModel model)
		{
			// convert Enum values depending to 
			// column type ...
			if (Type.IsEnum && ( 
				DbType != System.Data.DbType.String ||
				DbType != System.Data.DbType.String	)
			) {
				return (int)_fieldInfo.GetValue (model);
			} else {
				return _fieldInfo.GetValue (model);
			}
		}
		
		public void setAttribute (System.Reflection.CustomAttributeTypedArgument attr)
		{
			if (attr.ArgumentType.Equals (typeof (AccessTypes)))
				AccessType = (AccessTypes)attr.Value;
			else if (attr.ArgumentType.Equals (typeof (ColumnTypes)))
				ColumnType = (ColumnTypes)attr.Value;
			else if (attr.ArgumentType.Equals (typeof (System.Data.DbType)))
				DbType = (System.Data.DbType)attr.Value;
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

