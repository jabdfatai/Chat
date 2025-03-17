using SecureCommSvc.Core.Entity;
using SecureCommSvc.Core.QueryParameters;
using SecureCommSvc.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.Repo.Interface
{
    public interface IChatCommandRepo
    {
        AddResponse CreateChat(Chat chat);
        AddResponse PatchChat(Chat chat);

        AddResponse PatchChatSession(string sessionid, ChatPatchModel chatPatchModel);
        AddResponse DeleteChat(Guid chatid);

    }

    public interface IChatQueryRepo
    {
        List<Chat> GetChats(ChatQueryParameter chatQueryParameter);
        List<Chat> GetChats(Guid sessionid);
        Chat GetChat(Guid chatid);

    }
}

