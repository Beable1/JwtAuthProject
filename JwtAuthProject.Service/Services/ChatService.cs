using JwtAuthProject.Core.Dtos;
using JwtAuthProject.Core.Models;
using JwtAuthProject.Core.Repositories;
using JwtAuthProject.Core.Services;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Service.Services
{
    public class ChatService : IChatService
    {
     
         private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<Response<List<Chat>>> GetChatsAsync(string userId, string toUserId)
        {
            var chats=await _chatRepository.GetChatsAsync(userId, toUserId);
            return Response<List<Chat>>.Success(chats, 200);
        }
    }
}
