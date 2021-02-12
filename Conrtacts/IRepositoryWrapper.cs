using Conrtacts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Conrtacts
{
   public interface IRepositoryWrapper 
    {
        public IUserRepository User { get; }
        public IEmployeeRepository Employee { get; }
        public ICompanyRepository Company { get; }
        Task SaveAsync();
        void Save();
    }
}
