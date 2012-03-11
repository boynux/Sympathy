using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sympathy
{
	public class Reflector
	{
		public Reflector (iModel model)
		{
			_model = model;
			info = _model.GetType ();
		}
		
		public List<Column> getColumns ()
		{
			List<Column> columns = new List<Column> ();
			object[] fields = _model.GetType ().GetFields ( 
				System.Reflection.BindingFlags.Instance | 
				System.Reflection.BindingFlags.Public |
				System.Reflection.BindingFlags.NonPublic
			);
			
			foreach (System.Reflection.FieldInfo field in fields) {
				try {
				if (field.GetCustomAttributes (true)[0].GetType () == typeof (Attributes.FieldAttribute)) {
					Column column = new Column(field);
					foreach (System.Reflection.CustomAttributeData attribute in CustomAttributeData.GetCustomAttributes (field)) {
						foreach (CustomAttributeNamedArgument argument in attribute.NamedArguments) {
							column.setAttribute (argument.TypedValue);
						}
					
						columns.Add (column);
					}
				}
				} catch (Exception /* e */) {}	
			}
			
			return columns;
		}
		
		public Table getTable ()
		{
			string table = string.Empty;
			
			object[] attributes = _model.GetType ().GetCustomAttributes (true);
			foreach (Attribute attribute in attributes) {
				if (attribute.GetType () == typeof (Sympathy.Attributes.ModelAttribute))
				{
					Sympathy.Attributes.ModelAttribute attr = (Sympathy.Attributes.ModelAttribute)attribute;
					if (attr.Table != string.Empty)
					{
						table = attr.Table;
					}
				}
			}
			if (table.Length == 0)
				table = Utils.genrateNameFromType (_model.GetType ());
			
			return new Sympathy.Table (table, getColumns ().ToArray ());
		}
		
		protected iModel _model;
		protected System.Reflection.MemberInfo info;
	}
}

