using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApliuCoreWeb.Models.SignalRHub
{
    /// <summary>
    /// 服务端方法
    /// </summary>
    public abstract class IHubServer : Hub<IHubClient>
    {
        /// <summary>
        /// 向所有客户端发送消息
        /// </summary>
        /// <param name="Message"></param>
        public abstract Task SendAllMessage(String Message);

        /// <summary>
        /// 向除自己之外的客户端发送消息
        /// </summary>
        /// <param name="Message"></param>
        public abstract Task SendOthersMessage(String Message);

        /// <summary>
        /// 上线通知
        /// </summary>
        public override Task OnConnectedAsync()
        {
            String userName = null;// MemoryCacheCore.GetValue(Context.ConnectionId)?.ToString();
            if (userName != null)
            {
                Clients.All.OnConnectedAsync(userName);   //调用客户端用户上线线通知
            }
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 离线通知
        /// </summary>
        public override Task OnDisconnectedAsync(Exception stopCalled)
        {
            String userName = null;// MemoryCacheCore.GetValue(Context.ConnectionId)?.ToString();
            if (userName != null)
            {
                Clients.All.OnDisconnectedAsync(userName);   //调用客户端用户离线通知
                //MemoryCacheCore.Remove(Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(stopCalled);
        }
    }
}
