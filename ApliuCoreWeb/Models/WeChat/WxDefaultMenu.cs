using Apliu.Standard.Tools;
using Apliu.Standard.Tools.Web;
using System;

namespace ApliuCoreWeb.Models.WeChat
{
    public class WxDefaultMenu
    {
        /// <summary>
        /// 创建微信公众号自定义菜单
        /// </summary>
        public static async void CreateMenus()
        {
            string reqUri = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + WxTokenManager.AccessToken;
            string menuStr = @"{{
    ""button"": [
        {{
            ""type"": ""view"",
            ""name"": ""主页"",
            ""key"": ""{{0}}""
        }},
        {{
            ""name"": ""小工具"",
            ""sub_button"": [
                {{
                    ""type"": ""view"",
                    ""name"": ""临时文本"",
                    ""url"": ""{{0}}/text""
                }},
                {{
                    ""type"": ""view"",
                    ""name"": ""在线画板"",
                    ""url"": ""{{0}}/tool/drawing""
                }},
                {{
                    ""type"": ""view"",
                    ""name"": ""字符串处理"",
                    ""url"": ""{{0}}/tool/security""
                }}
            ]
        }},
        {{
            ""name"": ""小游戏"",
            ""sub_button"": [
                {{
                    ""type"": ""view"",
                    ""name"": ""见缝插针"",
                    ""url"": ""{{0}}/game/pins""
                }},
                {{
                    ""type"": ""pic_photo_or_album"",
                    ""name"": ""拍照或者相册发图"",
                    ""key"": ""rselfmenu_1_1"",
                    ""sub_button"": [

                    ]
                }},
                {{
                    ""type"": ""miniprogram"",
                    ""name"": ""wxa"",
                    ""url"": ""http://mp.weixin.qq.com"",
                    ""appid"": ""wx286b93c14bbf93aa"",
                    ""pagepath"": ""pages/lunar/index""
                }},
                {{
                    ""name"": ""发送位置"",
                    ""type"": ""location_select"",
                    ""key"": ""rselfmenu_2_0""
                }},
                {{
                    ""type"": ""view_limited"",
                    ""name"": ""图文消息"",
                    ""media_id"": ""MEDIA_ID2""
                }},
                {{
                    ""type"": ""click"",
                    ""name"": ""赞一下我们"",
                    ""key"": ""V1001_GOOD""
                }}
            ]
        }}
    ]
}}";
            //""url"":""https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={0}%2Fwx%2Fbind.html&response_type=code&scope=snsapi_base&state=%2Fwx%2Frepay.html""
            String defaultMenuContent = string.Format(menuStr, WeChatBase.WxDomain);//HttpUtility.UrlEncode(WeChatBase.WxDomain)

            System.Net.Http.HttpResponseMessage response = await HttpRequestHelper.HttpPostAsync(reqUri, WeChatBase.WxEncoding, defaultMenuContent);
            String content = await response.Content.ReadAsStringAsync();
            Logger.WriteLogAsync("微信公众号创建自定义菜单完成，详情：" + content);
        }
    }
}