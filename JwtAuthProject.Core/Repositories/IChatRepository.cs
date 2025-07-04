using JwtAuthProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Core.Repositories
{
    public interface IChatRepository:IGenericRepository<Chat>
    {

        Task<List<Chat>> GetChatsAsync(string userId,string toUserId);
    }
}
