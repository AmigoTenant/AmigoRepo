using System.Collections.Generic;
using XPO.ShuttleTracking.Mobile.Entity.Tasks.Abstract;

namespace XPO.ShuttleTracking.Mobile.Model
{
    public class PendentTaskGroup : List<TaskDefinition>
    {
        public PendentTaskGroup(IEnumerable<TaskDefinition> items) : base(items)
        {
        }
        public PendentTaskGroup()
        {
        }
        public string ExternalMoveId { get; set; }
    }
}
