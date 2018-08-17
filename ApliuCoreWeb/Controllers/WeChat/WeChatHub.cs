using ApliuCoreWeb.Models;
using ApliuCoreWeb.Models.SignalRHub;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ApliuCoreWeb.Controllers.WeChat
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
            await Clients.All.ReceiveMessage(new List<MessageModel>() { messageModel });
        }

        [HubMethodName("chatLogin")]
        public async Task ChatLogin(string userName, String password)
        {
            //Context.Items.Add(Context.ConnectionId, userName);//只是当前连接共享数据
            MemoryCacheCore.SetValue(Context.ConnectionId, userName);

            List<MessageModel> messageModels = new List<MessageModel>();
            String msgSql = "select top 10 UserName,Message from ChatMessage where UserName<>'' and Message<>'' order by SendTime desc;";
            DataTable dataTable = DataAccess.Instance.GetDataTable(msgSql);
            if (dataTable != null)
            {
                foreach (DataRow rowItem in dataTable.Rows)
                {
                    MessageModel msgModel = new MessageModel
                    {
                        username = rowItem["UserName"].ToString(),
                        message = rowItem["Message"].ToString(),
                    };
                    messageModels.Insert(0, msgModel);//消息按时间顺序插入到队列中
                }
                await Clients.Caller.ReceiveMessage(messageModels);
            }
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
            await Clients.Others.ReceiveMessage(new List<MessageModel>() { messageModel });
        }
    }
}