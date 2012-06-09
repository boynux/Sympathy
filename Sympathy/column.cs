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
			DbType = System.Data.DbType.String;
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
				if (_columnName == string.Empty)
					return Utils.genrateNameFromType(_fieldInfo.Name);
				
				return _columnName;
			}
			
			set {
				_columnName = value;
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
			} else if ( val.Equals (DBNull.Value) && Type.IsValueType ) {
				_fieldInfo.SetValue (model, Activator.CreateInstance (Type));
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
			if (Type.IsEnum && DbType != System.Data.DbType.String) {
				return (int)_fieldInfo.GetValue (model);
			} else if (Type == typeof (Boolean) && DbType != System.Data.DbType.String) {
				return Convert.ToBoolean (_fieldInfo.GetValue (model)) == true ? 1 : 0;
			} else {
				return _fieldInfo.GetValue (model);
			}
		}
		
		public void setAttribute (string name, System.Reflection.CustomAttributeTypedArgument attr)
		{
			if (attr.ArgumentType.Equals (typeof (AccessTypes)))
				AccessType = (AccessTypes)attr.Value;
			else if (attr.ArgumentType.Equals (typeof (ColumnTypes)))
				ColumnType = (ColumnTypes)attr.Value;
			else if (attr.ArgumentType.Equals (typeof (System.Data.DbType)))
				DbType = (System.Data.DbType)attr.Value;
			else if (name == "ColumnName")
				Name = (string)attr.Value;
		}
		
		public override string ToString ()
		{
			return string.Format ("[Column: AccessType={0}, ColumnType={1}, Name={2}, Type={3}]", AccessType, ColumnType, Name, Type.Name);
		}
		
		protected System.Reflection.FieldInfo _fieldInfo;
		protected AccessTypes _accessType;
		protected ColumnTypes _columnType;
		protected string _columnName = "";
	}
}

