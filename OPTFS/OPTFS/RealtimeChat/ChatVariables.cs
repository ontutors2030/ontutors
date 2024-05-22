namespace OPTFS.RealtimeChat
{
    public class ChatVariables
    {
        static List<ChatUserInfo> _ConnectedUsers;
        public static List<ChatUserInfo> ConnectedUsers
        {
            get
            {
                if (_ConnectedUsers == null)                
                    _ConnectedUsers = new List<ChatUserInfo>();
                return _ConnectedUsers;
            }
        }
    }
}
