using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TSI.Xamarin.Forms.Mvvm.Bootstraping;
using TSI.Xamarin.Forms.Mvvm.ViewModels;
using TSI.Xamarin.Forms.Mvvm.Views.Abstract;
using TSI.Xamarin.Forms.Persistence.NoSQL.Abstract;
using TSI.Xamarin.Forms.Persistence.Storage;
using Xamarin.Forms;
using XPO.ShuttleTracking.Application.DTOs.Responses.Move;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.Constants;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.State;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.PubSubEvents;
using XPO.ShuttleTracking.Mobile.View;
using XPO.ShuttleTracking.Mobile.View.Abstract;
using XPO.ShuttleTracking.Mobile.View.Dialog;
using XPO.ShuttleTracking.Mobile.View.SearchItem;
using XPO.ShuttleTracking.Mobile.ViewModel;
using XPO.ShuttleTracking.Mobile.ViewModel.Dialog;
using XPO.ShuttleTracking.Mobile.ViewModel.SearchItem;

namespace XPO.ShuttleTracking.Mobile.DependencyInjection
{
    public class Bootstrapper : NativeBootstrapper
    {
        public Bootstrapper(Xamarin.Forms.Application application, IResolver resolver) : base(application, resolver)
        {
        }

        public bool ShouldAddMainMenu { get; private set; } = false;

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<RegisterMoveViewModel, RegisterMoveView>();
            viewFactory.Register<CostCenterSearchViewModel, CostCenterSearchView>();
            viewFactory.Register<ProductSearchViewModel, ProductSearchView>();
            viewFactory.Register<FromBlockSearchViewModel, FromBlockSearchView>();
            viewFactory.Register<ToBlockSearchViewModel, ToBlockSearchView>();
            viewFactory.Register<MainMenuViewModel, MainMenuView>();
            viewFactory.Register<LoginViewModel, LoginView>();
            viewFactory.Register<ConfirmTosViewModel, ConfirmTosView>();
            viewFactory.Register<ConfirmPermissionViewModel, ConfirmPermissionView>();
            viewFactory.Register<StartMoveViewModel, StartMoveView>();
            viewFactory.Register<FinishMoveViewModel, FinishMoveView>();
            viewFactory.Register<RegisterAdditionalServiceViewModel, RegisterAdditionalServiceView>();
            viewFactory.Register<ConfirmationMoveViewModel, ConfirmationMoveView>();
            viewFactory.Register<StartAdditionalServiceViewModel, StartAdditionalServiceView>();
            viewFactory.Register<FinishAdditionalServiceViewModel, FinishAdditionalServiceView>();
            viewFactory.Register<ConfirmationAdditionalServiceViewModel, ConfirmationAdditionalServiceView>();
            viewFactory.Register<RegisterDetentionViewModel, RegisterDetentionView>();
            viewFactory.Register<StartDetentionViewModel, StartDetentionView>();
            viewFactory.Register<ConfirmationDetentionViewModel, ConfirmationDetentionView>();
            viewFactory.Register<FinishDetentionViewModel, FinishDetentionView>();
            viewFactory.Register<TermOfServicesViewModel, TermOfServicesView>();
            viewFactory.Register<DailyActivitiesViewModel, DailyActivitiesView>();
            viewFactory.Register<DailyActivitiesFilteredViewModel, DailyActivitiesFilteredView>();
            viewFactory.Register<DailyActivitiesFilteredItemNewViewModel, DailyActivitiesFilteredItemNewView>();
            viewFactory.Register<StoreForwardViewModel, StoreAndForwardView>();
            viewFactory.Register<AcknowledgeMoveViewModel, AcknowledgeMoveView>();
            viewFactory.Register<SummaryViewModel, SummaryView>();
            viewFactory.Register<SummaryPerHourViewModel, SummaryPerHourView>();
            viewFactory.Register<ChargeNumberSearchViewModel, ChargeNumberSearchView>();
            viewFactory.Register<AcknowledgeChargeNoViewModel, AcknowledgeChargeNoView>();


            viewFactory.Register<RegisterOperateTaylorLiftViewModel, RegisterOperateTaylorLiftView>();
            viewFactory.Register<StartOperateTaylorLiftViewModel, StartOperateTaylorLiftView>();
            viewFactory.Register<FinishOperateTaylorLiftViewModel, FinishOperateTaylorLiftView>();
            viewFactory.Register<ConfirmationOperateTaylorLiftViewModel, ConfirmationOperateTaylorLiftView>();
            viewFactory.Register<SettingsViewModel, SettingsView>();
        }

        protected override Page GetDefaultPage(IViewFactory viewFactory)
        {
            //Check if you're already logged in
            try
            {
                var storage = this.Resolver.Resolve<IPersistentStorageManager>();
                var expiration = storage.ReadValue<DateTime?>(SecuritySettings.ExpirationKey);
                if (expiration == null || expiration.Value < DateTime.UtcNow)
                {
                    return viewFactory.Resolve<LoginViewModel>();
                }

                //Init additional services
                MessagingCenter.Send(LoggedInMessage.Empty, LoggedInMessage.Name);

                //Check for a in-progress move
                var pendent = storage.ReadValue<string>(SecuritySettings.PendentMoveKey);

                if (string.IsNullOrWhiteSpace(pendent))
                {
                    Logger.Current.LogInfo("Performing AutoLogin: Main Menu");
                    return viewFactory.Resolve<MainMenuViewModel>();
                }

                Logger.Current.LogInfo("Performing AutoLogin: In Progress");
                var state = JsonConvert.DeserializeObject<State>(pendent);
                var movePage =  GetMoveViewModel(viewFactory,state);
                 ShouldAddMainMenu= true;
                return movePage;
            }
            catch (Exception e)
            {
                MessagingCenter.Send(LoggedOutMessage.Empty,LoggedOutMessage.Name);
                Logger.Current.LogWarning($"Error trying to autologin - {e}");
                var page = viewFactory.Resolve<LoginViewModel>();
                return page;
            }
        }

        private Page GetMoveViewModel(IViewFactory viewFactory,State state)
        {
            switch (state.Type)
            {
                case ServiceTypeEnum.Move:
                    return ResolveMove<IMoveRepository,BEMove,FinishMoveViewModel>(viewFactory, state.Id, state.Request);
                case ServiceTypeEnum.Detention:
                    return ResolveMove<IDetentionRepository,BEDetention,FinishDetentionViewModel>(viewFactory, state.Id, state.Request);
                case ServiceTypeEnum.Service:
                    return ResolveMove<IServiceRepository,BEService,FinishAdditionalServiceViewModel>(viewFactory, state.Id, state.Request);
                case ServiceTypeEnum.Taylor:
                    return ResolveMove<IOperateTaylorLiftRepository, BEOperateTaylorLift, FinishOperateTaylorLiftViewModel>(viewFactory, state.Id, state.Request);
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewFactory));
            }
        }

        private Page ResolveMove<TRepo,TEntity,TViewModel>(IViewFactory viewFactory, Guid id, ShuttletServiceDTO request)
            where TRepo: class, IRepository<TEntity,Guid>
            where TEntity : BEServiceBase,new()
            where TViewModel: FinishViewModelBase
        {
            var repo = Resolver.Resolve<TRepo>();
            var move = repo.FindByKey(id);
            return viewFactory.Resolve<TViewModel>(v =>
            {
                v.GeneralShuttletServiceDTO = request;
                v.GeneralServiceBase = move;
            });
        }
    }
}
