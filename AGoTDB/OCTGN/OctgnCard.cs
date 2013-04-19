using System.Diagnostics;
using GenericDB.OCTGN;

namespace AGoTDB.OCTGN
{
	[DebuggerDisplay("{Name}")]
	public class OctgnCard
	{
		[OctgnCardData(AttributeName = "id")]
		public string Id { get; set; }

		[OctgnCardData(AttributeName = "name")]
		public string Name { get; set; }

		[OctgnCardData(PropertyName = "House")]
		public string House { get; set; }

		[OctgnCardData(PropertyName = "Type")]
		public string Type { get; set; }

		[OctgnCardData(PropertyName = "Traits")]
		public string Traits { get; set; }

		[OctgnCardData(PropertyName = "Text")]
		public string Text { get; set; }

		[OctgnCardData(PropertyName = "Keywords")]
		public string Keywords { get; set; }

		[OctgnCardData(PropertyName = "Unique")]
		public string Unique { get; set; }

		[OctgnCardData(PropertyName = "Cost")]
		public string Cost { get; set; }

		[OctgnCardData(PropertyName = "Strength")]
		public string Strength { get; set; }

		[OctgnCardData(PropertyName = "Military")]
		public string Military { get; set; }

		[OctgnCardData(PropertyName = "Intrigue")]
		public string Intrigue { get; set; }

		[OctgnCardData(PropertyName = "Power")]
		public string Power { get; set; }

		[OctgnCardData(PropertyName = "Crest")]
		public string Crest { get; set; }

		[OctgnCardData(PropertyName = "GoldIncome")]
		public string GoldIncome { get; set; }

		[OctgnCardData(PropertyName = "Influence")]
		public string Influence { get; set; }

		[OctgnCardData(PropertyName = "Initiative")]
		public string Initiative { get; set; }

		[OctgnCardData(PropertyName = "PlotGoldIncome")]
		public string PlotGoldIncome { get; set; }

		[OctgnCardData(PropertyName = "PlotInitiative")]
		public string PlotInitiative { get; set; }

		[OctgnCardData(PropertyName = "PlotClaim")]
		public string PlotClaim { get; set; }
	}
}
