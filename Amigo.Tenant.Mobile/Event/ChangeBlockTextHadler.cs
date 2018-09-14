using System;
using System.Collections.Generic;
using System.Linq;
using XPO.ShuttleTracking.Mobile.Common.Util;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;

namespace XPO.ShuttleTracking.Mobile.Event
{
    public class ChangeBlockTextHadler
    {
        private readonly ILocationRepository _locationRepository;
        public ChangeBlockTextHadler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public void Search(string search)
        {
            var args = new SearchBlockInListEventArgs();
            if (string.IsNullOrEmpty(search) || search.Length < 3)
            {
                args.TotalResult = 0;
                args.LstBlock = null;
                OnThresholdReached(args);
                return;
            }

            var list = _locationRepository.GetAllSortedByName().Where(x=>x.Name.ToUpper().Contains(search.ToUpper())).ToList();
            if (list.Any())
            {
                args.TotalResult = list.Count;
                args.LstBlock = list;
            }
            else
            {
                args.TotalResult = 0;
                args.LstBlock = null;
                OnThresholdReached(args);
            }
            OnThresholdReached(args);            
        }
        protected virtual void OnThresholdReached(SearchBlockInListEventArgs e)
        {
            SearchInList?.Invoke(this, e);
        }

        public event EventHandler<SearchBlockInListEventArgs> SearchInList;
    }
    public class SearchBlockInListEventArgs : EventArgs
    {
        public int TotalResult { get; set; }
        public IList<Location> LstBlock { get; set; }
    }
}
