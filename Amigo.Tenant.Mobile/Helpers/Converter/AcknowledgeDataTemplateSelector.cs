using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Entity;

namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
    public class AcknowledgeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MoveTemplate { get; set; }
        public DataTemplate ServiceTemplate { get; set; }
        public DataTemplate DetentionTemplate { get; set; }
        public DataTemplate OperateTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (((BEAcknowledgeMove)item).TypeSelector)
            {
                case AcknowledgeType.Move: return MoveTemplate;
                case AcknowledgeType.Service: return ServiceTemplate;
                case AcknowledgeType.Detention: return DetentionTemplate;
                case AcknowledgeType.OperateTaylor: return OperateTemplate;
                default: return MoveTemplate;
            }
        }
    }
}
