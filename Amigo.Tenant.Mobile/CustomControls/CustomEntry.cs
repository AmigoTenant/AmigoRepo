using System;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.CustomControls
{
    public class CustomEntry : Entry
    {
        public int MaxLength { get; set; } = 20;
        public bool UppercaseOnly { get; set; } = true;
        public readonly uint AnimationTime = 175;
        public Label PlaceholderLabel { get; set; }

        public CustomEntry()
        {
            this.Focused += OnCustomEntryFocused;
            this.Unfocused += OnCustomEntryUnfocused;
            this.TextChanged += OnCustomEntryTextChanged;
            this.SizeChanged += OnCustomEntrySizeChanged;
        }

        private void OnCustomEntrySizeChanged(object sender, EventArgs e)
        {
            if (this.PlaceholderLabel == null) return;
            if (!string.IsNullOrWhiteSpace(this.Text) && !this.IsFocused) //If it's not empty and it's not focused
            {
                //Traslade the PlaceholderBind to its correct Y position
                var oldBounds = this.PlaceholderLabel.Bounds;
                var newBounds = new Rectangle(new Point(new Size(oldBounds.X, this.Y - this.PlaceholderLabel.Height - this.PlaceholderLabel.Margin.Bottom)), oldBounds.Size);
                this.PlaceholderLabel.LayoutTo(newBounds, this.AnimationTime, Easing.SpringOut);
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                //When the Y axis changes
                case "Y":
                case "Text":
                    if (this.PlaceholderLabel == null) break;
                    if (!string.IsNullOrWhiteSpace(this.Text) && !this.IsFocused) //If it's not empty and it's not focused
                    {
                        //Traslade the PlaceholderBind to its correct Y position
                        var oldBounds = this.PlaceholderLabel.Bounds;
                        var newBounds = new Rectangle(new Point(new Size(oldBounds.X, this.Y - this.PlaceholderLabel.Height - this.PlaceholderLabel.Margin.Bottom)), oldBounds.Size);
                        this.PlaceholderLabel.LayoutTo(newBounds, this.AnimationTime, Easing.SpringOut);
                    }
                    break;
            }
        }

        private void OnCustomEntryFocused(object s, FocusEventArgs e)
        {
            var entry = s as CustomEntry;
            if (entry.PlaceholderLabel == null) return; //If it doesn't have a PlaceholderBind, exit

            if (string.IsNullOrWhiteSpace(entry.Text)) //If this entry doesn't have text, make the focus animation
            {
                var oldBounds = entry.PlaceholderLabel.Bounds;
                var newBounds = new Rectangle(new Point(new Size(oldBounds.X, entry.Y - entry.PlaceholderLabel.Height - entry.PlaceholderLabel.Margin.Bottom)), oldBounds.Size);
                entry.PlaceholderLabel.LayoutTo(newBounds, entry.AnimationTime, Easing.SpringOut);
            }
            //whenever you focus on this entry, set the highlight for the placeholder
            entry.PlaceholderLabel.TextColor = (Color)Xamarin.Forms.Application.Current.Resources["ColorActiveFormInput"];
        }

        private void OnCustomEntryUnfocused(object s, FocusEventArgs e)
        {
            var entry = s as CustomEntry;
            if (entry.PlaceholderLabel == null) return; //If it doesn't have a PlaceholderBind, exit

            if (string.IsNullOrWhiteSpace(entry.Text)) //If this entry doesn't have text, make the unfocus animation
            {
                var oldBounds = entry.PlaceholderLabel.Bounds;
                var newBounds = new Rectangle(new Point(new Size(oldBounds.X, entry.Y + entry.PlaceholderLabel.Margin.Top)), oldBounds.Size);
                entry.PlaceholderLabel.LayoutTo(newBounds, entry.AnimationTime, Easing.SpringOut);
                entry.PlaceholderLabel.TextColor = (Color)Xamarin.Forms.Application.Current.Resources["ColorPlaceholderText"];
            }
            else //If this entry has text, set the placeholder color to normal
                entry.PlaceholderLabel.TextColor = (Color)Xamarin.Forms.Application.Current.Resources["ColorBodyText"];
        }

        private void OnCustomEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue?.Length > MaxLength)
                ((Entry) sender).Text = e.OldTextValue;
            else
                ((Entry) sender).Text = UppercaseOnly ? e.NewTextValue?.ToUpper() : e.NewTextValue?.ToString() ;
        }
    }
}
