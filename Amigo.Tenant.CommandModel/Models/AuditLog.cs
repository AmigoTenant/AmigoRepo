namespace Amigo.Tenant.CommandModel.Models
{
    public class AuditLog
    {
        public int ID { get; set; }
        public string Command { get; set; }
        public string PostTime { get; set; }
        public string HostName { get; set; }
        public string LoginName { get; set; }
    }
}