using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace Amigo.Tenant.Web.Logging
{
    public class WorkItem
    {
        private static readonly Queue<WorkItem> queue = new Queue<WorkItem>();

        private static readonly Semaphore MaxQueueSemaphore = new Semaphore(MaxQueueLength, MaxQueueLength);

        private static readonly object WorkItemLockObject = new object();
        private static WorkItem _currentWorkItem;
        private static Thread _worker;
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["amigotenantDb"].ConnectionString;

        public delegate void Worker();

        public ActionType Action { get; set; }
        public ICollection<RequestInfo> RequestInfoList { get; private set; }

        public static int MaxQueueLength => 100;

        public int Count => this.RequestInfoList.Count;

        public static int QueueCount => queue.Count;

        public WorkItem(ActionType action)
        {
            this.Action = action;
            this.RequestInfoList = new List<RequestInfo>();
        }

        private void Add(RequestInfo info)
        {
            this.RequestInfoList.Add(info);
        }

        private void Enqueue()
        {
            if (MaxQueueSemaphore.WaitOne(1000))
            {
                lock (queue)
                {
                    queue.Enqueue(this);
                    Monitor.Pulse(queue);
                }
            }
            else
            {
                EventLog.WriteEntry("Application",
                    "Timed-out enqueueing a WorkItem. Queue size = " + QueueCount +
                    ", Action = " + this.Action, EventLogEntryType.Error, 101);
            }
        }

        public static void QueuePageView(RequestInfo info, int batchSize)
        {
            lock (WorkItemLockObject)
            {
                if (_currentWorkItem == null)
                {
                    _currentWorkItem = new WorkItem(ActionType.Add);
                }
                _currentWorkItem.Add(info);
                if (_currentWorkItem.Count >= batchSize)
                {
                    _currentWorkItem.Enqueue();
                    _currentWorkItem = null;
                }
            }
        }

        public static WorkItem Dequeue()
        {
            lock (queue)
            {
                for (;;)
                {
                    if (queue.Count > 0)
                    {
                        WorkItem workItem = queue.Dequeue();
                        MaxQueueSemaphore.Release();
                        return workItem;
                    }
                    Monitor.Wait(queue);
                }
            }
        }

        public static void Init(Worker work)
        {
            lock (WorkItemLockObject)
            {
                if (_worker == null)
                    _worker = new Thread(new ThreadStart(work));
                if (!_worker.IsAlive)
                    _worker.Start();
            }
        }

        public static void Work()
        {
            try
            {
                for (;;)
                {
                    WorkItem workItem = WorkItem.Dequeue();
                    switch (workItem.Action)
                    {
                        case ActionType.Add:
                            string sql = "INSERT INTO dbo.RequestLog (URL,ServiceName,Request,Response,RequestedBy,RequestDate) values (@URL,@ServiceName,@Request,@Response,@RequestedBy,@RequestDate)";
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                foreach (RequestInfo info in workItem.RequestInfoList)
                                {
                                    using (SqlCommand cmd = new SqlCommand(sql,conn))
                                    {
                                        cmd.Parameters.Add("@URL", SqlDbType.VarChar,200).Value = info.Url;
                                        cmd.Parameters.Add("@ServiceName", SqlDbType.VarChar,200).Value = info.ServiceName;
                                        cmd.Parameters.Add("@Request", SqlDbType.Xml).Value = info.Request;
                                        cmd.Parameters.Add("@Response", SqlDbType.Xml).Value = info.Response;
                                        cmd.Parameters.Add("@RequestedBy", SqlDbType.Int).Value = info.RequestBy;
                                        cmd.Parameters.Add("@RequestDate", SqlDbType.DateTime2).Value = info.RequestDate;
                                        
                                        try
                                        {
                                            conn.Open();
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (SqlException e)
                                        {
                                            EventLog.WriteEntry("Application",
                                            "Error in WritePageView: " + e.Message + "\n",
                                            EventLogEntryType.Error, 104);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Application", "Error in MarketModule worker thread: " + e.Message,
                EventLogEntryType.Error, 105);
                throw;
            }            
        }
    }
}
