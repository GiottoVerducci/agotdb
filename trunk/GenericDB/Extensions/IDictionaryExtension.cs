using System.Collections.Generic;

namespace GenericDB.Extensions
{
	public static class IDictionaryExtension
	{
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			TValue result;
			if (!dictionary.TryGetValue(key, out result))
				return defaultValue;
			return result;
		}
	}
}
