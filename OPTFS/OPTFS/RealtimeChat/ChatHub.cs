using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace OPTFS.RealtimeChat
{
    public class ChatHub : Hub
    {        
        // عند الاتصال
        public override Task OnConnectedAsync()
        {
            try
            {
                var UserId = Context?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var ConnectionId = Context?.ConnectionId;
                var Connection = ChatVariables.ConnectedUsers.SingleOrDefault(CurrentConnection => CurrentConnection.ConnectionId == ConnectionId);
                if (Connection == null)
                    ChatVariables.ConnectedUsers.Add(new ChatUserInfo
                    {
                        ConnectionId = ConnectionId,
                        UserId = UserId,
                        UserInfo = Context?.User,
                    });
            }
            catch (Exception Ex)
            {
                string ExMessage = Ex.Message, ExString = Ex.ToString();
            }
            return base.OnConnectedAsync();
        }

        // عند قطع الاتصال
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                // حذف بيانات المستخدم من قائمة المتصلين
                ChatVariables.ConnectedUsers.RemoveAll(CurrentConnection => CurrentConnection.ConnectionId == Context.ConnectionId);
            }
            catch (Exception Ex)
            {
                string ExMessage = Ex.Message, ExString = Ex.ToString();
            }
            return base.OnDisconnectedAsync(exception);
        }

        // ارسال اشعار من مستخدم موبايل الى مستخدم موبايل
        public async void SendToUser(string FromUser, string ToUser, int messageId)
        {
            try
            {
                var Receiver = ChatVariables.ConnectedUsers.FirstOrDefault(CurrentConnection => CurrentConnection.UserId == ToUser);
                if (Receiver != null)
                {
                    await Clients.Client(Receiver.ConnectionId).SendAsync("ReceivePrivateMessage", FromUser, messageId);
                    await Clients.Client(Receiver.ConnectionId).SendAsync("UpdateMessageBadge", FromUser, messageId);
                }

                var Sender = ChatVariables.ConnectedUsers.FirstOrDefault(CurrentConnection => CurrentConnection.UserId == FromUser);                
                if (Sender != null)
                    await Clients.Client(Sender.ConnectionId).SendAsync("ReceivePrivateMessage", ToUser, messageId);
            }
            catch (Exception Ex)
            {
                string ExMessage = Ex.Message, ExString = Ex.ToString();
            }
        }
        
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}