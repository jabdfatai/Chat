using Microsoft.EntityFrameworkCore;
using SecureCommSvc.Core.Entity;
using SecureCommSvc.Core.QueryParameters;
using SecureCommSvc.Core.Repo.Interface;
using SecureCommSvc.Core.Response;
using SecureCommSvc.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Infrastructure.Repository
{
    public class ChatCommandRepo : IChatCommandRepo
    {
        AddResponse addResponse = new AddResponse();
        SecureConnDbContext context;
        public ChatCommandRepo(SecureConnDbContext context)
        {
            this.context = context;

        }

        public AddResponse CreateChat(Chat chat)
        {
            int result = 0;
            try
            {
                var query = context.chats.Add(chat);
                result = context.SaveChanges();
                if (result > 0)
                {
                    addResponse.Successful = true;
                    addResponse.MiscField1 = chat.id.ToString();
                    addResponse.MiscField2 = chat.sessionid.ToString();
                }

            }
            catch (Exception ex)
            {

            }
            return addResponse;
        }

        public AddResponse DeleteChat(Guid chatid)
        {
            int result = 0;
            try
            {
                var selchat = context.chats.FirstOrDefault(a => a.chatid == chatid);
                if (selchat != null)
                {
                    selchat.STATUS = 0;
                }
                result = context.SaveChanges();
                if (result > 0)
                {
                    addResponse.Successful = true;
                    addResponse.MiscField1 = selchat.id.ToString();
                    addResponse.MiscField2 = selchat.chatid.ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return addResponse;
        }

        public AddResponse PatchChat(Chat chat)
        {
            int result = 0;
            var selchat = context.chats.FirstOrDefault(a => a.chatid == chat.chatid);
            if (selchat != null)
            {
                if (!string.IsNullOrEmpty(chat.content))
                {
                    selchat.content = chat.content;
                }
                if (chat.send_sta != null)
                {
                    selchat.send_sta = chat.send_sta;
                }
                if (chat.senddate != null)
                {
                    selchat.senddate = chat.senddate;
                }
                if (chat.sendtime != null)
                {
                    selchat.sendtime = chat.sendtime;
                }
                if (chat.del_sta != null)
                {
                    selchat.del_sta = chat.del_sta;
                }

                if (chat.readdate != null)
                {
                    selchat.readdate = chat.readdate;
                }
                if (chat.readtime != null)
                {
                    selchat.readtime = chat.readtime;
                }

                if (chat.read_sta != null)
                {
                    selchat.read_sta = chat.read_sta;
                }

                result = context.SaveChanges();
                if (result > 0)
                {
                    addResponse.Successful = true;
                    addResponse.MiscField1 = selchat.id.ToString();
                    addResponse.MiscField2 = selchat.chatid.ToString();
                }

            }

            return addResponse;
        }
        string GetPatchString(ChatPatchModel chatPatchModel)
        {
            string query = "update chats set ";
            string delim = "";

            PropertyInfo[] pi = chatPatchModel.GetType().GetProperties();
            for (int i = 0; i < pi.Length; i++)
            {
                if (pi[i].CanRead)
                {
                    {
                        if (pi[i].GetValue(chatPatchModel) != null)
                        {

                            query = query + delim + pi[i].Name + " = '" + EscapeData(pi[i].GetValue(chatPatchModel).ToString()) + "'";
                            delim = ",";
                        }
                    }

                }
            }

            return query;

        }

        protected object EscapeData(object myData)
        {
            try
            {
                if (myData == null)
                {
                    return myData;
                }

                if (myData.ToString().IndexOf('\'') >= 0) // .IndexOfAny(.Contains("'")) 
                {
                    return myData.ToString().Replace("'", "''");
                }
                else
                {
                    return myData;
                }
            }
            catch (Exception ex)
            {

                return myData;
            }

        }

        public AddResponse PatchChatSession(string sessionid,ChatPatchModel chatPatchModel)
        {
            try
            {
                int result = 0;
                string sqlquery = GetPatchString(chatPatchModel) + $" where sessionid = '{sessionid}'";
                var res = context.Database.ExecuteSqlRaw(sqlquery);
                result = context.SaveChanges();
                if (res > 0)
                {
                    addResponse.Successful = true;
                    addResponse.MiscField1 = sessionid.ToString();
                    addResponse.MiscField2 = result.ToString();
                }
             
            }
            catch (Exception ex)
            {

            }
            return addResponse;
        }
    }
    public class ChatQueryRepo : IChatQueryRepo
    {

        AddResponse addResponse = new AddResponse();
        SecureConnDbContext context;
        public ChatQueryRepo(SecureConnDbContext context)
        {
            this.context = context;
        }
        public Chat GetChat(Guid chatid)
        {
            Chat chat = new Chat();
            try
            {
                var query = context.chats.Where(a => a.chatid == chatid);
                if (query.Any())
                {
                    chat = query.First();
                }

            }
            catch (Exception ex)
            {

            }
            return chat;
        }

        public List<Chat> GetChats(ChatQueryParameter chatQueryParameter)
        {
            List<Chat> chatlist = new List<Chat>();
            try
            {
                var query = context.chats.Where(a => a.STATUS == 1);

                if (chatQueryParameter.userid != null)
                {
                    query = query.Where(a => a.sndrid == chatQueryParameter.userid || a.rcvid == chatQueryParameter.userid);
                }

                if (chatQueryParameter.sessionid != null)
                {
                    query = query.Where(a => a.sessionid == chatQueryParameter.sessionid);
                }
                if (chatQueryParameter.senderid != null)
                {
                    query = query.Where(a => a.sndrid == chatQueryParameter.senderid);
                }
                if (chatQueryParameter.receiverid != null)
                {
                    query = query.Where(a => a.rcvid == chatQueryParameter.receiverid);
                }
                if (chatQueryParameter.content != null)
                {
                    query = query.Where(a => a.content.Contains(chatQueryParameter.content));
                }

                if (chatQueryParameter.read_sta != null)
                {
                    query = query.Where(a => a.read_sta == chatQueryParameter.read_sta);
                }

                if (query.Any())
                {
                    chatlist = query.OrderByDescending(a => a.id).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return chatlist;
        }


        public List<Chat> GetChats(Guid sessionid)
        {
            List<Chat> chatlist = new List<Chat>();
            try
            {
                var query = context.chats.Where(a => a.STATUS == 1 & a.sessionid == sessionid);


                if (query.Any())
                {
                    chatlist = query.OrderBy(a => a.id).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return chatlist;
        }

    }
}
