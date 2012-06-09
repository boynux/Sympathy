using System;

namespace Sympathy
{
	public class Utils
	{
		public static string genrateNameFromType (Type type)
		{
			return genrateNameFromType (type.ToString ());
		}
		
		public static string genrateNameFromType (string typeName)
		{
			// replace name space points with _
			string[] parts =  typeName.Split ('.');
			for (int index = 0; index < parts.Length; ++index)
			{
				// Conver camelized form into _ separated words
				string word = string.Empty;
				for (int index1 = 0; index1 < parts[index].Length; ++index1)
				{
					char letter = parts[index][index1];
					if (letter >= 'A' && letter <= 'Z') {
						if (index1 > 0) {
							char prevLetter = parts[index][index1 - 1];
							
							if ((prevLetter >= 'a' && prevLetter <= 'z') || (prevLetter >= '0' && prevLetter <= '9'))
								word += "_" + letter.ToString ().ToLower ();
							else 
								word += letter.ToString ().ToLower ();
						}
						/*
						if (index1 < parts[index].Length - 1) {
							char nextLetter = parts[index][index1 + 1];
							if (nextLetter >= 'a' && nextLetter <= 'z')
								word += (index1 > 0 ? "_" : "") + letter.ToString ().ToLower ();
							else 
								word += letter.ToString ().ToLower ();
						} */ else {
							word += letter.ToString ().ToLower ();
						}
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

