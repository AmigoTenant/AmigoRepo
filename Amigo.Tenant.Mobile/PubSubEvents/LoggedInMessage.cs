
namespace XPO.ShuttleTracking.Mobile.PubSubEvents
{
    public sealed class LoggedInMessage
    {
        public const string Name = "LoggedIn";
        public static LoggedInMessage Empty => new LoggedInMessage();
    }

    public sealed class LoggedOutMessage
    {
        public const string Name = "LoggedOut";
        public static LoggedOutMessage Empty => new LoggedOutMessage();
    }
}
