namespace HRManagementSystem.Application.Queries.Leaves
{
    public class LeavesResponse
    {
        public List<LeavesInfo>? LeavesInfos { get; set; }
        /// <summary>
        /// 回傳資料總數
        /// </summary>
        public int Total { get; set; }
        public class LeavesInfo
        {
            /// <summary>
            /// 流水號，供編輯時選定單筆Id
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
            public decimal LeaveLimitHours { get; set; } = decimal.Zero;
            /// <summary>
            /// 
            /// </summary>
            public int OperateUserId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public DateTime CreateDate { get; set; }
        }
    }
}
