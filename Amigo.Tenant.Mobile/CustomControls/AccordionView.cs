using System;
using System.Collections;
using System.Linq;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.CustomControls
{
    public class DefaultTemplate : Grid
    {
        public DefaultTemplate()
        {
            Padding = 5;

            //Grid
            RowSpacing = 0;
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });

            //Labels and text
            var lblLabel = new Label { Style = (Style)Xamarin.Forms.Application.Current.Resources["GeneralLabelText"] };
            var lblDesc = new Label { Style = (Style)Xamarin.Forms.Application.Current.Resources["GeneralLabelTextN"] };

            var lblCommentLabel = new Label { Style = (Style)Xamarin.Forms.Application.Current.Resources["GeneralLabelText"] };
            var lblCommentDesc = new Label { Style = (Style)Xamarin.Forms.Application.Current.Resources["GeneralLabelTextN"] };

            Children.Add(lblLabel, 1, 0);
            Children.Add(lblDesc, 2, 0);
            Children.Add(lblCommentLabel, 1, 1);
            Children.Add(lblCommentDesc, 2, 1);

            lblLabel.SetBinding(Label.TextProperty, "Tags");
            lblDesc.SetBinding(Label.TextProperty, "Desc");
            lblCommentLabel.SetBinding(Label.TextProperty, "CommentTags");
            lblCommentDesc.SetBinding(Label.TextProperty, "CommentDesc");
        }
    }

    public class AccordionView : ScrollView
    {
        public AccordionView()
        {
            var itemTemplate = new DataTemplate(typeof(DefaultTemplate));

            SubTemplate = itemTemplate;
            Template = new DataTemplate(() => (object)(new AccordionSectionView(itemTemplate, this)));
            Content = _layout;

            this.SetBinding(ItemsSourceProperty, "List");
            Template.SetBinding(AccordionSectionView.HeaderProperty, "Header");
            Template.SetBinding(AccordionSectionView.ActionTypeProperty, "ActionType");
            Template.SetBinding(AccordionSectionView.ItemsSourceProperty, "List");
            Template.SetBinding(AccordionSectionView.AcknowledgeStateProperty, "AcknowledgeState");
        }

        private readonly StackLayout _layout = new StackLayout { Spacing = 1 };

        public DataTemplate Template { get; set; }
        public DataTemplate SubTemplate { get; set; }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                "ItemsSource",
                typeof(IList),
                typeof(AccordionSectionView),
                default(IList),
                propertyChanged: PopulateList);

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public AccordionView(DataTemplate itemTemplate)
        {
            SubTemplate = itemTemplate;
            Template = new DataTemplate(() => (object)(new AccordionSectionView(itemTemplate, this)));
            Content = _layout;
        }

        private void PopulateList()
        {

            _layout.Children.Clear();

            foreach (var item in ItemsSource)
            {
                var template = (Xamarin.Forms.View)Template.CreateContent();
                template.BindingContext = item;
                _layout.Children.Add(template);
            }
        }

        private static void PopulateList(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            if (oldValue == newValue) return;
            ((AccordionView)bindable).PopulateList();
        }

    }

    public class AccordionSectionView : StackLayout
    {
        private bool _isExpanded;
        private readonly StackLayout _content = new StackLayout { HeightRequest = 0 };
        private readonly Color _headerColor = (Color)Xamarin.Forms.Application.Current.Resources["ColorBackground"];
        private readonly ImageSource _arrowRight = ImageSource.FromFile("arrow_right.png");
        private readonly Grid _header = new Grid() { Padding = new Thickness(10) };
        private readonly Image _headerIcon = new Image { VerticalOptions = LayoutOptions.Center };


        private readonly Label _headerActionType = new Label
        {
            TextColor = (Color)Xamarin.Forms.Application.Current.Resources["ColorTextLink"],
            FontAttributes = FontAttributes.Bold,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center
        };

        private readonly Label _headerAcknowledgeState = new Label
        {
            TextColor = (Color)Xamarin.Forms.Application.Current.Resources["ColorSuccess"],
            FontAttributes = FontAttributes.Bold,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center
        };

        private readonly Label _headerText = new Label
        {
            TextColor = (Color)Xamarin.Forms.Application.Current.Resources["ColorBodyText"],
            FontAttributes = FontAttributes.Bold,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            VerticalTextAlignment = TextAlignment.Center,
        };
        
        private readonly DataTemplate _template;
        private readonly ScrollView _parent;

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                "ItemsSource",
                typeof(IList),
                typeof(AccordionSectionView),
                default(IList),
                propertyChanged: PopulateList);

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ActionTypeProperty =
            BindableProperty.Create(
                "ActionType",
                typeof(string),
                typeof(AccordionSectionView),
                propertyChanged: ChangeTitle);

        public string ActionType
        {
            get { return (string)GetValue(ActionTypeProperty); }
            set { SetValue(ActionTypeProperty, value); }
        }
        
        public static readonly BindableProperty HeaderProperty =
            BindableProperty.Create(
                "Header",
                typeof(string),
                typeof(AccordionSectionView),
                propertyChanged: ChangeTitle);

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly BindableProperty AcknowledgeStateProperty =
            BindableProperty.Create(
                "AcknowledgeState",
                typeof(string),
                typeof(AccordionSectionView),
                propertyChanged: ChangeTitle);

        public string AcknowledgeState
        {
            get { return (string)GetValue(AcknowledgeStateProperty); }
            set { SetValue(AcknowledgeStateProperty, value); }
        }

        public AccordionSectionView(DataTemplate itemTemplate, ScrollView parent)
        {
            _template = itemTemplate;
            _parent = parent;
            _headerText.BackgroundColor = _headerColor;
            //_headerTitle.BackgroundColor = _headerColor;
            //_headerSubtitle.BackgroundColor = _headerColor;
            _headerIcon.Source = _arrowRight;
            _header.BackgroundColor = _headerColor;

            //Grid
            _header.RowSpacing = 1;
            _header.ColumnSpacing = 1;
            _header.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _header.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _header.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            _header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            _header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            _header.Children.Add(_headerIcon, 0, 0);
            _header.Children.Add(_headerText, 1, 0);

            _header.Children.Add(_headerActionType, 2, 0);
            _header.Children.Add(_headerAcknowledgeState, 2, 1);
            
            Grid.SetRowSpan(_headerIcon, 3);
            Grid.SetRowSpan(_headerText, 3);

            Spacing = 0;
            Children.Add(_header);
            Children.Add(_content);

            var tap = new TapGestureRecognizer();
            tap.Tapped += TapOnTapped;
            _header.GestureRecognizers.Add(tap);

        }

        private double _accordionSectionHeight;
        private async void TapOnTapped(object sender, EventArgs eventArgs)
        {
            if (_accordionSectionHeight <= 0)
                _accordionSectionHeight = _content.Children.Sum(child => child.Height);

            const uint animSpeed = 200;
            if (_isExpanded)
            {
                _headerIcon.RotateTo(0, animSpeed);
                var finishHeight = 0;
                Action<double> callback = input => { _content.HeightRequest = input; };
                _content.Animate("invis", callback, _accordionSectionHeight, finishHeight, 16, animSpeed, Easing.CubicInOut);
                for (var i = _content.Children.Count; i > 0; i--)
                    await _content.Children[i - 1].FadeTo(0, 100);
                _isExpanded = false;
            }
            else
            {
                _headerIcon.RotateTo(90, 150);
                var startHeight = 0;
                Action<double> callback = input => { _content.HeightRequest = input; };
                Action<double, bool> finishAction = async (d, b) => {
                    foreach (var child in _content.Children)
                        await child.FadeTo(1, 100);
                };
                _content.Animate("invis", callback, startHeight, _accordionSectionHeight, 16, animSpeed, Easing.CubicInOut, finishAction);
                _isExpanded = true;

                // Scroll top by the current Y position of the section
                if (_parent.Parent is VisualElement)
                {
                    await _parent.ScrollToAsync(0, Y, true);
                }
            }
        }

        private void ChangeTitle()
        {
            _headerText.Text = Header;
            _headerActionType.Text = ActionType;
            _headerAcknowledgeState.Text = AcknowledgeState;
        }

        private void PopulateList()
        {
            _content.Children.Clear();

            foreach (var item in ItemsSource)
            {
                var template = (Xamarin.Forms.View)_template.CreateContent();
                template.BindingContext = item;
                _content.Children.Add(template);
            }
        }

        private static void ChangeTitle(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;
            ((AccordionSectionView)bindable).ChangeTitle();
        }

        private static void PopulateList(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;
            ((AccordionSectionView)bindable).PopulateList();
        }
    }
}
