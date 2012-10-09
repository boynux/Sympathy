using System;
using Sympathy;
using System.Collections.Generic;

namespace UnitTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Manager<MSSqlHandler> manager = new Manager<MSSqlHandler> ();
			TestModel test = new TestModel ();
			
			Console.WriteLine (string.Format ("TestModel default values: {0}: {1}.", Activator.CreateInstance (typeof (int)),test.Name));
			
			for (int index = 0; index < 10; ++index) {
				TestModel test1 = new TestModel ();
				
				test1.Name = string.Format ("test update {0}", index + 1);
				test1.type = TestEnum.Test1;
				Console.WriteLine (string.Format ("Creating new TestModel with name: {0} and ID {1}.", test1.Name, test1.ID));
				
				manager.save (test1);
				Console.WriteLine (string.Format ("TestModel saved with ID: {0}.", test1.ID));
			}
			
			
			LinkedModel lm = new LinkedModel ();
			lm.TestModel = test;
			
			manager.save (lm);
			
			Console.WriteLine (string.Format ("Deleting linked model with ID {0} to TestModel {1}:{2}", lm.ID, lm.TestModel.ID, lm.TestModel.Name));
			manager.delete (lm);
			
			// manager.delete (test);
			
			foreach (TestModel item in manager.filter<TestModel> (new Criteria ())) {
				Console.WriteLine (string.Format ("TestModel {0} ({1} - {2}).", item.ID, item.Name, item.type.ToString ()));
			}
			
			
			IList<TestModel> models = manager.filter<TestModel> (new Criteria () { {"id__lt", 50} });
			foreach (TestModel item in models)
			{	
				Console.WriteLine (string.Format ("Deleting {0} ({1}).", item.ID, item.Name));
				manager.delete (item);
			}
		}
	}
}