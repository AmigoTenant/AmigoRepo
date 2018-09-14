namespace XPO.ShuttleTracking.Domain.QueryModel.Tracking
{
    public class MoveQueryModel
    {
        public int Id { get; set; }
        public MoveStatus Status { get; set; }        
    }

    public enum MoveStatus
    {
        Pendent,
        Finished
    }
}
