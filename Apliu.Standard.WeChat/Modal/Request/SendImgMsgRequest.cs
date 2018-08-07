namespace Apliu.Standard.WeChat.Modal.Request
{
    class SendMediaMsgRequest
    {
        /// <summary>
        /// BaseRequest
        /// </summary>
        public BaseRequest BaseRequest { get; set; }
        /// <summary>
        /// Msg
        /// </summary>
        public MediaMsg Msg { get; set; }
        /// <summary>
        /// Scene
        /// </summary>
        public int Scene { get; set; }
    }
}
