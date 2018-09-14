using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TSI.Xamarin.Forms.Mvvm.ViewModels;
using TSI.Xamarin.Forms.Mvvm.Views.Abstract;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.View.Abstract;

namespace XPO.ShuttleTracking.Mobile.Navigation
{
    /// <summary>
    /// Wrapper around INavigation
    /// </summary>
    public class Navigator : INavigator
    {
        private readonly Lazy<INavigation> _navigation;
        private readonly IViewFactory _viewFactory;        

        public Navigator(Lazy<INavigation> navigation, IViewFactory viewFactory)
        {
            _navigation = navigation;
            _viewFactory = viewFactory;
        }

        private INavigation Navigation => _navigation.Value;        

        public void ClearNavigationStack()
        {
            try
            {
                var existingPages = Navigation.NavigationStack.ToList();
                for (var i = 0; i < existingPages.Count - 1; i++)
                {
                    var page = existingPages[i];
                    Navigation.RemovePage(page);
                    if (page is IPersistentView)continue;                    
                    page.Behaviors?.Clear();                    
                    var vm = page.BindingContext;
                    (vm as IDisposable)?.Dispose();
                    var contentPage = page as ContentPage;
                    if (contentPage != null) contentPage.Content = null;
                    page.BindingContext = null;
                }
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: false);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Cannot destroy pages. - {e}");                
            }
        }

        public void ClearNavigationStackToRoot()
        {
            try
            {
                var existingPages = Navigation.NavigationStack.ToList();
                for (var i = 1; i < existingPages.Count - 1; i++)
                {
                    var page = existingPages[i];
                    Navigation.RemovePage(page);

                    if (page is IPersistentView) continue;                    

                    //kill page
                    page.Behaviors?.Clear();
                    var vm = page.BindingContext;
                    (vm as IDisposable)?.Dispose();
                    var contentPage = page as ContentPage;
                    if (contentPage != null) contentPage.Content = null;
                    page.BindingContext = null;
                }
                GC.Collect(GC.MaxGeneration,GCCollectionMode.Optimized,blocking:false);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void ClearNavigationStackToRoot(bool removePersistentPages)
        {
            try
            {
                var existingPages = Navigation.NavigationStack.ToList();
                for (var i = 1; i < existingPages.Count - 1; i++)
                {
                    var page = existingPages[i];                   
                    Navigation.RemovePage(page);

                    if (page is IPersistentView) continue;
                    //kill page
                    page.Behaviors?.Clear();
                    var vm = page.BindingContext;
                    (vm as IDisposable)?.Dispose();
                    var contentPage = page as ContentPage;
                    if (contentPage != null) contentPage.Content = null;
                    page.BindingContext = null;
                }
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: false);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async void RemoveLastPageFromStack()
        {
            var lastPage = Navigation.NavigationStack.Count -1;
            if (lastPage < 0) return;

            var previousPage = Navigation.NavigationStack[lastPage];
            if (previousPage is IPersistentView)
            {
                await Navigation.PopAsync();
                return;
            }

            await Navigation.PopAsync();

            previousPage.Behaviors?.Clear();            
            var vm = previousPage.BindingContext;
            (vm as IDisposable)?.Dispose();
            var contentPage = previousPage as ContentPage;
            if (contentPage != null) contentPage.Content = null;
            previousPage.BindingContext = null;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: false);
        }

        public async Task<IViewModel> PopAsync()
        {            
            var view = await Navigation.PopAsync();
            var vm = view.BindingContext as IViewModel;
            vm?.OnPopped();
            
            //Kill page
            view.Behaviors?.Clear();            
            (view.BindingContext as IDisposable)?.Dispose();
            var contentPage = view as ContentPage;
            if (contentPage != null) contentPage.Content = null;
            view.BindingContext = null;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: false);

            return vm;
        }

        public async Task<IViewModel> PopModalAsync()
        {            
            var view = await Navigation.PopModalAsync();
            var vm =view.BindingContext as IViewModel;
            vm?.OnPopped();         
            return vm;
        }

        public async Task PopToRootAsync()
        {
            ClearNavigationStackToRoot();
            await Navigation.PopToRootAsync();
        }

        public async Task PushAsync<TViewModel, TParam>(TParam param) where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = _viewFactory.Resolve(out viewModel);
            if (view.Parent != null) return;
            await Navigation.PushAsync(view);
            //viewModel?.OnPushed(param);            
        }

        public async Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {            
            TViewModel viewModel;
            var view = _viewFactory.Resolve(out viewModel, setStateAction);
            if (view.Parent != null) return default(TViewModel);            
            await Navigation.PushAsync(view);
            viewModel?.OnPushed();
            Logger.Current.LogInfo($"Now Pushing: {view.GetType().FullName}");
            return viewModel;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            if (view.Parent != null) return default (TViewModel);
            await Navigation.PushAsync(view);
            viewModel?.OnPushed();            
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {            
            TViewModel viewModel;
            var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
            if (view.Parent != null) return default(TViewModel);
            await Navigation.PushModalAsync(view);
            viewModel?.OnPushed();            
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {            
            var view = _viewFactory.Resolve(viewModel);
            await Navigation.PushModalAsync(view);
            if (view.Parent != null) return default(TViewModel);
            viewModel?.OnPushed();            
            return viewModel;
        }
    }
}
