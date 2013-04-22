using System.Drawing;

namespace GenericDB.BusinessObjects
{
    public class CardNodeInfo : StringPair
    {
        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }

        public CardNodeInfo(string value1, string value2)
            : base(value1, value2)
        {
            ForeColor = Color.Empty;
            BackColor = Color.Empty;
        }
    }
}
