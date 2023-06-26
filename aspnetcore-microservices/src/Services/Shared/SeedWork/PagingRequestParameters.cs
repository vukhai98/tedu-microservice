﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class PagingRequestParameters
    {
        private const int maxPageSize = 100;

        private int _pageSize = 10;

        private int _pageNumber = 1;

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