using SecureCommSvc.Core.Entity;
using SecureCommSvc.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.Repo.Interface
{
    public interface IUserCommandRepo
    {
        AddResponse CreateUser(User user);
        AddResponse PatchUser(User user);
        AddResponse DeleteUser(Guid userid);

    }
    public interface IUserQueryRepo
    {
        User GetUser(Guid userid);

    }
}
