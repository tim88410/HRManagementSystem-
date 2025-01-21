using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.Domain.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}
