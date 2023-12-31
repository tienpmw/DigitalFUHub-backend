﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Notification
{
    public class NotificationResponeDTO
    {
       public int TotalNotification { get;set; }
	   public ICollection<NotificationDetailResponeDTO>? Notifications { get; set; }	
    } 

    public class NotificationDetailResponeDTO
    {
		public long NotificationId { get; set; }
		public string? Title { get; set; } = null!;
		public string? Content { get; set; } = null!;
		public string? Link { get; set; }
		public DateTime DateCreated { get; set; }
		public bool IsReaded { get; set; }
	}
}
