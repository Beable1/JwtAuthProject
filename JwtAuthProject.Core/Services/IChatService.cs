using JwtAuthProject.Core.Dtos;
using JwtAuthProject.Core.Models;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Core.Services
{
    public interface IChatService
    {
        Task<Response<List<Chat>>> GetChatsAsync(string userId, string toUserId);

    }
}
