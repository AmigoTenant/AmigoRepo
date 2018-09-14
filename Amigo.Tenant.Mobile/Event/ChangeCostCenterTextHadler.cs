using System;
using System.Collections.Generic;
using System.Linq;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;

namespace XPO.ShuttleTracking.Mobile.Event
{
    public class ChangeCostCenterTextHadler
    {
        private readonly ICostCenterRepository _costCenterRepository;
        public ChangeCostCenterTextHadler(ICostCenterRepository costCenterRepository)
        {
            _costCenterRepository = costCenterRepository;
        }

        public void Search(string search)
        {
            var args = new SearchInListEventArgs();
            if (string.IsNullOrEmpty(search) || search.Length<3)
            {
                args.TotalResult = 0;
                args.LstCostCenter = null;
                OnThresholdReached(args);
                return;
            }

            var list =
                _costCenterRepository.FindAll(x => x.Name.ToUpper().Contains(search.ToUpper()), 20)
                    .OrderBy(dto => dto.Name)
                    .ToList();
            if (list.Any())
            { 
                args.TotalResult = list.Count;
                args.LstCostCenter = list;
            }else
            {
                args.TotalResult = 0;
                args.LstCostCenter = null;
                OnThresholdReached(args);
            }
            OnThresholdReached(args);
        }
        protected virtual void OnThresholdReached(SearchInListEventArgs e)
        {
            SearchInList?.Invoke(this, e);
        }

        public event EventHandler<SearchInListEventArgs> SearchInList;
    }
    public class SearchInListEventArgs : EventArgs
    {
        public int TotalResult { get; set; }
        public IList<CostCenterDTO> LstCostCenter { get; set; }
    }
}
