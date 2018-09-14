using System.Collections.Generic;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.Helpers.Behaviors
{
    public class ToolbarOnPlatformBehavior: Behavior<ContentPage>
    {
        public ToolbarOnPlatformBehavior()
        {
            this.Android = new List<ToolbarItem>();
            this.WinPhone = new List<ToolbarItem>();
            this.iOS = new List<ToolbarItem>();
        }
        /// <value>To be added.</value>
        /// <remarks>To be added.</remarks>
        public List<ToolbarItem> Android { get; set; }

        /// <summary>The type as it is implemented on the iOS platform.</summary>
        /// <value>To be added.</value>
        /// <remarks>To be added.</remarks>
        public List<ToolbarItem> iOS { get; set; }

        /// <summary>The type as it is implemented on the WinPhone platform.</summary>
        /// <value>To be added.</value>
        /// <remarks>To be added.</remarks>
        public List<ToolbarItem> WinPhone { get; set; }
        
        protected override void OnAttachedTo(ContentPage bindable)
        {
            if (bindable == null) return;

            List<ToolbarItem> items;
            switch (Device.OS)
            {
                case TargetPlatform.iOS:
                    items = this.iOS;
                    break;
                case TargetPlatform.Android:
                    items  = Android;
                    break;
                case TargetPlatform.WinPhone:
                case TargetPlatform.Windows:
                    items = WinPhone;
                    break;
                default:
                    items = Android;
                    break;
            }            

            foreach (var item in items)
            {
                bindable.ToolbarItems.Add(item);
            }
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            bindable?.ToolbarItems.Clear();
        }
    }
}
