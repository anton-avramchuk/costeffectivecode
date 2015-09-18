using JetBrains.Annotations;

namespace CostEffectiveCode.Common.Messaging.Process
{
    [PublicAPI]
    public enum ProcessStatusEnum
    {
        InProgress,

        // not used yet -- here just to show the idea here
        Pending,
        Blocked

        //Start -- special cases having dedicated events, no need to have statuses for them, it could lead to inconsistency
        //Failure,
        //Finished
    }
}