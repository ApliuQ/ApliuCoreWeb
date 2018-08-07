using System.Collections.Generic;

namespace Apliu.Standard.WeChat.Modal.Response
{
    public class UpdateChatRoomResponse
    {    /// <summary>
         /// BaseResponse
         /// </summary>
        public BaseResponse BaseResponse { get; set; }
        /// <summary>
        /// MemberCount
        /// </summary>
        public int MemberCount { get { return MemberList.Count; } }
        /// <summary>
        /// MemberList
        /// </summary>
        public List<Member> MemberList { get; set; }
    }
}
