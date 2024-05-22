using System.Security.Claims;

namespace OPTFS.RealtimeChat
{
    public class ChatUserInfo
    {
        public string? UserId { get; set; }
        public string? ConnectionId { get; set; }
        public ClaimsPrincipal? UserInfo { get; set; }
    }
}
