namespace CostEffectiveCode.Akka.Messages
{
    public class FetchRequestMessage
    {
        public bool Single { get; set; }

        public int? Page { get; set; }

        public int? Limit { get; set; }
    }
}