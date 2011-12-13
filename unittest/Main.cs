using System;
using Sympathy;

namespace UnitTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			TestModel test = new TestModel ();
			
			Console.WriteLine (string.Format ("TestModel default values: {0}: {1}.", Activator.CreateInstance (typeof (int)),test.Name));
			// TestModel model2 = test.getObject (1);
			// Console.WriteLine (string.Format ("{0} ({1}).", model2.ID, model2.Name));
			
			for (int index = 0; index < 100; ++index) {
				test.Name = string.Format ("test update {0}", index + 1);
				Console.WriteLine (string.Format ("Creating new TestModel with name: {0} and ID {1}.", test.Name, test.ID));
				
				test.save ();
			}
			
			/*
			Console.WriteLine (string.Format ("TestModel saved with ID: {0}.", test.ID));
			
			Console.WriteLine (string.Format ("Updating TestModel with ID: {0}.", test.ID));
			test.Name = "test1000";
			test.save ();
			*/
			System.Collections.Generic.List <TestModel> models = new System.Collections.Generic.List<TestModel> ();
			TestModel model = test.filter ();
			foreach (TestModel item in model)
			{		
				Console.WriteLine (string.Format ("Deleting {0} ({1}).", item.ID, item.Name));
				models.Add (item);
			}
			
			models.ForEach (item => item.delete ());
			
		}
	}
}
