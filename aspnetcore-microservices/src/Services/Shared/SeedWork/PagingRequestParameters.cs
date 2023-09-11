using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class PagingRequestParameters
    {
        private const int maxPageSize  = 100;

        public int _pageSize { set; get; } = 10;

        public int _pageNumber { set; get; } = 1;

        private int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        private int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > 0)
                    _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }

        public string OrderBy { get; set; }
    }
}
