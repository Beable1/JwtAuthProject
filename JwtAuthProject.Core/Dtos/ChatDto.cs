using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Core.Dtos
{
    public class ChatDto
    {
        public string UserId { get; set; }

        public string ToUserId { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime Date { get; set; }
    }
}
