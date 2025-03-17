using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.QueryParameters
{
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public long TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items"></param>
        /// <param name="count"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PagedList(List<T> items, long count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        /// <summary>
        /// Method to generate paginated list from IQueryable
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        /// <summary>
        /// Method to generate paginated list from IEnumberable
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedIList(List<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        /// <summary>
        /// Convert list of itams into a paginate response
        /// </summary>
        /// <param name="items"></param>
        /// <param name="count"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedListItem(List<T> items, int count, int pageNumber, int pageSize)
        {
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }

    /// <summary>
    /// Base pagination model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CountModel<T>
    {

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public int OtherInfo { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public List<T> Items { get; set; }

        public static CountModel<T> Load(PagedList<T> data)
        {
            return new CountModel<T>()
            {
                CurrentPage = data.CurrentPage,
                PageSize = data.PageSize,
                TotalCount = data.TotalCount,
                TotalPages = data.TotalPages,
                Items = data.ToList(),
            };
        }

        public CountModel()
        {

        }

        public static CountModel<T> Empty()
        {
            return new CountModel<T>()
            {
                Items = new List<T>()
            };
        }
    }
}
