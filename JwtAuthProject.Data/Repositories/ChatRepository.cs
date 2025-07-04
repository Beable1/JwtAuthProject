using JwtAuthProject.Core.Models;
using JwtAuthProject.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Data.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        public ChatRepository(Context context) : base(context)
        {
        }

        public async Task<List<Chat>> GetChatsAsync(string userId, string toUserId)
        {
             return await _context.Chats.Where(x=>x.UserId==userId&&x.ToUserId==toUserId||x.ToUserId==userId&&x.UserId==toUserId)
                .OrderBy(y=>y.Date).ToListAsync();
        }
    }
}
