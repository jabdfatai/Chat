using SecureCommSvc.Core.Entity;
using SecureCommSvc.Core.Repo.Interface;
using SecureCommSvc.Core.Response;
using SecureCommSvc.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Infrastructure.Repository
{
    public class UserCommandRepo : IUserCommandRepo
    {
        AddResponse addResponse = new AddResponse();
        SecureConnDbContext context;

        public UserCommandRepo(SecureConnDbContext context)
        {
            this.context = context;
        }
        public AddResponse CreateUser(User user)
        {
            int result = 0;
            try
            {
                var query = context.users.Add(user);
                result = context.SaveChanges();
                if (result > 0)
                {
                    addResponse.Successful = true;
                    addResponse.MiscField1 = user.id.ToString();
                    addResponse.MiscField2 = user.USER_ID.ToString();
                }

            }
            catch (Exception ex)
            {

            }
            return addResponse;
        }

        public AddResponse DeleteUser(Guid userid)
        {
            int result = 0;
            try
            {

                var user = context.users.FirstOrDefault(a => a.USER_ID == userid);
                if (user != null)
                {
                    user.STATUS = 0;
                    user.DT_MODF = DateTime.Now;
                }
                result = context.SaveChanges();
                if (result > 0)
                {
                    addResponse.Successful = true;
                    addResponse.MiscField1 = userid.ToString();
                    addResponse.MiscField2 = result.ToString();
                }

            }
            catch (Exception ex)
            {

            }
            return addResponse;
        }

        public AddResponse PatchUser(User user)
        {
            int result = 0;
            var seluser = context.users.FirstOrDefault(a => a.USER_ID == user.USER_ID);
            if (seluser != null)
            {
                if (user.GENDER != null)
                {
                    seluser.GENDER = user.GENDER;
                }
                if (!string.IsNullOrEmpty(user.FST_NAME))
                {
                    seluser.FST_NAME = user.FST_NAME.ToString();
                }
                if (!string.IsNullOrEmpty(user.EMAIL))
                {
                    seluser.EMAIL = user.EMAIL.ToString();
                }
                if (!string.IsNullOrEmpty(user.LST_NAME))
                {
                    seluser.LST_NAME = user.LST_NAME.ToString();
                }
                if (!string.IsNullOrEmpty(user.PHONE))
                {
                    seluser.PHONE = user.PHONE.ToString();
                }
                if (user.BIR_DT != null)
                {
                    seluser.BIR_DT = user.BIR_DT;
                }
                if (user.TITLEID != null)
                {
                    seluser.TITLEID = user.TITLEID;
                }
                if (user.MARITAL_STA_CODE != null)
                {
                    seluser.MARITAL_STA_CODE = user.MARITAL_STA_CODE;
                }

                result = context.SaveChanges();
                if (result > 0)
                {
                    addResponse.Successful = true;
                    addResponse.MiscField1 = seluser.id.ToString();
                    addResponse.MiscField2 = seluser.USER_ID.ToString();
                }

            }

            return addResponse;
        }
    }
    public class UserQueryRepo : IUserQueryRepo
    {
        SecureConnDbContext context;
        public UserQueryRepo(SecureConnDbContext context)
        {
            this.context = context;
        }
        public User GetUser(Guid userid)
        {
            User user = new User();
            try
            {
                var query = context.users.Where(a => a.USER_ID == userid);
                if (query.Any())
                {
                    user = query.First();
                }

            }
            catch (Exception ex)
            {

            }
            return user;
        }
    }
}
