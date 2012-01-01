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
							column.setAttribute (argument.TypedValue.Value);
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
				table = genrateTableNameFromType (_model.GetType ());
			
			return new Sympathy.Table (table, getColumns ().ToArray ());
		}
		
		protected string genrateTableNameFromType (Type type)
		{
			string typeName = type.ToString ();
			
			// replace name space points with _
			string[] parts =  typeName.Split ('.');
			for (int index = 0; index < parts.Length; ++index)
			{
				// Conver camelized form into _ separated words
				string word = string.Empty;
				for (int index1 = 0; index1 < parts[index].Length; ++index1)
				{
					char letter = parts[index][index1];
					if (letter >= 'A' && letter <= 'Z') 
					{
						word += (index1 > 0 ? "_" : "") + letter.ToString ().ToLower ();
					} 
					else
					{
						word += letter;
					}
				}
				parts[index] = word;
			}
			
			return string.Join ("_", parts);
		}
		
		protected iModel _model;
		protected System.Reflection.MemberInfo info;
	}
}

