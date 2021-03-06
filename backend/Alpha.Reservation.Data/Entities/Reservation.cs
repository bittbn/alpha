﻿using System;

namespace Alpha.Reservation.Data.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }       

        public bool? IsConfirmed { get; set; }

        public Guid UserId { get; set; }

        public Guid RoomId { get; set; }
        
        public virtual User User { get; set; }
        
        public virtual Room Room { get; set; }

        public Reservation()
        {
            IsConfirmed = null;
        }
    }
}