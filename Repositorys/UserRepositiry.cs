using Conrtacts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorys
{
    public class UserRepositiry: RepositoryBase<User>, IUserRepository
    {
        public UserRepositiry(RepositoryContext context):base(context)
        {
        }


        public Task<User> GetUserByCondition(Guid id) => 
            FindByCondition(x=>x.UserId.Equals(id), false).SingleOrDefaultAsync();



        public void DeleteUser(User user) => Delete(user);

        public void GreateUser(User user) => Create(user);
       
    }
}
