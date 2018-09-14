using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using TSI.Xamarin.Forms.Mvvm.ViewModels;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Infrastructure.Presentation.Messaging;
using XPO.ShuttleTracking.Mobile.Helpers;
using Logger = XPO.ShuttleTracking.Mobile.Infrastructure.Logger;

namespace XPO.ShuttleTracking.Mobile.Model
{
    public class TodayViewModel : ViewModelBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string _textTodayDate;
        public string TextTodayDate
        {
            get { return _textTodayDate; }
            set { SetProperty(ref _textTodayDate, value); }
        }
        public override void OnPushed()
        {
            base.OnPushed();
            TextTodayDate = DateTime.Now.ToString(DateFormats.CurrentWorkday);
            OnPropertyChanged("ChargeNo");
        }        
        public async void HandleResult(ResponseDTO<PagedList<EquipmentDTO>> result, Action action)
        {
            if (result == null)
            {
                await ShowOkAlert(AppString.lblErrorUnknown);
                return;
            }
            if (result.IsValid)
            {
                if (result.Data != null)
                {
                    if (result.Data.Items != null)
                    {
                        if (result.Data.Items.Count > 0)
                        {
                            action();
                        }
                        else
                        {
                            await ShowOkAlert(AppString.lblModalEquipNoTitle, Resource.AppString.lblEquipmentMatchData);
                        }                        
                    }
                    else
                    {
                        await ShowOkAlert(AppString.lblModalEquipNoTitle, Resource.AppString.lblEquipmentMatchData);
                    }                    
                }
                else
                {
                    await ShowOkAlert(AppString.lblModalEquipNoTitle, Resource.AppString.lblEquipmentDataError);
                }                
            }
            else
            {               
                await ShowOkAlert(AppString.lblModalEquipNoTitle, Resource.AppString.lblEquipmentDataError);
            }
        }

        public async Task<bool> CheckGpsConnection()
        {            
            var isGeolocationEnabled = PluginManager.Geolocator.IsGeolocationEnabled;            
            if(!isGeolocationEnabled)
            {
                await ShowOkAlert(AppString.lblGpsException);
            }
            return isGeolocationEnabled;
        }

        protected Task<bool> ShowYesNoAlert(string message)
        {
            return ShowYesNoAlert(string.Empty,message);
        }
        protected Task<bool> ShowYesNoAlert(string title, string message)
        {
            return MessageService.Current.ConfirmAsync(title, message, AppString.btnDialogYes, AppString.btnDialogNo);
        }
        protected async Task ShowOkAlert(string message)
        {
            await ShowOkAlert(string.Empty, message);
        }
        protected Task ShowOkAlert(string title, string message)
        {
            return MessageService.Current.ShowMessageAsync(title, message, AppString.btnDialogOk);
        }

        protected Task<PromptResult> PromptText(string text, string title, int maxLength)
        {
            return MessageService.Current.PromptAsync(text, title, AppString.btnDialogOk, maxLength);
        }

        protected Task ShowError(string code, string message)
        {
            return MessageService.Current.ShowMessageAsync(AppString.lblError, string.Format(ErrorCode.Format, code, message), AppString.btnDialogOk);
        }

        public override void OnCommandError(Exception ex)
        {
            Logger.Current.LogWarning($"Command Error:{ex}");
        }
    }    
}