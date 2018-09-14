using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace XPO.ShuttleTracking.Mobile.Helpers.Timing
{
    public class Timer: CancellationTokenSource
    {
        public Timer(Action callback, int millisecondsPeriod)
        {
            Task.Delay(0,Token).ContinueWith(async (t,s) =>
            {
                try
                {
                    while (!IsCancellationRequested)
                    {
                        Token.ThrowIfCancellationRequested();

                        await Task.Run(callback);

                        await Task.Delay(millisecondsPeriod, Token).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException ce)
                {
                    Debug.WriteLine(ce);                    
                }catch(Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            },new object(),Token,TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,TaskScheduler.Current);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                Cancel();

            base.Dispose(disposing);
        }
    }
}
