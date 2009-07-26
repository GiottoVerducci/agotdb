
namespace GenericDB.BusinessObjects
{
	/// <summary>
	/// Represents a pair of strings.
	/// </summary>
	public class StringPair
	{
		public string Value1 { get; set; }
		public string Value2 { get; set; }

		public StringPair()
		{
			Value1 = "";
			Value2 = "";
		}

		public StringPair(string value1, string value2)
		{
			Value1 = value1;
			Value2 = value2;
		}

		public static StringPair Add(StringPair first, StringPair second)
		{
			return new StringPair(first.Value1 + second.Value1, first.Value2 + second.Value2);
		}

		public static StringPair operator +(StringPair first, StringPair second)
		{
			return Add(first, second);
		}
	}
}