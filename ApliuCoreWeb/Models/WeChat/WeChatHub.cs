using ApliuCoreWeb.Models.SignalRHub;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ApliuCoreWeb.Models.WeChat
{
    public class WeChatHub : IHubServer
    {
        [HubMethodName("sendAllMessage")]
        public override async Task SendAllMessage(string Message)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.userName = MemoryCacheCore.GetValue(Context.ConnectionId).ToString();
            messageModel.Message = Message;
            await Clients.All.ReceiveMessage(messageModel);
        }

        [HubMethodName("chatLogin")]
        public void ChatLogin(string userName, String password)
        {
            MemoryCacheCore.Create(Context.ConnectionId, userName);
        }

        [HubMethodName("sendOthersMessage")]
        public override async Task SendOthersMessage(string Message)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.userName = MemoryCacheCore.GetValue(Context.ConnectionId).ToString();
            messageModel.Message = Message;
            String chatLogSql = String.Format(@"insert into ChatMessage (CshatMsgID,UserName,Message,SendTime,HubConnectionId,IP)
                values('{0}','{1}','{2}','{3}','{4}','{5}');", Guid.NewGuid().ToString(), messageModel.userName, messageModel.Message,
                messageModel.datetimeNow, Context.ConnectionId, "0.0.0.0");
            Boolean logResult = DataAccess.Instance.PostData(chatLogSql);
            await Clients.Others.ReceiveMessage(messageModel);
        }
    }
}