using System;
using System.Collections.Generic;
using Sympathy;
using Sympathy.Attributes;

namespace Sympathy
{
	[ModelAttribute /* (Table = "test_model") */]
	public class TestModel : iModel
	{
		[FieldAttribute (AccessType=AccessTypes.ReadOnly, ColumnType=ColumnTypes.PrimaryKey)]
		public int ID;
		
		[FieldAttribute (AccessType=AccessTypes.ReadWrite)]
		public string Name;
		
		[FieldAttribute (DbType = System.Data.DbType.String)]
		public TestEnum type;
	}
	
	public enum TestEnum
	{
		None,
		Test1,
		Test2
	}
}

