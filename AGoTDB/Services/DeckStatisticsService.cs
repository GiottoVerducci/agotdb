// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010, 2011 Vincent Ripoll
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
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// © A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl
// © Le Trône de Fer JCE 2008 Edge Entertainment

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AGoTDB.BusinessObjects;

namespace AGoTDB.Services
{
	public class DeckStatisticsService
	{
		public static Dictionary<string, int> GetTraitsDistribution(AgotCard.CardType cardType, AgotCardList cardList)
		{
			var result = new Dictionary<string, int>();

			foreach (var c in cardList)
			{
				if (c.Type == null || c.Type.Value != (int)cardType || c.Traits == null || String.IsNullOrEmpty(c.Traits.Value))
					continue;

				foreach (var t in c.Traits.Value.Split('.'))
				{
					var current = t.Trim();
					if (current == "")
						continue;
					if (result.ContainsKey(current))
						result[current] += c.Quantity;
					else
						result.Add(current, c.Quantity);
				}
			}
			return result;
		}

		public static Dictionary<string, int> GetCrestsDistribution(AgotCard.CardType cardType, AgotCardList cardList)
		{
			var result = new Dictionary<string, int> { 
				{ Resource1.WarVirtueText, 0 }, 
				{ Resource1.HolyVirtueText, 0 }, 
				{ Resource1.LearnedVirtueText, 0 }, 
				{ Resource1.NobleVirtueText, 0 }, 
				{ Resource1.ShadowVirtueText, 0 }
			};

			foreach (var c in cardList)
			{
				if (c.Type == null || c.Type.Value != (int)cardType)
					continue;
				if (c.War != null && c.War.Value)
					result[Resource1.WarVirtueText] += c.Quantity;
				if (c.Holy != null && c.Holy.Value)
					result[Resource1.HolyVirtueText] += c.Quantity;
				if (c.Learned != null && c.Learned.Value)
					result[Resource1.LearnedVirtueText] += c.Quantity;
				if (c.Noble != null && c.Noble.Value)
					result[Resource1.NobleVirtueText] += c.Quantity;
				if (c.Shadow != null && c.Shadow.Value)
					result[Resource1.ShadowVirtueText] += c.Quantity;
			}

			return (from kv in result
					where kv.Value > 0
					select kv).ToDictionary(kv => kv.Key, kv => kv.Value);
		}

		public static Dictionary<string, int> GetIconsDistribution(AgotCardList cardList)
		{
			var result = new Dictionary<string, int>();

			var icons = new string[3];

			foreach (var c in cardList)
			{
				if (c.Type == null || c.Type.Value != (int)AgotCard.CardType.Character)
					continue;
				bool hasMil = c.Military != null && c.Military.Value;
				bool hasInt = c.Intrigue != null && c.Intrigue.Value;
				bool hasPow = c.Power != null && c.Power.Value;

				int i = 0;
				if (hasMil) icons[i++] = Resource1.MilitaryIconAbrev;
				if (hasInt) icons[i++] = Resource1.IntrigueIconAbrev;
				if (hasPow) icons[i++] = Resource1.PowerIconAbrev;

				var key = i > 0
					? String.Join(" ", icons, 0, i)
					: "-";

				if (result.ContainsKey(key))
					result[key] += c.Quantity;
				else
					result.Add(key, c.Quantity);
			}

			return result;
		}

		public static Dictionary<string, int> GetHousesDistribution(AgotCard.CardType cardType, AgotCardList cardList)
		{
			var result = new Dictionary<string, int>();

			var housesLists = new List<string>();

			foreach (var c in cardList)
			{
				if (c.Type == null || c.Type.Value != (int)cardType || c.House == null || c.House.Value == (int)AgotCard.CardHouse.Neutral)
					continue;

				var house = AgotCard.GetHouseName(c.House.Value);

				if (result.ContainsKey(house))
					result[house] += c.Quantity;
				else
					result.Add(house, c.Quantity);
			}

			return result;
		}

		public static Dictionary<int, int> GetCardInfluenceCosts(AgotCardList cardList)
		{
			var result = new Dictionary<int, int>();
			Regex reg = new Regex(AgotCard.CardPatterns[AgotCard.Pattern.CostsInfluence], RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
			foreach (var c in cardList)
			{
				var m = reg.Matches(c.Text.Value);
				if (m.Count > 0)
				{
					var stringValue = m[0].Groups[1].ToString();
					var value = Convert.ToInt32(stringValue == "X" ? "-1" : stringValue); // gets the influence cost
					if (result.ContainsKey(value))
						result[value] += c.Quantity;
					else
						result[value] = c.Quantity;
				}
			}
			return result;
		}

		public static Dictionary<string, int> GetIconsStrength(AgotCardList cardList)
		{
			var result = new Dictionary<string, int>();
			result.Add(Resource1.MilitaryIconAbrev, 0);
			result.Add(Resource1.IntrigueIconAbrev, 0);
			result.Add(Resource1.PowerIconAbrev, 0);

			foreach (var c in cardList)
			{
				if (c.Type == null || c.Type.Value != (int)AgotCard.CardType.Character)
					continue;
				bool hasMil = c.Military != null && c.Military.Value;
				bool hasInt = c.Intrigue != null && c.Intrigue.Value;
				bool hasPow = c.Power != null && c.Power.Value;

				var strength = c.Strength.Value.IsX ? 0 : c.Strength.Value.Value;
				if (hasMil) result[Resource1.MilitaryIconAbrev] += strength * c.Quantity;
				if (hasInt) result[Resource1.IntrigueIconAbrev] += strength * c.Quantity;
				if (hasPow) result[Resource1.PowerIconAbrev] += strength * c.Quantity;
			}

			return result;
		}
	}
}
