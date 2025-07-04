using JwtAuthProject.Core.Models;
using JwtAuthProject.Core.UnitOfWork;
using JwtAuthProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthProject.MyHub
{
    public class MyHub:Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<UserApp> userManager;
        public Dictionary<string, string> Users = new();

        public MyHub(UserManager<UserApp> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task Connect(string userId)
        {
            Users.Add(Context.ConnectionId, userId);
            UserApp? user= await userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if(user is not null)
            {
                user.Status = "online";
                await _unitOfWork.CommitAsync();

                await Clients.All.SendAsync("Users", user);
            }
        }


        public async Task OnDisconnect()
        {
            string userId;
            Users.TryGetValue(Context.ConnectionId, out userId);

            UserApp? user = await userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user is not null)
            {
                user.Status = "offline";
                await _unitOfWork.CommitAsync();

                await Clients.All.SendAsync("Users", user);
            }

        }


        public async Task SendMessageAsync(string message,string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("receiveMessage", message);
        }




        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;

            // Connection ID'yi geri gönderme (isteğe bağlı)
            await Clients.Caller.SendAsync("ReceiveConnectionId", connectionId);

            await base.OnConnectedAsync();
        }

    }
}
