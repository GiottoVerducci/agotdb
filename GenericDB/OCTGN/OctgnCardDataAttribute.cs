using System;

namespace GenericDB.OCTGN
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class OctgnCardDataAttribute : Attribute
	{
		public string PropertyName { get; set; }
		public string AttributeName { get; set; }
	}
}