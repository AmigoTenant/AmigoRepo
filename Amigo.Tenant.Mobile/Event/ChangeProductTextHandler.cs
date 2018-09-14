using System;
using System.Collections.Generic;
using System.Linq;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;

namespace XPO.ShuttleTracking.Mobile.Event
{
    public class ChangeProductTextHandler
    {
        private readonly IProductRepository _productRepository;
        public ChangeProductTextHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public void SearchProduct(string search)
        {
            var args = new SearchProductInListEventArgs();
            if (string.IsNullOrEmpty(search) || search.Length < 3)
            {
                args.TotalResult = 0;
                args.LstProducts = null;
                OnThresholdReached(args);
                return;
            }

            var list =
                _productRepository.FindAll(x => x.Name.ToUpper().Contains(search.ToUpper()), 20)
                    .OrderBy(dto => dto.Name)
                    .ToList();
            if (list.Any())
            {
                args.TotalResult = list.Count;
                args.LstProducts = list;
            }else
            {
                args.TotalResult = 0;
                args.LstProducts = null;
                OnThresholdReached(args);
            }
            OnThresholdReached(args);
        }
          protected virtual void OnThresholdReached(SearchProductInListEventArgs e)
        {
            SearchProductInList?.Invoke(this, e);

        }
        public event EventHandler<SearchProductInListEventArgs> SearchProductInList;
    }
    
    public class SearchProductInListEventArgs : EventArgs
    {
        public int TotalResult { get; set; }
        public IList<ProductDTO> LstProducts { get; set; }
    }

}

