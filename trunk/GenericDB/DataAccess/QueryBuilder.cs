// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012 Vincent Ripoll
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Beyond.ExtendedControls;

namespace GenericDB.DataAccess
{
	public static class QueryBuilder
	{
		public static string EscapeSqlCharacters(string text)
		{
			return EscapeSqlCharacters(text, true);
		}

		public static string EscapeSqlCharacters(string text, bool escapePercent)
		{
			string result = text.Replace("[", "[[]").Replace("'", "''").Replace("\"", "\"\"");
			if (escapePercent)
				result = result.Replace("%", "[%]");
			return result;
		}

		/// <summary>
		/// Returns the filtering expression for queries relative to a given column within a range of values
		/// given by the Text member of two TextBox objects.
		/// If the string value of the lower bound of the range is "", then the lower bound is considered to be 0.
		/// If the string value of the upper bound of the range is "", then there is no upper bound.
		/// "X" values are always found, regardless of the bounds
		/// </summary>
		/// <param name="lowTextBox">the TextBox containing the lower bound of the range</param>
		/// <param name="highTextBox">the TextBox containing the upper bound of the range</param>
		/// <param name="column">the name of the field that is filtered</param>
		/// <returns>A query containing the filtering expression</returns>
		public static Query GetFilterFromRangeBoxes(TextBox lowTextBox, TextBox highTextBox, string column)
		{
			string high = highTextBox.Text.Trim();
			string low = lowTextBox.Text.Trim();
			return GetFilterFromRange(low, high, column);
		}

		/// <summary>
		/// Returns the filtering expression for queries relative to a given column within a range of values.
		/// If the string value of the lower bound of the range is "", then the lower bound is considered to be 0.
		/// If the string value of the upper bound of the range is "", then there is no upper bound.
		/// "X" values are always found, regardless of the bounds
		/// </summary>
		/// <param name="low">the TextBox containing the lower bound of the range</param>
		/// <param name="high">the TextBox containing the upper bound of the range</param>
		/// <param name="column">the name of the field that is filtered</param>
		/// <returns>A query containing the filtering expression</returns>
		private static Query GetFilterFromRange(string low, string high, string column)
		{
			var result = new Query();
			if (string.IsNullOrEmpty(low) && string.IsNullOrEmpty(high))
				return result;

			// sql part of the query
			if (!string.IsNullOrEmpty(low))
				result.SqlQuery = String.Format("({0} >= {1})", column, Int32.Parse(low, CultureInfo.CurrentCulture));
			if (!string.IsNullOrEmpty(high))
				result = result + new Query(String.Format("({0} <= {1})", column, Int32.Parse(high, CultureInfo.CurrentCulture)), "");

			result.SqlQuery = String.Format("(({0} = -1) OR ({1}))", column, result.SqlQuery);

			// human part of the query
			if (!string.IsNullOrEmpty(low))
			{
				result.HumanQuery = !string.IsNullOrEmpty(high)
					? String.Format(CultureInfo.CurrentCulture, Query.Localization.RangeBetween, column, low, high)
					: String.Format(CultureInfo.CurrentCulture, Query.Localization.RangeGreaterOrLesser, column, ">=", low);
			}
			else
				result.HumanQuery = String.Format(CultureInfo.CurrentCulture, Query.Localization.RangeGreaterOrLesser, column, "<=", high);
			return result;
		}

		/// <summary>
		/// Returns a filter (subquery) reflecting the state of a checked box list.
		/// All items must be DbFilter objects.
		/// A logical operator "OR" or "AND" must be provided in order to know if the choices in the
		/// checked box list must be all present ("AND") or if any combination of them is sufficient ("OR").
		/// The value1 field of the object returned is used in SQL queries, the  value2 contains the query
		/// using an user-understandable form.
		/// </summary>
		/// <param name="clb">The checked box list</param>
		/// <param name="logicalOperator">Indicates the combination between the choices</param>
		/// <param name="positiveDataType">The type of result expected to make a selection positive</param>
		/// <returns>A query which can be used in a SQL query</returns>
		public static Query GetFilterFromExtendedCheckedListBox(ExtendedCheckedListBox clb, string logicalOperator, PositiveDataType positiveDataType)
		{
			return GetFilterFromExtendedCheckedListBox(clb, logicalOperator, positiveDataType, null, null);
		}

		/// <summary>
		/// Returns a filter (subquery) reflecting the state of a checked box list, augmented with
		/// additional included or excluded items. All items must be DbFilter objects.
		/// A logical operator "OR" or "AND" must be provided in order to know if the choices in the
		/// checked box list must be all present ("AND") or if any combination of them is sufficient ("OR").
		/// The value1 field of the object returned is used in SQL queries, the  value2 contains the query
		/// using an user-understandable form.
		/// </summary>
		/// <param name="clb">The checked box list</param>
		/// <param name="logicalOperator">Indicates the combination between the choices</param>
		/// <param name="positiveDataType">The type of result expected to make a selection positive</param>
		/// <param name="additionalIncludedItems">Additional included items.</param>
		/// <param name="additionalExcludedItems">Additional excluded items.</param>
		/// <returns>A query which can be used in a SQL query</returns>
		public static Query GetFilterFromExtendedCheckedListBox(ExtendedCheckedListBox clb, string logicalOperator, PositiveDataType positiveDataType, IList<DbFilter> additionalIncludedItems, IList<DbFilter> additionalExcludedItems)
		{
			var result = new Query();

			// included values
			var includedValues = clb.GetItemsByState(CheckState.Checked).ConvertAll<DbFilter>(i => (DbFilter)i);
			if (additionalIncludedItems != null)
				includedValues.AddRange(additionalIncludedItems);
			result += GetFilterFromItems(includedValues, true, logicalOperator, positiveDataType);
			// excluded values
			var excludedValues = clb.GetItemsByState(CheckState.Indeterminate).ConvertAll<DbFilter>(i => (DbFilter)i);
			if (additionalExcludedItems != null)
				excludedValues.AddRange(additionalExcludedItems);
			result += GetFilterFromItems(excludedValues, false, logicalOperator, positiveDataType);

			if (!string.IsNullOrEmpty(result.SqlQuery))
				result.SqlQuery = "(" + result.SqlQuery.Trim() + ")";
			return result;
		}


		public static Query GetFilterFromTextBox(TextBox textBox, ExtendedCheckBox checkBox, string column)
		{
			if (checkBox.CheckState == CheckState.Unchecked) // no filter to apply
				return new Query();

			string[] filters = textBox.Text.Split(';');
			List<string> included; // items that must appear in the query result
			List<string> excluded; // items that must not appear in the query result

			// add each filter to the right list, depending on the general mode defined by the checkbox
			// and on each individual "+" or "-" modifier
			GetIncludedAndExcluded(filters, out included, out excluded, checkBox.CheckState == CheckState.Indeterminate);
			return BuildIncludeAndExcludeQueryFromLists(included, excluded, column);
		}

		private static void GetIncludedAndExcluded(string[] filters, out List<string> included, out List<string> excluded, bool reversed)
		{
			included = new List<string>();
			excluded = new List<string>();
			// REMARK: trailing space are removed.
			// TODO: leave trailing space between quotes?
			for (var i = 0; i < filters.Length; ++i)
			{
				string current = filters[i].Trim();
				if (string.IsNullOrEmpty(current))
					continue;

				bool currentReversed = reversed;
				if (current.StartsWith("+") || current.StartsWith("-"))
				{
					currentReversed = current.StartsWith("-") ^ reversed;
					current = current.Substring(1).Trim(); // remove "+" or "-";
				}

				current = EscapeSqlCharacters(current, false);
				if (!currentReversed)
					included.Add(current);
				else
					excluded.Add(current);
			}
		}

		private static Query GetFilterFromItems(IList<DbFilter> items, bool include, string logicalOperator, PositiveDataType positiveDataType)
		{
			var result = new Query();
			string prefix = include ? "" : "NOT ";
			string expectedYes = include ? "YES" : "NO";
			if (!include)
				logicalOperator = "AND";

			for (var i = 0; i < items.Count; ++i)
			{
				var filter = items[i];

				string subresult = "";
				switch (positiveDataType)
				{
					case PositiveDataType.LikeValue: subresult = String.Format(" ({0}{1} LIKE '%{2}%') ", prefix, filter.Column, EscapeSqlCharacters(filter.ShortName)); break;
					case PositiveDataType.ExactValue: subresult = String.Format(" ({0}{1} = {2}) ", prefix, filter.Column, EscapeSqlCharacters(filter.ShortName)); break;
					case PositiveDataType.Integer: subresult = String.Format(" ({0}NOT(ISNULL({1}))) ", prefix, filter.Column); break;
					case PositiveDataType.Yes: subresult = String.Format(" ({0} = {1}) ", filter.Column, expectedYes); break;
					case PositiveDataType.KeywordValue:
						string value = filter.ToString();
						// treat ellipsis
						// treat X such as in "House X character only"
						var words = EscapeSqlCharacters(value).Split(' ');
						words = words.Select(w => w == "X" ? "%" : w).ToArray();
						value = String.Join(" ", words);

						int pos = Math.Max(value.IndexOf("..."), value.IndexOf('…'));
						if (pos == -1) // no ellipsis, keep the value as it is
							value = String.Format("%{0}.%", value);
						if (pos != -1) // ellipsis, remove the last word and "..." or "…"
						{
							value = value.Substring(0, value.Substring(0, pos).LastIndexOf(' '));
							value = String.Format("%{0}%.%", value);
						}

						subresult = String.Format(" ({0}{1} LIKE '{2}') ", prefix, filter.Column, value); break;
					case PositiveDataType.TriggerValue: subresult = String.Format(" ({0}{1} LIKE '%{2}%') ", prefix, filter.Column, EscapeSqlCharacters(filter + ": ")); break;
				}
				if (!include)
					subresult = String.Format(" (({0} IS NULL) OR {1})", filter.Column, subresult);
				result.SqlQuery += subresult + " " + logicalOperator;

				// human part of the query
				result.HumanQuery += ((include) ? "" : "-") + filter + " ";
			}
			if (!string.IsNullOrEmpty(result.SqlQuery))
				result.SqlQuery = result.SqlQuery.Substring(0, result.SqlQuery.LastIndexOf(' ')); // remove the last logical operator
			return result;
		}

		/// <summary>
		/// Build a query with items that must appear and items that must not appear.
		/// </summary>
		/// <param name="included">the list of items that must appear</param>
		/// <param name="excluded">the list of items that must not appear</param>
		/// <param name="column">the column on which the filter is applied</param>
		/// <returns></returns>
		private static Query BuildIncludeAndExcludeQueryFromLists(IList<string> included, IList<string> excluded, string column)
		{
			var result = new Query();

			for (var i = 0; i < included.Count; ++i)
			{
				result.SqlQuery += String.Format(" ({0} LIKE '%{1}%') AND", column, included[i]);
				result.HumanQuery += "+" + included[i] + " "; // human form of the query
			}
			for (var i = 0; i < excluded.Count; ++i)
			{
				result.SqlQuery += String.Format(" (NOT {0} LIKE '%{1}%') AND", column, excluded[i]);
				result.HumanQuery += "-" + excluded[i] + " "; // human form of the query
			}

			if (!string.IsNullOrEmpty(result.SqlQuery))
			{
				result.SqlQuery = result.SqlQuery.Substring(0, result.SqlQuery.LastIndexOf(' ')); // remove the last logical operator
				result.SqlQuery = "(" + result.SqlQuery.Trim() + ")";
				result.HumanQuery = column + " " + result.HumanQuery; // human form of the query
			}

			return result;
		}
	}

	public enum PositiveDataType { Unknown, LikeValue, ExactValue, Yes, Integer, KeywordValue, TriggerValue };
}
