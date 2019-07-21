namespace IronCards.Services
{
    public class CardDocument
    {
        public int Id { get; set; }
        public int ParentLaneId { get; set; }
        public string CardName { get; set; }
        public string CardDescription { get; set; }
        public int CardPoints { get; set; }
        public string CardType { get; set; }
        public int FeatureId { get; set; }
    }
}