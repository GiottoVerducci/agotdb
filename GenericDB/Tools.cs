using System;

namespace GenericDB
{
	public class Tools
	{
		/// <summary>
		/// Synthesizes the result of multiple comparisons by returning -1
		/// if the first non-0 comparison result is negative, 1 if the first non-0
		/// comparison result is positive, and 0 if all comparison results are 0.
		/// </summary>
		/// <param name="compResults">The list of comparison results.</param>
		/// <returns>-1, 0 or 1</returns>
		public static int MultipleCompare(params int[] compResults)
		{
			for (var i = 0; i < compResults.Length; ++i)
				if (compResults[i] != 0)
					return Math.Sign(compResults[i]);
			return 0;
		}
	}
}
