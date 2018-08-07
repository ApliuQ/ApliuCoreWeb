using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ApliuCoreWeb.Models.SignalRHub
{
    /// <summary>
    /// Hub名称区分大小写（如未设定，则驼峰命名格式，客户端js里面首字母小写）
    /// </summary>
    public class ServerHub : Hub
    {
        public void Hello()
        {
            Clients.All.SendAsync("SendAsync Hello");
        }

        public void Remark()
        {
            //Server 端 方法名区分大小写（如未设定，则驼峰命名格式，客户端js里面首字母小写）
            //Client 端 方法名不区分大小写

            //throw new HubException("This message will flow to the client", new { user = Context.User.Identity.Name, message = message });
            //base.OnDisconnected();连接断开/重连/连接时的触发事件
            //Context.ConnectionId//当前连接的HubCallerContext对象 的 链接ID 多个 Hubs也会共用同一个 connection ID .
            //js 客户端id   $.connection.hub.id

            //Groups.Add(Context.ConnectionId, groupName);//客户端组对象
            //System.Security.Principal.IPrincipal user = Context.User; 用户信息
            //Js客户端调用服务端方法：testHub.server.SendMsg("message");

            //System.Collections.Generic.IDictionary<string, Cookie> cookies = Context.Request.Cookies;//Cookies信息
            //System.Web.HttpContextBase httpContext = Context.Request.GetHttpContext();//Request 的 HttpContext 对象
            //js-》contosoChatHubProxy.state.userName = "Fadi Fakhouri"; server-》string userName = Clients.Caller.userName;

            //所有连接的客户端 Clients.All.addContosoChatMessageToPage(name, message);
            //只是 Calling 的客户端 Clients.Caller.addContosoChatMessageToPage(name, message);
            //所有连接的客户端除了 Calling 的客户端 Clients.Others.addContosoChatMessageToPage(name, message);
            //通过 connection ID 指定特定的客户端 Clients.Client(Context.ConnectionId).addContosoChatMessageToPage(name, message);
            //通过 connection ID 排除特定的客户端 Clients.AllExcept(connectionId1, connectionId2).addContosoChatMessageToPage(name, message);
            //指定一个特殊组 Clients.Group(groupName).addContosoChatMessageToPage(name, message);
            //指定一个特殊组，并且排除特定 connection ID 的客户端 Clients.Group(groupName, connectionId1, connectionId2).addContosoChatMessageToPage(name, message);
            //指定一个特殊组，但是排除 calling Clients.OthersInGroup(groupName).addContosoChatMessageToPage(name, message);
            //通过 userId 指定一个特殊的用户，一般情况下是 IPrincipal.Identity.Name Clients.User(userid).addContosoChatMessageToPage(name, message);
            //在一个 connection IDs 列表里的所有客户端和组 Clients.Clients(ConnectionIds).broadcastMessage(name, message);
            //指定多个组 Clients.Groups(GroupIds).broadcastMessage(name, message);
            //通过用户名 Clients.Client(username).broadcastMessage(name, message);
            //一组用户名  Clients.Users(new string[] { "myUser", "myUser2" }).broadcastMessage(name, message);

            //IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<StockTickerHub>();  hub类方法外面调用hub方法
            //_context.Clients.All.updateStockPrice(stock);


            //generated proxy --》var contosoChatHubProxy = $.connection.testHub;--》$.connection.hub.start()
            //非 generated proxy-->var connection = $.hubConnection(); var contosoChatHubProxy = connection.createHubProxy('testHub');--》connection.start()
            //如果你要给客户端的方法注册多个事件处理器，那么你就不能使用 generated proxy。如果你不使用 generated proxy ，那么你就不能引用 "signalr/hubs" URL。
            //ASP.NET Web Forms  引用 <script src='<%: ResolveClientUrl("~/signalr/hubs") %>'></script>
            //MVC 引用 <script src="~/signalr/hubs"></script>
            //$.connection.hub.start().done(functiong(){        }).fail(functiong(){        });//除了在启动的时候可以设置，在调用方法的时候也可以设置失败回调

            //throw new HubException("This message will flow to the client", new { user = Context.User.Identity.Name, message = message });抛出hub异常

            //$.connection.hub.connectionSlow    connection.connectionSlow(function () {
            //            starting: 在任何数据发送之前执行。
            //received: 当任何数据通过连接获取到的时候执行。可以得到数据。
            //connectionSlow: 当客户端检测到缓慢或者不流畅的连接的时候执行。 
            //reconnecting: 当潜在的协议重新开始连接的时候执行。 
            //reconnected: 当潜在的协议以及重新建立连接的时候执行。
            //stateChanged: 当连接的状态发生改变的时候执行。可以提供一个旧的和新的状态（Connecting, Connected, Reconnecting, 或者 Disconnected）。
            //disconnected: 当连接中断以后执行。

            //开启客户端日志
            //            $.connection.hub.logging = true;
            //$.connection.hub.start();

            //其他客户端连接指定地址的signalR Hub
            //var hubConnection = new HubConnection("http://www.contoso.com/");
            //IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("StockTickerHub");
            //stockTickerHubProxy.On<Stock>("UpdateStockPrice", stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Symbol, stock.Price));
            //await hubConnection.Start();
        }
    }
}