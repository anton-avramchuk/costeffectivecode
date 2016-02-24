namespace CostEffectiveCode.Akka.Messages
{
    public class FetchRequestMessage
    {
        public FetchRequestMessage(bool single)
        {
            Single = single;
            Page = null;
            Limit = null;
        }

        public FetchRequestMessage(int page, int limit)
        {
            Single = false;
            Page = page;
            Limit = limit;
        }

        public FetchRequestMessage(int limit)
        {
            Single = false;
            Page = null;
            Limit = limit;
        }

        public bool Single { get; set; }

        public int? Page { get; set; }

        public int? Limit { get; set; }
    }
}