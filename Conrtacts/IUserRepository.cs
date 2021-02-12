using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Conrtacts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        public void GreateUser(User user);
        public void DeleteUser(User user);
        Task<User> GetUserByCondition(Guid id);

    }
}
