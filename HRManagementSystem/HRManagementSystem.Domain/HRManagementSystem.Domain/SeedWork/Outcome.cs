using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.Domain.SeedWork
{
    public record Outcome
    {
        public bool IsSuccess { get; private set; }
        public DomainError Error { get; private set; }

        public bool Failure
        {
            get { return !IsSuccess; }
        }
        protected Outcome(bool isSuccess, DomainError error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
        public static Outcome Fail(DomainError message)
        {
            return new Outcome(false, message);
        }

        public static Outcome Success()
        {
            return new Outcome(true, null);
        }

        internal static Outcome Fail(object price_SetPluginExtendPrice)
        {
            throw new NotImplementedException();
        }
    }
}
