using System;

namespace Sympathy.Attributes
{
	public class MethodAttribute
	{
		public MethodType Type;
	}
	
	public enum MethodType {
		BeforeSerialize,
		AfterSerialize
	}
}

