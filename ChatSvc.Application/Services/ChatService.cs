using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SecureCommSvc.Application.Responses;
using SecureCommSvc.Application.Services.Interface;
using SecureCommSvc.Core.Entity;
using SecureCommSvc.Core.QueryParameters;
using SecureCommSvc.Core.Repo.Interface;
using SecureCommSvc.Core.Request;
using SecureCommSvc.Core.Response;
using SecureCommSvc.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UserIdentitySvc.Core.Interface;

namespace SecureCommSvc.Application.Services
{
    public class ChatService : IChatService
    {
        IHubContext<ChatHub> hubContext;
        IConfiguration configuration;
        IChatCommandRepo ChatCommandRepo;
        IChatQueryRepo chatQueryRepo;
        ILogger<ChatService> logger;
        IEventStreamProvider eventStreamProvider;
        public ChatService(IHubContext<ChatHub> hubContext,
            ILogger<ChatService> logger,
            IConfiguration configuration,
            IChatCommandRepo ChatCommandRepo,
            IChatQueryRepo chatQueryRepo,
             IEventStreamProvider eventStreamProvider)
        {
            this.hubContext = hubContext;
            this.configuration = configuration;
            this.ChatCommandRepo = ChatCommandRepo;
            this.chatQueryRepo = chatQueryRepo;
            this.logger = logger;
            this.eventStreamProvider = eventStreamProvider;
        }
        public async Task<ApiResponse<AddResponse>> AddChat(ChatAddViewModel addChatViewModel)
        {
            ChatViewModel chatViewModel = new ChatViewModel();
            var resp = ApiResponse<AddResponse>.Failed("");
            try
            {
                Chat chat = new Chat();
                chat.chatid = Guid.NewGuid();
                chat.chann_id = addChatViewModel.chann_id;
                chat.content = addChatViewModel.content;
                chat.rcvid = addChatViewModel.receiverid;
                chat.sessionid = addChatViewModel.sessionid == null ? Guid.NewGuid() : addChatViewModel.sessionid.Value;
                chat.sndrid = addChatViewModel.senderid;
                chat.send_sta = addChatViewModel.send_sta;
                chat.senddate = DateTime.UtcNow;
                chat.sendtime = DateTime.UtcNow.ToString("hh:mm:ss tt");

                //Persist Chat Record  

                var persresp = ChatCommandRepo.CreateChat(chat);
                if (persresp.Successful == true)
                {
                    chatViewModel.chatid = chat.chatid;
                    chatViewModel.sessionid = chat.sessionid;
                    chatViewModel.chann_id = chat.chann_id;
                    chatViewModel.content = chat.content;
                    chatViewModel.receiverid = chat.rcvid;
                    chatViewModel.senderid = chat.sndrid;
                    chatViewModel.senddate = chat.senddate;
                    chatViewModel.sendtime = chat.sendtime;
                    chatViewModel.del_sta = chat.del_sta;
                    chatViewModel.send_sta = chat.send_sta;
                    var unreadmod = chatQueryRepo.GetChats(new ChatQueryParameter
                    {
                        read_sta = false,
                        receiverid = chat.rcvid

                    });
                    chatViewModel.rcvunreadcount = unreadmod.Count;
                    resp = ApiResponse<AddResponse>.Success(persresp);

                    // Brodcast  Instant Message to SignalR

                    await hubContext.Clients.All.SendAsync("ReceiveMessage", chatViewModel);

                    NotModel notmodel = new NotModel();
                    notmodel.user_id = chatViewModel.receiverid;
                    notmodel.subject = "Instant Message Received";
                    notmodel.body = "You have a new message";
                    notmodel.eventid = 2;

                    string notmodelstring = JsonConvert.SerializeObject(notmodel);

                    string eventstreamaddress = $"{configuration.GetSection("parameters").GetSection("eventstreamserver").Value}:{configuration.GetSection("parameters").GetSection("eventstreamport").Value}";
                   
                    //Publish Chat Sending Event on Kafka  
                    
                    var res = eventStreamProvider.SendOrderRequest("mHealth-event-notification", notmodelstring, eventstreamaddress);

                    var eventsendres = res.Result;


                }
            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message);
            }
            return resp;
        }

        public ApiResponse<AddResponse> DeleteChat(string chatid)
        {
            var resp = ApiResponse<AddResponse>.Failed("");
            try
            {

                var persresp = ChatCommandRepo.DeleteChat(new Guid(chatid));

                if (persresp.Successful == true)
                {
                    resp = ApiResponse<AddResponse>.Success(persresp);
                }

            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message);
            }
            return resp;
        }
        public ApiResponse<AddResponse> PatchChat(string chatid, ChatPatchViewModel chatPatchViewModel)
        {
            var resp = ApiResponse<AddResponse>.Failed("");
            try
            {

                var persresp = ChatCommandRepo.PatchChat(new Chat
                {
                    chatid = new Guid(chatid),
                    content = chatPatchViewModel.content,
                    del_sta = chatPatchViewModel.del_sta,
                    readdate = chatPatchViewModel.readdate,
                    readtime = chatPatchViewModel.readtime,
                    read_sta = chatPatchViewModel.read_sta,
                    senddate = chatPatchViewModel.senddate,
                    sendtime = chatPatchViewModel.sendtime,
                    send_sta = chatPatchViewModel.send_sta,
                    DT_MODF = DateTime.UtcNow

                });

                if (persresp.Successful == true)
                {
                    resp = ApiResponse<AddResponse>.Success(persresp);
                }

            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message);
            }
            return resp;
        }



        public ApiResponse<CountModel<ChatListViewModel>> GetChatList(string usertoken, ChatQueryParameter chatQueryParameter)
        {
            var resp = ApiResponse<CountModel<ChatListViewModel>>.Failed("");
            List<ChatListViewModel> chatLists = new List<ChatListViewModel>();
            try
            {
                var currentuser = GetUserInfo(usertoken);
                var persresp = chatQueryRepo.GetChats(chatQueryParameter);
                if (persresp != null)
                {

                    var chatsessiongrps = persresp.GroupBy(a => a.sessionid);
                    if (chatsessiongrps != null)
                    {
                        foreach (var chatsessiongrp in chatsessiongrps)
                        {
                            ChatListViewModel chatListViewModel = new ChatListViewModel();
                            var chatsession = chatsessiongrp.FirstOrDefault();
                            chatListViewModel.sessionid = chatsession.sessionid;
                            chatListViewModel.correspondenceid = currentuser.USER_ID == chatsession.sndrid ? chatsession.rcvid : chatsession.sndrid;
                            chatListViewModel.msgcount = chatsessiongrp.Count();
                            chatListViewModel.unreadmsgcount = chatsessiongrp.Count(a => a.read_sta == false);


                            chatListViewModel.chatlist = chatsessiongrp.Select(a => new ChatViewModel
                            {
                                chann_id = a.chann_id,
                                chatid = a.chatid,
                                content = a.content,
                                del_sta = a.del_sta,
                                readdate = a.readdate,
                                readtime = a.readtime,
                                read_sta = a.read_sta,
                                senddate = a.senddate,
                                sendtime = a.sendtime,
                                send_sta = a.send_sta,
                                senderid = a.sndrid,
                                receiverid = a.rcvid,
                                sessionid = a.sessionid,
                                msg_type = currentuser.USER_ID == a.sndrid ? "sent" : "received"

                            }).ToList();
                            chatLists.Add(chatListViewModel);
                        }


                    }

                    var pagedlist = CountModel<ChatListViewModel>.Load(PagedList<ChatListViewModel>.ToPagedIList(chatLists, chatQueryParameter.PageNumber.Value, chatQueryParameter.PageSize));

                    pagedlist.OtherInfo = persresp.Count(a => a.rcvid == chatQueryParameter.userid & a.read_sta == false);

                    resp = ApiResponse<CountModel<ChatListViewModel>>.Success(pagedlist);



                }

            }
            catch (Exception ex)
            {

            }
            return resp;
        }



        UserViewModel GetUserInfo(string token)
        {
            UserViewModel userViewModel = new UserViewModel();
            try
            {
                string tokenstrip = token.Split(" ")[1];

                JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
                JwtSecurityToken securitytoken = (JwtSecurityToken)tokenhandler.ReadToken(tokenstrip);
                securitytoken.Payload.TryGetValue("tokenauth", out var tokenauth);

                string tokenauthstr = tokenauth.ToString();
                var usertokedetails = JsonConvert.DeserializeObject<UserViewModel>(tokenauthstr);

                if (tokenauth != null)
                {
                    userViewModel.IDP_ENR = usertokedetails.IDP_ENR;
                    userViewModel.USER_ID = usertokedetails.USER_ID;
                    userViewModel.GENDER = usertokedetails.GENDER;
                    userViewModel.TITLE = usertokedetails.TITLE;
                    userViewModel.MARITALSTATUS = usertokedetails.MARITALSTATUS;
                    userViewModel.BIR_DT = usertokedetails.BIR_DT;
                    userViewModel.EMAIL = usertokedetails.EMAIL;
                    userViewModel.LASTNAME = usertokedetails.LASTNAME;
                    userViewModel.FIRSTNAME = usertokedetails.FIRSTNAME;
                    userViewModel.PHONE = usertokedetails.PHONE;

                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
            return userViewModel;
        }

        public ApiResponse<CountModel<ChatViewModel>[]> GetChatList(string usertoken, ChatSpecificParameter chatSpecificParameter)
        {
            var resp = ApiResponse<CountModel<ChatViewModel>[]>.Failed("");
            List<ChatViewModel> chatLists = new List<ChatViewModel>();
            try
            {
                var currentuser = GetUserInfo(usertoken);
                var persresp = chatQueryRepo.GetChats(chatSpecificParameter.sessionid);
                if (persresp != null)
                {

                    chatLists = persresp.Select(a => new ChatViewModel
                    {
                        chann_id = a.chann_id,
                        chatid = a.chatid,
                        content = a.content,
                        del_sta = a.del_sta,
                        readdate = a.readdate,
                        sessionid = a.sessionid,
                        readtime = a.readtime,
                        read_sta = a.read_sta,
                        senddate = a.senddate,
                        sendtime = a.sendtime,
                        send_sta = a.send_sta,
                        senderid = a.sndrid,
                        receiverid = a.rcvid,
                        msg_type = currentuser.USER_ID == a.sndrid ? "sent" : "received"



                    }).ToList();

                }
                if (chatSpecificParameter.PageNumber == null)
                {
                    chatSpecificParameter.PageNumber = (int)Math.Ceiling(persresp.Count / (double)chatSpecificParameter.PageSize);
                }
                var pagedlistprevpage = persresp.Count > chatSpecificParameter.PageSize ? CountModel<ChatViewModel>.Load(PagedList<ChatViewModel>.ToPagedIList(chatLists, chatSpecificParameter.PageNumber.Value - 1, chatSpecificParameter.PageSize)) : null;

                var pagedlistcurrent = CountModel<ChatViewModel>.Load(PagedList<ChatViewModel>.ToPagedIList(chatLists, chatSpecificParameter.PageNumber.Value, chatSpecificParameter.PageSize));


                var finalresponse = new CountModel<ChatViewModel>[] { pagedlistprevpage, pagedlistcurrent };


                resp = ApiResponse<CountModel<ChatViewModel>[]>.Success(finalresponse);



            }
            catch (Exception ex)
            {

            }
            return resp;
        }

        public ApiResponse<AddResponse> PatchChatSession(string sessionId, ChatPatchViewModel chatPatchViewModel)
        {
            var resp = ApiResponse<AddResponse>.Failed("");
            try
            {
                var persresp = ChatCommandRepo.PatchChatSession(sessionId, new ChatPatchModel
                {

                    del_sta = chatPatchViewModel.del_sta,
                    readdate = chatPatchViewModel.readdate,
                    readtime = chatPatchViewModel.readtime,
                    read_sta = chatPatchViewModel.read_sta,
                    senddate = chatPatchViewModel.senddate,
                    sendtime = chatPatchViewModel.sendtime,
                    send_sta = chatPatchViewModel.send_sta

                });
                if (persresp.Successful == true)
                {
                    resp = ApiResponse<AddResponse>.Success(persresp);
                }


            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message);
            }
            return resp;
        }
    }
}
