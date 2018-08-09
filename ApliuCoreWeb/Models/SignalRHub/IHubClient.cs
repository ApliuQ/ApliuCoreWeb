using Apliu.Standard.Tools;
using System;
using System.Threading.Tasks;

namespace ApliuCoreWeb.Models.SignalRHub
{
    /// <summary>
    /// 消息体
    /// </summary>
    public class MessageModel
    {
        public String username;
        public String message;
        public String datetimenow = TimeHelper.DataTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// 客户端方法
    /// </summary>
    public interface IHubClient
    {
        /// <summary>
        /// 客户端接收消息
        /// </summary>
        /// <param name="messageModel"></param>
        Task ReceiveMessage(MessageModel messageModel);

        /// <summary>
        /// 用户上线通知
        /// </summary>
        /// <param name="userName"></param>
        Task OnConnectedAsync(String userName);

        /// <summary>
        /// 用户掉线/离线通知
        /// </summary>
        /// <param name="userName"></param>
        Task OnDisconnectedAsync(String userName);
    }
}
