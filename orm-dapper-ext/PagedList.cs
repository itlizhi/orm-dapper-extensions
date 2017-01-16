using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Orm
{
    /// <summary>
    /// 自定义pageModel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : IPagedList
    {
        /// <summary>
        /// 每页的的数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总页码
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public long TotalItem { get; set; }

        /// <summary>
        /// 是否存在上一页
        /// </summary>
        public bool HasPrevious { get; set; }

        /// <summary>
        /// 是否存在下一页
        /// </summary>
        public bool HasNext { get; set; }

        /// <summary>
        ///数据集合
        /// </summary>
        public IEnumerable<T> Items { get; set; }
    }

    public interface IPagedList
    {
        /// <summary>
        /// 每页的的数量
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        int CurrentPage { get; set; }

        /// <summary>
        /// 总页码
        /// </summary>
        int TotalPage { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        long TotalItem { get; set; }

        /// <summary>
        /// 是否存在上一页
        /// </summary>
        bool HasPrevious { get; set; }

        /// <summary>
        /// 是否存在下一页
        /// </summary>
        bool HasNext { get; set; }

    }
}
