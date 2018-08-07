using Apliu.Standard.WeChat.Helper;
using Apliu.Standard.WeChat.Modal;
using Apliu.Standard.WeChat.Modal.Response;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Apliu.Standard.WeChat
{
    public class WeChatClient
    {
        /// <summary>
        /// 操作微信的实例对象
        /// </summary>
        public WeChatHelper weChatHelper;
        /// <summary>
        /// 所有联系人信息
        /// </summary>
        private Dictionary<string, Contact> contactDict;
        /// <summary>
        /// 微信登陆二维码
        /// </summary>
        private Image weChatLoginQRCode;
        public WeChatClient()
        {
            weChatHelper = new WeChatHelper();
            contactDict = new Dictionary<string, Contact>();

            //发生异常通知
            //weChatHelper.ExceptionCatched += Client_ExceptionCatched;
            //获取登陆二维码 改为主动获取
            //weChatHelper.GetLoginQrCodeComplete += Client_GetLoginQrCodeComplete;
            //weChatHelper.CheckScanComplete += Client_CheckScanComplete;
            //weChatHelper.LoginComplete += Client_LoginComplete;
            //weChatHelper.BatchGetContactComplete += Client_BatchGetContactComplete;
            //weChatHelper.GetContactComplete += Client_GetContactComplete;
            //weChatHelper.MPSubscribeMsgListComplete += Client_MPSubscribeMsgListComplete;
            //weChatHelper.LogoutComplete += Client_LogoutComplete;
            //weChatHelper.ReceiveMsg += Client_ReceiveMsg;
            //weChatHelper.DelContactListComplete += Client_DelContactListComplete;
            //weChatHelper.ModContactListComplete += Client_ModContactListComplete;

            //weChatHelper.Start();
        }

        /// <summary>
        /// 启动微信助手
        /// </summary>
        public void Start()
        {
            if (weChatHelper != null) weChatHelper.Start();
        }

        /// <summary>
        /// 退出当前微信账号
        /// </summary>
        public void Logout()
        {
            if (weChatHelper != null) weChatHelper.LogoutAsync();
        }

        /// <summary>
        /// 获取微信登陆二维码
        /// </summary>
        /// <returns></returns>
        public Image GetWeChatLoginQRCode()
        {
            return weChatHelper.GetLoginQrCode();
        }

        /// <summary>
        /// 成功获取微信登陆二维码之后的通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_GetLoginQrCodeComplete(object sender, TEventArgs<Image> e)
        {
            weChatLoginQRCode = e.Result;
        }

        private void Client_ModContactListComplete(object sender, TEventArgs<List<Contact>> e)
        {
            Console.WriteLine("接收修改联系人信息");
            foreach (var item in e.Result)
            {
                contactDict[item.UserName] = item;
            }
        }

        private void Client_DelContactListComplete(object sender, TEventArgs<List<DelContactItem>> e)
        {
            Console.WriteLine("接收删除联系人信息");
        }

        private void Client_ReceiveMsg(object sender, TEventArgs<List<AddMsg>> e)
        {
            foreach (var item in e.Result)
            {
                switch (item.MsgType)
                {
                    case MsgType.MM_DATA_TEXT:
                        if (contactDict.Keys.Contains(item.FromUserName))
                        {
                            if (item.FromUserName.StartsWith("@@"))
                            {
                                //群消息，内容格式为[群内username];<br/>[content]，例如Content=@ffda8da3471b87ff22a6a542c5581a6efd1b883698db082e529e8e877bef79b6:<br/>哈哈
                                string[] content = item.Content.Split(new string[] { ":<br/>" }, StringSplitOptions.RemoveEmptyEntries);
                                Console.WriteLine(contactDict[item.FromUserName].NickName + "：" + contactDict[item.FromUserName].MemberDict[content[0]].NickName + "：" + content[1]);
                            }
                            else
                            {
                                Console.WriteLine(contactDict[item.FromUserName].NickName + "：" + item.Content);
                            }
                        }
                        else
                        {
                            //不包含（一般为群）则需要我们主动拉取信息
                            weChatHelper.GetBatchGetContactAsync(item.FromUserName);
                        }

                        //自动回复
                        if (item.Content == "666666")
                        {
                            weChatHelper.SendMsgAsync("双击666！", item.FromUserName);
                        }
                        break;
                    case MsgType.MM_DATA_HTML:
                        break;
                    case MsgType.MM_DATA_IMG:
                        break;
                    case MsgType.MM_DATA_PRIVATEMSG_TEXT:
                        break;
                    case MsgType.MM_DATA_PRIVATEMSG_HTML:
                        break;
                    case MsgType.MM_DATA_PRIVATEMSG_IMG:
                        break;
                    case MsgType.MM_DATA_VOICEMSG:
                        break;
                    case MsgType.MM_DATA_PUSHMAIL:
                        break;
                    case MsgType.MM_DATA_QMSG:
                        break;
                    case MsgType.MM_DATA_VERIFYMSG:
                        //自动加好友，日限额80个左右，请勿超限额多次调用，有封号风险
                        //weChatHelper.VerifyUser(item.RecommendInfo);
                        break;
                    case MsgType.MM_DATA_PUSHSYSTEMMSG:
                        break;
                    case MsgType.MM_DATA_QQLIXIANMSG_IMG:
                        break;
                    case MsgType.MM_DATA_POSSIBLEFRIEND_MSG:
                        break;
                    case MsgType.MM_DATA_SHARECARD:
                        break;
                    case MsgType.MM_DATA_VIDEO:
                        break;
                    case MsgType.MM_DATA_VIDEO_IPHONE_EXPORT:
                        break;
                    case MsgType.MM_DATA_EMOJI:
                        break;
                    case MsgType.MM_DATA_LOCATION:
                        break;
                    case MsgType.MM_DATA_APPMSG:
                        break;
                    case MsgType.MM_DATA_VOIPMSG:
                        break;
                    case MsgType.MM_DATA_STATUSNOTIFY:
                        switch (item.StatusNotifyCode)
                        {
                            case StatusNotifyCode.StatusNotifyCode_READED:
                                break;
                            case StatusNotifyCode.StatusNotifyCode_ENTER_SESSION:
                                break;
                            case StatusNotifyCode.StatusNotifyCode_INITED:
                                break;
                            case StatusNotifyCode.StatusNotifyCode_SYNC_CONV:
                                //初始化的时候第一次sync会返回最近聊天的列表
                                weChatHelper.GetBatchGetContactAsync(item.StatusNotifyUserName);
                                break;
                            case StatusNotifyCode.StatusNotifyCode_QUIT_SESSION:
                                break;
                            default:
                                break;
                        }
                        break;
                    case MsgType.MM_DATA_VOIPNOTIFY:
                        break;
                    case MsgType.MM_DATA_VOIPINVITE:
                        break;
                    case MsgType.MM_DATA_MICROVIDEO:
                        break;
                    case MsgType.MM_DATA_SYSNOTICE:
                        break;
                    case MsgType.MM_DATA_SYS:
                        //系统消息提示，例如完成好友验证通过，建群等等，提示消息“以已经通过了***的朋友验证请求，现在可以开始聊天了”、“加入了群聊”
                        //不在字典，说明是新增，我们就主动拉取加入联系人字典
                        if (!contactDict.Keys.Contains(item.FromUserName))
                        {
                            weChatHelper.GetBatchGetContactAsync(item.FromUserName);
                        }
                        break;
                    case MsgType.MM_DATA_RECALLED:
                        break;
                    default:
                        break;
                }
            }
        }

        private void Client_LogoutComplete(object sender, TEventArgs<User> e)
        {
            Console.WriteLine("已登出");
            //System.Net.Mime.MediaTypeNames.Application.Exit();
        }

        private void Client_MPSubscribeMsgListComplete(object sender, TEventArgs<List<MPSubscribeMsg>> e)
        {
            Console.WriteLine("获取公众号文章，总数：" + e.Result.Count);
        }

        private void Client_GetContactComplete(object sender, TEventArgs<List<Contact>> e)
        {
            Console.WriteLine("获取联系人列表（包括公众号，联系人），总数：" + e.Result.Count);
            foreach (var item in e.Result)
            {
                if (!contactDict.Keys.Contains(item.UserName))
                {
                    contactDict.Add(item.UserName, item);
                }

                //联系人列表中包含联系人，公众号，可以通过参数做区分
                if (item.VerifyFlag != 0)
                {
                    //个人号
                }
                else
                {
                    //公众号
                }
            }
            //如果获取完成
            if (weChatHelper.IsFinishGetContactList)
            {

            }
        }

        private void Client_BatchGetContactComplete(object sender, TEventArgs<List<Contact>> e)
        {
            Console.WriteLine("拉取联系人信息，总数：" + e.Result.Count);
            foreach (var item in e.Result)
            {
                if (!contactDict.Keys.Contains(item.UserName))
                {
                    contactDict.Add(item.UserName, item);
                }
            }
        }

        private void Client_LoginComplete(object sender, TEventArgs<User> e)
        {
            Console.WriteLine("登陆成功：" + e.Result.NickName);
        }

        private void Client_CheckScanComplete(object sender, TEventArgs<Image> e)
        {
            Console.WriteLine("用户已扫码");
            //返回头像
            //qrForm.SetPic(e.Result);
        }

        /// <summary>
        /// 所有的异常通知事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_ExceptionCatched(object sender, TEventArgs<Exception> e)
        {
            if (e.Result is GetContactException)
            {
               // Logger.WriteLog("微信客户端获取好友列表异常：" + e.Result.ToString());
            }
            else if (e.Result is OperateFailException)
            {
               // Logger.WriteLog("微信客户端异步操作异常：" + e.Result.ToString());
            }
            else
            {
               // Logger.WriteLog("微信客户端发生异常：" + e.Result.ToString());
            }
        }
    }
}
