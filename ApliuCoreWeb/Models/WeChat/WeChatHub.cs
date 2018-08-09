using ApliuCoreWeb.Models.SignalRHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
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
            messageModel.username = MemoryCacheCore.GetValue(Context.ConnectionId)?.ToString();
            messageModel.message = Message;
            await Clients.All.ReceiveMessage(messageModel);
        }

        [HubMethodName("chatLogin")]
        public void ChatLogin(string userName, String password)
        {
            MemoryCacheCore.SetValue(Context.ConnectionId, userName);
        }

        [HubMethodName("sendOthersMessage")]
        public override async Task SendOthersMessage(string Message)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.username = MemoryCacheCore.GetValue(Context.ConnectionId)?.ToString();
            messageModel.message = Message;
            String chatLogSql = String.Format(@"insert into ChatMessage (CshatMsgID,UserName,Message,SendTime,HubConnectionId,IP)
                values('{0}','{1}','{2}','{3}','{4}','{5}');", Guid.NewGuid().ToString(), messageModel.username, messageModel.message,
                messageModel.datetimenow, Context.ConnectionId, "0.0.0.0");
            Boolean logResult = DataAccess.Instance.PostData(chatLogSql);
            await Clients.Others.ReceiveMessage(messageModel);
        }
    }
}