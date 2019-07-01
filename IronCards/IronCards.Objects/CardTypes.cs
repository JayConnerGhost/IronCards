using System.Drawing;

namespace IronCards.Objects
{
    public enum CardTypes
    {
        Idea,
        Requirement,
        Bug,
        ExternalRequirement
    }
    public static class CardTypesUtilities
    {
        public static Color GetColor(CardTypes cardType)
        {
            var returnValue=Color.AliceBlue;

            switch (cardType)
            {
                case CardTypes.Idea:
                    returnValue=Color.CornflowerBlue;
                    break;
                case CardTypes.Bug:
                    returnValue = Color.DarkRed;
                    break;
                case CardTypes.ExternalRequirement:
                    returnValue=Color.Chartreuse;
                    break;
                case CardTypes.Requirement:
                    returnValue = Color.DarkOrchid;
                    break;
            }

            return returnValue;
        }
    }
}