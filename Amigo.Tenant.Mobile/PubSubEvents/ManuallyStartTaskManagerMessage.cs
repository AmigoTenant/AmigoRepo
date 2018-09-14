namespace XPO.ShuttleTracking.Mobile.PubSubEvents
{
    public sealed class ManuallyStartTaskManagerMessage
    {
        public const string Name = "RunTasks";
        public static ManuallyStartTaskManagerMessage Empty => new ManuallyStartTaskManagerMessage();
    }
}