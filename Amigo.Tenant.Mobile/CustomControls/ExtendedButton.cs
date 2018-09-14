using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.CustomControls
{
    /// <summary>
    /// Extends <see cref="Xamarin.Forms.Button"/>.
    /// </summary>
    public class ExtendedButton : Button
    {
        /// <summary>
        /// Bindable property for button content vertical alignment.
        /// </summary>
        public static readonly BindableProperty VerticalContentAlignmentProperty = BindableProperty.Create(nameof(VerticalContentAlignment),typeof(TextAlignment), typeof(ExtendedButton), TextAlignment.Center);

        /// <summary>
        /// Bindable property for button content horizontal alignment.
        /// </summary>
        public static readonly BindableProperty HorizontalContentAlignmentProperty = BindableProperty.Create(nameof(HorizontalContentAlignment), typeof(TextAlignment), typeof(ExtendedButton),TextAlignment.Center);
           
        /// <summary>
        /// Gets or sets the content vertical alignment.
        /// </summary>
        public TextAlignment VerticalContentAlignment
        {
            get { return (TextAlignment) GetValue(VerticalContentAlignmentProperty); }
            set { this.SetValue(VerticalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content horizontal alignment.
        /// </summary>
        public TextAlignment HorizontalContentAlignment
        {
            get { return (TextAlignment) GetValue(HorizontalContentAlignmentProperty); }
            set { this.SetValue(HorizontalContentAlignmentProperty, value); }
        }
    }
}
