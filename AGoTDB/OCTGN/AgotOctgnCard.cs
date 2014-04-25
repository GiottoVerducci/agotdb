using System.Diagnostics;
using GenericDB.OCTGN;

namespace AGoTDB.OCTGN
{
    [DebuggerDisplay("{Name}")]
    public class AgotOctgnCard : IOctgnCard
    {
        [OctgnCardData(AttributeName = "id")]
        public string Id { get; set; }

        [OctgnCardData(AttributeName = "name")]
        public string Name { get; set; }

        [OctgnCardData(AttributeName = "House")]
        public string House { get; set; }

        [OctgnCardData(AttributeName = "Type")]
        public string Type { get; set; }

        [OctgnCardData(AttributeName = "Traits")]
        public string Traits { get; set; }

        [OctgnCardData(AttributeName = "Text")]
        public string Text { get; set; }

        [OctgnCardData(AttributeName = "Keywords")]
        public string Keywords { get; set; }

        [OctgnCardData(AttributeName = "Unique")]
        public string Unique { get; set; }

        [OctgnCardData(AttributeName = "Cost")]
        public string Cost { get; set; }

        [OctgnCardData(AttributeName = "Strength")]
        public string Strength { get; set; }

        [OctgnCardData(AttributeName = "Military")]
        public string Military { get; set; }

        [OctgnCardData(AttributeName = "Intrigue")]
        public string Intrigue { get; set; }

        [OctgnCardData(AttributeName = "Power")]
        public string Power { get; set; }

        [OctgnCardData(AttributeName = "Crest")]
        public string Crest { get; set; }

        [OctgnCardData(AttributeName = "GoldIncome")]
        public string GoldIncome { get; set; }

        [OctgnCardData(AttributeName = "Influence")]
        public string Influence { get; set; }

        [OctgnCardData(AttributeName = "Initiative")]
        public string Initiative { get; set; }

        [OctgnCardData(AttributeName = "PlotGoldIncome")]
        public string PlotGoldIncome { get; set; }

        [OctgnCardData(AttributeName = "PlotInitiative")]
        public string PlotInitiative { get; set; }

        [OctgnCardData(AttributeName = "PlotClaim")]
        public string PlotClaim { get; set; }

        public bool Equals(AgotOctgnCard other)
        {
            return this.Name == other.Name
               && this.House == other.House
               && this.Type == other.Type
               && this.Traits == other.Traits
               && this.Text == other.Text
               && this.Keywords == other.Keywords
               && this.Unique == other.Unique
               && this.Cost == other.Cost
               && this.Strength == other.Strength
               && this.Military == other.Military
               && this.Intrigue == other.Intrigue
               && this.Power == other.Power
               && this.Crest == other.Crest
               && this.GoldIncome == other.GoldIncome
               && this.Influence == other.Influence
               && this.Initiative == other.Initiative
               && this.PlotGoldIncome == other.PlotGoldIncome
               && this.PlotInitiative == other.PlotInitiative
               && this.PlotClaim == other.PlotClaim;
        }
    }
}