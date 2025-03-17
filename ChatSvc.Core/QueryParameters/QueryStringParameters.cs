using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.QueryParameters
{
    public class QueryStringParameters
    {
        const int maxPageSize = 50;

        /// <summary>
        /// Page number for pagination
        /// </summary>
        public int? PageNumber { get; set; } = 1;
     
        private int _pageSize = 10;

        /// <summary>
        /// page size for pagination
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
