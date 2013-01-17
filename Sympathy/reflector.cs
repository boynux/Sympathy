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
		
		public Dictionary<string, Column> getColumns ()
		{
			Dictionary<string, Column> columns = new Dictionary<string, Column> ();
			object[] fields = _model.GetType ().GetFields ( 
				System.Reflection.BindingFlags.Instance | 
				System.Reflection.BindingFlags.Public |
				System.Reflection.BindingFlags.NonPublic |
			    System.Reflection.BindingFlags.FlattenHierarchy
			);
			
			foreach (System.Reflection.FieldInfo field in fields) {
				try {
				if (field.GetCustomAttributes (true)[0].GetType () == typeof (Attributes.FieldAttribute)) {
					Column column = new Column(field);
					foreach (System.Reflection.CustomAttributeData attribute in CustomAttributeData.GetCustomAttributes (field)) {
						foreach (CustomAttributeNamedArgument argument in attribute.NamedArguments) {
							column.setAttribute (argument.MemberInfo.Name, argument.TypedValue);
						}
					
						columns.Add (field.Name, column);
					}
				}
				} catch (Exception /* e */) {}	
			}
			
			return columns;
		}

		public void afterSerialize ()
		{
			MethodInfo[] methods = _model.GetType ().GetMethods ( 
				System.Reflection.BindingFlags.Instance | 
				System.Reflection.BindingFlags.Public |
				System.Reflection.BindingFlags.NonPublic |
			    System.Reflection.BindingFlags.FlattenHierarchy
			);
			
			foreach (System.Reflection.MethodInfo method in methods) {
				try {
					if (method.GetCustomAttributes (true)[0].GetType () == typeof (Attributes.SerializeAttribute)) {
						method.Invoke ( _model, new object[] { } );
					}
				} catch (Exception /* e */) {}	
			}
			
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
			
			return new Sympathy.Table (table, getColumns ());
		}
		
		protected iModel _model;
		protected System.Reflection.MemberInfo info;
	}
}

