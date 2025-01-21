using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Application.Queries.Leaves
{
    public class LeavesRequest : IRequest<LeavesResponse>
    {
        /// <summary>
        /// 第幾頁
        /// </summary>
        [Required]
        [Range(1, 10000)]
        public int Page { get; set; }
        /// <summary>
        /// 每頁幾筆
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int PageLimit { get; set; }
        /// <summary>
        /// 前端排序選項
        /// </summary>
        public string SortColumn { get; set; } = string.Empty;
        /// <summary>
        /// ASC OR DESC
        /// </summary>
        public string SortOrderBy { get; set; } = string.Empty;
    }
}
