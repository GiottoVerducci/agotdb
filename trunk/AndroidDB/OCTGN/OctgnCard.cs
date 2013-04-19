using System.Diagnostics;
using GenericDB.OCTGN;

namespace NRADB.OCTGN
{
	[DebuggerDisplay("{Name}")]
	public class OctgnCard
	{
		[OctgnCardData(AttributeName = "id")]
		public string Id { get; set; }

		[OctgnCardData(AttributeName = "name")]
		public string Name { get; set; }

        [OctgnCardData(PropertyName = "Subtitle")]
        public string Subtitle { get; set; }

        [OctgnCardData(PropertyName = "Type")]
        public string Type { get; set; }

        [OctgnCardData(PropertyName = "Keywords")]
        public string Keywords { get; set; }

        [OctgnCardData(PropertyName = "Cost")]
        public string Cost { get; set; }

        [OctgnCardData(PropertyName = "Requirement")]
        public string Requirement { get; set; }

        [OctgnCardData(PropertyName = "Stat")]
        public string Stat { get; set; }

        [OctgnCardData(PropertyName = "Rules")]
        public string Text { get; set; }

        [OctgnCardData(PropertyName = "Flavor")]
        public string Flavor { get; set; }

        [OctgnCardData(PropertyName = "Instructions")]
        public string Instructions { get; set; }

        [OctgnCardData(PropertyName = "Faction")]
        public string Faction { get; set; }

        [OctgnCardData(PropertyName = "Side")]
        public string Side { get; set; }

        [OctgnCardData(PropertyName = "Influence")]
        public string Influence { get; set; }
	}
}
