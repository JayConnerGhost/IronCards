using System;
using System.Windows.Forms;

namespace IronCards.Controls
{
    public class Card:UserControl
    {
        public int ParentLaneId { get; }
        public string CardName { get; }
        public string CardDescription { get; }
        public int CardPoints { get; }

        public Card(int parentLaneId, string cardName, string cardDescription, int cardPoints)
        {
            ParentLaneId = parentLaneId;
            CardName = cardName;
            CardDescription = cardDescription;
            CardPoints = cardPoints;
            throw new NotImplementedException();
        }
    }
}