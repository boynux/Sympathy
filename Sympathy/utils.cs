using System;

namespace Sympathy
{
	public class Utils
	{
		public static string genrateDBNameFromType (Type type)
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
	}
}

