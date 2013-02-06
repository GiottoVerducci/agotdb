using System;

namespace AGoTDB.OCTGN
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class OctgnCardDataAttribute : Attribute
	{
		public string PropertyName { get; set; }
		public string AttributeName { get; set; }
	}
}