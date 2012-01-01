using System;
using Sympathy;
using Sympathy.Attributes;

namespace UnitTest
{
	[Model (Table = "sympathy_linked_model")]
	public class LinkedModel : iModel
	{
		[FieldAttribute (AccessType = AccessTypes.ReadOnly, ColumnType = ColumnTypes.PrimaryKey)]
		public readonly int ID ;
		
		[FieldAttribute]
		protected int testModel;
		
		public TestModel TestModel 
		{
			get
			{
				if (_testModel == null) {
					Manager manager = new Manager ();
					_testModel =  manager.getObject<TestModel>(testModel);
				}
				
				return _testModel;
			}
			
			set
			{
				testModel = value.ID;
				_testModel = null;
			}
		}
		
		public TestModel _testModel;
	}
}