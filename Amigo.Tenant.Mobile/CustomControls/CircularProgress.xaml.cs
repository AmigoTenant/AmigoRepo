using System;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Helpers.Timing;

namespace XPO.ShuttleTracking.Mobile.CustomControls
{
    public partial class CircularProgress : ContentView
    {
        public static readonly BindableProperty LabelProperty = BindableProperty.Create("Label",typeof(string),typeof(CircularProgress),string.Empty,BindingMode.OneWay, propertyChanged: OnTextUpdated);

        private static void OnTextUpdated(BindableObject bindable, object oldValue, object newValue)
        {
            var circularProgress = bindable as CircularProgress;
            circularProgress?.SetText(newValue as string);
        }

        public CircularProgress()
        {
            InitializeComponent();            
        }
        public void Start()
        {
            var borderColor = ShapeProgress.BorderColor;
            var shaperColor = ShapeProgress.ProgressBorderColor;

            var rotation = new Animation(callback: progress =>
            {
                if (progress > 100)
                {
                    ShapeProgress.BorderColor = shaperColor;
                    ShapeProgress.ProgressBorderColor = borderColor;
                }

                if (ShapeProgress.BorderColor != borderColor)
                {
                    ShapeProgress.BorderColor = borderColor;
                    ShapeProgress.ProgressBorderColor = shaperColor;
                }
                ShapeProgress.Progress = (int)progress;
            },
                start: 0,
                end: 105,
                easing: Easing.CubicInOut);

            rotation.Commit(ShapeProgress, "Loop", length: 2000, repeat: () => true);
        }

        protected internal void SetText(string text)
        {
            TextLabel.Text = text;
        }

        public void Stop()
        {
            ShapeProgress.AbortAnimation("Loop");            
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set {
                SetValue(LabelProperty,value);
                TextLabel.Text = value;
            }
        }
    }
}
