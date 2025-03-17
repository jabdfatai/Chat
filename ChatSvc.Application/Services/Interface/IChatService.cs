using SecureCommSvc.Application.Responses;
using SecureCommSvc.Core.Entity;
using SecureCommSvc.Core.QueryParameters;
using SecureCommSvc.Core.Response;
using SecureCommSvc.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Application.Services.Interface
{
    public interface IChatService
    {
        Task<ApiResponse<AddResponse>> AddChat(ChatAddViewModel addChatViewModel);
        ApiResponse<AddResponse> PatchChat(string chatid, ChatPatchViewModel chatPatchViewModel);

        ApiResponse<AddResponse> PatchChatSession(string sessionId, ChatPatchViewModel chatPatchViewModel);
        ApiResponse<AddResponse> DeleteChat(string chatid);
        ApiResponse<CountModel<ChatListViewModel>> GetChatList(string usertoken, ChatQueryParameter chatQueryParameter);
        ApiResponse<CountModel<ChatViewModel>[]> GetChatList(string usertoken, ChatSpecificParameter chatSpecificParameter);


    }
}
