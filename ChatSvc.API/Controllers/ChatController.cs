using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SecureCommSvc.Application.Responses;
using SecureCommSvc.Application.Services;
using SecureCommSvc.Application.Services.Interface;
using SecureCommSvc.Core.QueryParameters;
using SecureCommSvc.Core.Response;
using SecureCommSvc.Core.ViewModel;

namespace SecureCommSvc.API.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class ChatController : ControllerBase
    {
        IChatService chatService;
        ILogger<ChatController> logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            this.chatService = chatService;
            this.logger = logger;
        }
        [HttpPost]
        public ActionResult<ApiResponse<AddResponse>> AddChat([FromBody] ChatAddViewModel chatAddViewModel)
        {
            try
            {

                var resp = chatService.AddChat(chatAddViewModel).Result;
                if (resp.Successful)
                {

                    return CreatedAtAction(nameof(AddChat), new { id = resp.ResponseData.MiscField1 }, resp);
                }
                else if (resp.ResultType == ResultType.Duplicate)
                {
                    return Conflict(resp);
                }
                else
                {
                    return BadRequest(resp);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());
                return BadRequest();
            }

        }
 
        [HttpPatch("{chatId}")]
        public ActionResult<ApiResponse<AddResponse>> PatchChat([FromRoute] string chatId, [FromBody] ChatPatchViewModel chatPatchViewModel)
        {
            try
            {
                var resp = chatService.PatchChat(chatId, chatPatchViewModel);
                if (resp.Successful)
                {

                    return Ok(resp);

                }
                else
                {
                    return BadRequest(resp);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());
                return BadRequest();
            }

        }


        [HttpPatch("{sessionId}/chatsession")]
        public ActionResult<ApiResponse<AddResponse>> PatchChatSession([FromRoute] string sessionId, [FromBody] ChatPatchViewModel chatPatchViewModel)
        {
            try
            {
                var resp = chatService.PatchChatSession(sessionId, chatPatchViewModel);
                if (resp.Successful)
                {

                    return Ok(resp);

                }
                else
                {
                    return BadRequest(resp);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());
                return BadRequest();
            }

        }



        [HttpDelete("{chatId}")]
        public ActionResult<ApiResponse<AddResponse>> DeleteChat([FromRoute] string chatId)
        {
            try
            {
                var resp = chatService.DeleteChat(chatId);
                if (resp.Successful)
                {

                    return Ok(resp);

                }
                else
                {
                    return BadRequest(resp);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());
                return BadRequest();
            }

        }

        [HttpGet()]
        public ActionResult<ApiResponse<CountModel<ChatListViewModel>>> GetChats([FromQuery] ChatQueryParameter chatQueryParameter)
        {
            try
            {
                var usertoken = Request.Headers.TryGetValue("Authorization", out var strtoken);

                if (usertoken)
                {
                    var resp = chatService.GetChatList(strtoken, chatQueryParameter);
                    if (resp.Successful)
                    {

                        return Ok(resp);

                    }
                    else
                    {

                        return BadRequest(resp);
                    }
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());
            }
            return BadRequest();

        }
        [HttpGet("session")]
        public ActionResult<ApiResponse<CountModel<ChatListViewModel>[]>> GetChatBysession([FromQuery] ChatSpecificParameter chatSpecificParameter)
        {
            try
            {
           

                var usertoken = Request.Headers.TryGetValue("Authorization", out var strtoken);

                if (usertoken)
                {
                    var pagenumtest = Request.Query.TryGetValue("PageNumber", out var pagenum);
                    if (!pagenumtest)
                    {
                        chatSpecificParameter.PageNumber = null;
                    }
                    var resp = chatService.GetChatList(strtoken, chatSpecificParameter);
                    if (resp.Successful)
                    {

                        return Ok(resp);

                    }
                    else
                    {
                        return BadRequest(resp);
                    }
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());
            }
            return BadRequest();

        }

    }
}
