using System;

namespace CostEffectiveCode.Akka.Messages
{
    public class FetchRequestMessageBase
    {
        public FetchRequestMessageBase(bool single, bool firstOrDefault)
        {
            if (single && firstOrDefault)
                throw new ArgumentException("Cannot fetch both single and firstOrDefault at the same time",
                    nameof(firstOrDefault));

            Single = single;
            FirstOrDefault = firstOrDefault;

            Page = null;
            Limit = null;
        }

        public FetchRequestMessageBase(int page, int limit)
        {
            Single = false;
            FirstOrDefault = false;

            Page = page;
            Limit = limit;
        }

        public FetchRequestMessageBase(int limit)
        {
            Single = false;
            FirstOrDefault = false;

            Page = null;
            Limit = limit;
        }

        public FetchRequestMessageBase()
        {
            // Fetch all

            Single = false;
            FirstOrDefault = false;

            Page = null;
            Limit = null;
        }

        public bool Single { get; protected set; }

        public bool FirstOrDefault { get; protected set; }

        public int? Page { get; protected set; }

        public int? Limit { get; protected set; }

    }
}
