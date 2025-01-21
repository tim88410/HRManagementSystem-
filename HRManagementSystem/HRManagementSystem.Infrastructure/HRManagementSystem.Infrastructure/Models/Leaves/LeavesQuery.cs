using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Infrastructure.Models.Leaves
{
    public class LeavesQuery
    {

        /// <summary>
        /// 
        /// </summary>
        public enum SortColumn
        {
            Id = 1,
            LeaveName = 2,
            Description = 3,
            LeaveLimitHours = 4,
            CreateDate = 5
        }

        public enum SortOrderBy
        {
            /// <summary>
            /// 正序排列
            /// </summary>
            Asc = 1,
            /// <summary>
            /// 倒序排列
            /// </summary>
            Desc = 2
        }

        public class LeavesQueryParameter
        {
            /// <summary>
            /// 
            /// </summary>
            public string LeaveName { get; set; } = string.Empty;
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
            public string? SortOrderBy { get; set; }

            public int StartIndex { get { return (Page - 1) * PageLimit + 1; } }
            public int EndIndex { get { return Page * PageLimit; } }

            public string SortBy
            {
                get
                {
                    if (string.IsNullOrEmpty(SortColumn) ||
                    !Enum.TryParse(SortColumn.Trim(), true, out LeavesQuery.SortColumn intSortBy) ||
                    !Enum.IsDefined(typeof(LeavesQuery.SortColumn), intSortBy))
                    {
                        SortColumn = "Id";
                    }
                    else
                    {
                        SortColumn = Enum.GetName(typeof(LeavesQuery.SortColumn), intSortBy);
                    }

                    if (string.IsNullOrEmpty(SortOrderBy) ||
                        !Enum.TryParse(SortOrderBy.Trim(), true, out LeavesQuery.SortOrderBy intSortorderBy) ||
                        !Enum.IsDefined(typeof(LeavesQuery.SortOrderBy), intSortorderBy))
                    {
                        SortOrderBy = Enum.GetName(typeof(LeavesQuery.SortOrderBy), LeavesQuery.SortOrderBy.Asc);
                    }
                    else
                    {
                        SortOrderBy = Enum.GetName(typeof(LeavesQuery.SortOrderBy), intSortorderBy);
                    }

                    return SortColumn + " " + SortOrderBy;
                }
            }
        }

        public class LeavesDTO
        {
            /// <summary>
            /// 流水號
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string LeaveName { get; set; } = string.Empty;
            /// <summary>
            /// 描述
            /// </summary>
            public string Description { get; set; } = string.Empty;
            /// <summary>
            /// 
            /// </summary>
            public decimal LeaveLimitHours { get; set; }
            /// <summary>
            /// 更新時間
            /// </summary>
            public DateTime CreateDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int OperateUserId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int TotalItem { get; set; }
        }
    }
}
