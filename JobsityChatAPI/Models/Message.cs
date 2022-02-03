﻿using System;


namespace JobsityChatAPI.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public virtual LocalUser User { get; set; }
    }
}
