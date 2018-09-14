using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TSI.Xamarin.Forms.Mvvm.ViewModels;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.View.Abstract
{
    public abstract class NavigatingPage: ContentPage
    {        
        protected override void OnAppearing()
        {
            var vm = BindingContext as ViewModelBase;
            vm?.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            var vm = BindingContext as ViewModelBase;
            vm?.OnDisappearing();
            ////if (Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Contains(this)) return;
            ////this.Behaviors.Clear();
            ////this.Content = null;
            ////this.BindingContext = null;            
            ////ShuttleNavigationStack.OrphanPages.Enqueue(this);            
            //Task.Run(async () =>
            //{
            //    await Task.Delay(2000);
            //    ShuttleNavigationStack.CleanOrphanPage(this);
            //    GC.Collect(GC.MaxGeneration,GCCollectionMode.Optimized,blocking:false);
            //});
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }

    //public static class ShuttleNavigationStack
    //{
    //    public static void CleanOrphanPage(Page page)
    //    {
    //        if (!App.Current.MainPage.Navigation.NavigationStack.Contains(page))
    //        {
    //            var contentPage = page as ContentPage;
    //            var vm = page.BindingContext as IDisposable;                
    //            if (contentPage != null) contentPage.Content = null;
    //            page.Behaviors.Clear();
    //            vm?.Dispose();
    //            page.BindingContext = null;
    //        }
    //    }

    //    public static Queue<Page> OrphanPages = new Queue<Page>();

    //    public static void CleanOrphanPages()
    //    {
    //        var pagesCount = OrphanPages.Count;
    //        for (int i = 0; i < pagesCount; i++)
    //        {
    //            var page = OrphanPages.Peek();
    //            if (page == null) break;
    //            if (!App.Current.MainPage.Navigation.NavigationStack.Contains(page))
    //            {
    //                var contentPage = page as ContentPage;
    //                if (contentPage != null) contentPage.Content = null;
    //                page.Behaviors.Clear();
    //                page.BindingContext = null;
    //                OrphanPages.Dequeue();
    //            }
    //        }

    //        //foreach (var page in OrphanPages)
    //        //{
    //        //    if (!App.Current.MainPage.Navigation.NavigationStack.Contains(page))
    //        //    {
    //        //        var contentPage = page as ContentPage;
    //        //        if (contentPage != null) contentPage.Content = null;
    //        //        page.Behaviors.Clear();
    //        //        page.BindingContext = null;
    //        //        OrphanPages.Remove(page);
    //        //    }
    //        //}
    //    }
    //}
    ////public class ShuttleNavigationpage : NavigationPage
    ////{
    ////    //public ShuttleNavigationpage():base()
    ////    //{
    ////    //    this.Popped += ShuttleNavigationpage_Popped;
    ////    //}

    ////    //private void ShuttleNavigationpage_Popped(object sender, NavigationEventArgs e)
    ////    //{
    ////    //    Debug.WriteLine("hi");
    ////    //}

    ////    //public ShuttleNavigationpage(Page page):base(page)
    ////    //{
    ////    //    this.Popped += ShuttleNavigationpage_Popped;
    ////    //}

    ////    public ShuttleNavigationpage()
    ////    {
    ////        this.PropertyChanged += OnPropertyChanged;
    ////    }

    ////    public ShuttleNavigationpage(Page page) : base(page)
    ////    {
    ////        this.PropertyChanged += OnPropertyChanged;
    ////    }

    ////    private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    ////    {
    ////        if (propertyChangedEventArgs.PropertyName == "MainPage")
    ////        {
    ////            Debug.WriteLine("Its time to check if I can clean");
    ////        }
    ////    }
    ////}
}