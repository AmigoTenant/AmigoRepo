using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Common.Util;
using XPO.ShuttleTracking.Mobile.Entity.StoreAndForward;
using XPO.ShuttleTracking.Mobile.Entity.Tasks.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.Presentation.Messaging;
using XPO.ShuttleTracking.Mobile.Infrastructure.PubSubEvents;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.PubSubEvents;
using XPO.ShuttleTracking.Mobile.Resource;
using Environment = System.Environment;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public sealed class StoreForwardViewModel : TodayViewModel
    {
        private readonly Dictionary<int, string> _texts = new Dictionary<int, string>
        {
            {StoreForwardCode.EVENT_STATUS_EXECUTING,  StoreForwardCode.EVENT_NAME_STATUS_EXECUTING},
            {StoreForwardCode.EVENT_STATUS_PENDING,    StoreForwardCode.EVENT_NAME_STATUS_PENDING},
            {StoreForwardCode.EVENT_STATUS_COMPLETED,  StoreForwardCode.EVENT_NAME_STATUS_COMPLETED},
            {StoreForwardCode.EVENT_STATUS_FAILED,     StoreForwardCode.EVENT_NAME_STATUS_FAILED}
        };

        public override void OnAppearing()
        {
            base.OnAppearing();
            TaskManager.Current.QueueUpdated += Current_QueueUpdated;
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            TaskManager.Current.QueueUpdated -= Current_QueueUpdated;
        }

        public StoreForwardViewModel()
        {
            //Init commands
            SyncNowCommand = CreateCommand(SyncNow);
            UpdateCommand = new Command(OnUpdateCommand);
        }

        private void Current_QueueUpdated(StoreAndFowardMessage messaage)
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;

                using (StartLoading(AppString.lblGeneralLoading))
                {
                    Task.Run(() => UpdatePendentTasks()).ContinueWith(t=> IsLoading=false);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        public void OnUpdateCommand()
        {
            IsLoading = true;
            UpdatePendentTasks();
            IsLoading = false;
        }

        private async Task SyncNow()
        {
            if (TaskManager.Current?.IsExecuting == true)
            {
                await MessageService.Current.ShowMessageAsync(AppString.titleStoreForward, AppString.storeForwardrunning,AppString.btnDialogOk);
            }
            else
            {
                MessagingCenter.Send(ManuallyStartTaskManagerMessage.Empty, ManuallyStartTaskManagerMessage.Name);
            }
        }

        public override void OnPushed()
        {
            base.OnPushed();

            Task.Run(() =>
            {
                try
                {
                    IsLoading = true;
                    OnUpdateCommand();
                }
                catch (Exception ex)
                {
                    Logger.Current.LogWarning($"An error ocurred updating S&F Viewer : {ex}");
                }
                finally
                {
                    IsLoading = false;
                }
            });    
        }
        private void UpdatePendentTasks()
        {
            var taskQueue = TaskManager.Current.TasksQueue.ToList();

            var pendentTasks = new BEStoreAndForward[taskQueue.Count];

            for (var i = 0; i < taskQueue.Count; i++)
            {
                var task = taskQueue[i];
                var statusCode = _texts[GetStatusFromTask(task)];

                var detailBreak = Ellipsis(task.Details);

                pendentTasks[i]=new BEStoreAndForward()
                {
                    ActivityName = task.ActivityName,
                    Status = $"{statusCode}({task.ExecutionTimes})",
                    RegisteredDate = task.RegisteredDate.ToString(DateFormats.StoreAndForward),
                    ExecutionDate = task.ExecutionDate.ToString(DateFormats.StoreAndForward),
                    Details = task.Details,

                    Tag = "Activity" + Environment.NewLine +
                          "Status" + Environment.NewLine +
                          "Created" + Environment.NewLine +
                          "Updated" + Environment.NewLine +
                          "Details",
                    Desc = task.ActivityName + Environment.NewLine +
                           $"{statusCode}({task.ExecutionTimes})" + Environment.NewLine +
                           task.RegisteredDate.ToString(DateFormats.StoreAndForward) + Environment.NewLine +
                           task.ExecutionDate.ToString(DateFormats.StoreAndForward) + Environment.NewLine +
                           detailBreak
                };
            }
            PendentTasks.Clear();
            PendentTasks.AddRange(pendentTasks);
        }

        private static string Ellipsis(string text)
        {
            if(string.IsNullOrEmpty(text)) return string.Empty;

            if (text.Length > 28)
            {
                return text.Substring(0,25) + "...";
            }
            return text;
        }

        private static int GetStatusFromTask(TaskDefinition task)
        {
            var statusCode = StoreForwardCode.EVENT_STATUS_PENDING;
            if (task.Completed)
            {
                statusCode = StoreForwardCode.EVENT_STATUS_COMPLETED;
            }
            else if (task.ExecutionTimes > 1)
            {
                statusCode = StoreForwardCode.EVENT_STATUS_FAILED;
            }
            return statusCode;
        }
        public ObservableRangeCollection<BEStoreAndForward> PendentTasks { get; } = new ObservableRangeCollection<BEStoreAndForward>();

        public ICommand SyncNowCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
    }


}
