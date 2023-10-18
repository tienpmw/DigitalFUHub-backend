﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Conversation
{
    public class ConversationResponseDTO
    {
        public long ConversationId { get; set; }
        public string? ConversationName { get; set; }
        public DateTime DateCreate { get; set; }
        public bool IsActivate { get; set; }
        public int IsRead { get; set; }
        public string LatestMessage { get; set; } = string.Empty;
        public ICollection<UserConversationResponseDTO>? Users { get; set; }

    }
}
