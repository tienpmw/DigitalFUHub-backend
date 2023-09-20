﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MessageId { get; set; }
        public long UserId { get; set; }
        public long ConversationId { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool isImage { get; set; }
        public DateTime DateCreate { get; set; }
        public bool isDelete { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(ConversationId))]
        public Conversation Conversation { get; set; } = null!;

        public virtual ICollection<MessageImage>? MessageImages { get; set; }
    }
}
