using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
namespace OPTFS.RealtimeChat
{
    public class SignalRClient
    {
        HubConnection connection;        

        static SignalRClient Instance;

        public static SignalRClient GetInstance
        {
            get
            {
                if (Instance == null)
                    Instance = new SignalRClient();
                return Instance;
            }
        }

        public SignalRClient()
        {
            ConnectToServer();
        }

        // الاتصال بالسيرفر
        public async void ConnectToServer()
        {
            /*connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7163/chatHub")
                .Build();

            await connection.StartAsync();            

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };*/

            // keep trying until we manage to connect
            while (true)
            {
                try
                {
                    connection = new HubConnectionBuilder().WithUrl("https://localhost:7163/chatHub").Build();
                    await connection.StartAsync();
                    return; // yay! connected
                }
                catch (Exception e) { /* bugger! */}
            }
        }

        
        public async void SendToUser(string FromUser, string ToUser, int messageId)
        {
            try
            {
                var connectionstate = connection.State;
                if (connectionstate == HubConnectionState.Disconnected)
                    ConnectToServer();
                await connection.InvokeAsync("SendToUser", FromUser, ToUser, messageId);
            }
            catch (Exception Ex)
            {
                string ExMessage = Ex.Message, ExString = Ex.ToString();
            }
        }        
    }
}