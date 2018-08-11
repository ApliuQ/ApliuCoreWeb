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
            MessageModel messageModel = new MessageModel
            {
                username = MemoryCacheCore.GetValue(Context.ConnectionId)?.ToString(),
                message = Message
            };
            await Clients.All.ReceiveMessage(messageModel);
        }

        [HubMethodName("chatLogin")]
        public void ChatLogin(string userName, String password)
        {
            //Context.Items.Add(Context.ConnectionId, userName);//只是当前连接共享数据
            MemoryCacheCore.SetValue(Context.ConnectionId, userName);
        }

        [HubMethodName("sendOthersMessage")]
        public override async Task SendOthersMessage(string Message)
        {
            MessageModel messageModel = new MessageModel
            {
                username = MemoryCacheCore.GetValue(Context.ConnectionId)?.ToString(),
                message = Message
            };

            String chatLogSql = String.Format(@"insert into ChatMessage (ChatMsgID,UserName,Message,SendTime,HubConnectionId,IP)
                values('{0}','{1}','{2}','{3}','{4}','{5}');", Guid.NewGuid().ToString(), messageModel.username, messageModel.message,
                messageModel.datetimenow, Context.ConnectionId, Context.GetHttpContext().GetClientIP());
            Boolean logResult = DataAccess.Instance.PostData(chatLogSql);
            await Clients.Others.ReceiveMessage(messageModel);
        }
    }
}