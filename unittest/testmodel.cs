using System;
using System.Collections.Generic;
using Sympathy;
using Sympathy.Attributes;

namespace Sympathy
{
	[ModelAttribute /* (Table = "test_model") */]
	public class TestModel : Model<Sympathy.MSSqlHandler>
	{
		[FieldAttribute (AccessType=AccessTypes.ReadOnly, ColumnType=ColumnTypes.PrimaryKey)]
		public readonly int ID = 0;
		
		[FieldAttribute (AccessType=AccessTypes.ReadWrite)]
		public  string Name = string.Empty;
	}
}

