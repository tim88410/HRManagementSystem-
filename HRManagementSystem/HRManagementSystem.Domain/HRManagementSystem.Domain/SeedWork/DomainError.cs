using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.Domain.SeedWork
{
    public abstract class DomainError
    {
        protected DomainError(int code, string errorMessage)
        {
            Code = code;
            ErrorMessage = errorMessage;
        }

        public int Code { get; }
        public string ErrorMessage { get; }
        public class LeaveDomainError : DomainError
        {
            public LeaveDomainError(int code, string errormessage) : base(code, errormessage)
            {
            }

            public static LeaveDomainError Leave_Name = new LeaveDomainError(1, "Leave Name Cannt Be Empty");
            public static LeaveDomainError Leave_LeaveLimitHours = new LeaveDomainError(2, "Leave Hours Cannt Be Zero");
            public static LeaveDomainError Leave_OperateUserId = new LeaveDomainError(3, "Leave OperateUserId Shouldnt Be Zero");
        }
    }
}
