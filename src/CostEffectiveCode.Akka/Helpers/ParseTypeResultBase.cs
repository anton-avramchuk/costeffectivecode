namespace CostEffectiveCode.Akka.Helpers
{
    public abstract class ParseTypeResultBase
    {
        protected ParseTypeResultBase(bool isSubclass)
        {
            IsSubclass = isSubclass;
        }

        public bool IsSubclass { get; private set; }
    }
}