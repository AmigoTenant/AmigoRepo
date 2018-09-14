using System;
using System.Threading.Tasks;
using TSI.Xamarin.Forms.Mvvm.ViewModels;

namespace XPO.ShuttleTracking.Mobile.Navigation
{
    /// <summary>
    /// Wrapper around INavigation
    /// </summary>
    public interface INavigator
    {
        Task<IViewModel> PopAsync();

        Task<IViewModel> PopModalAsync();

        Task PopToRootAsync();

        Task PushAsync<TViewModel,TParam>(TParam param) where TViewModel : class, IViewModel;

        Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        void ClearNavigationStack();
        void ClearNavigationStackToRoot();
        void RemoveLastPageFromStack();
        void ClearNavigationStackToRoot(bool removePersistentPages);
    }
}
