﻿using System;
using System.Collections.Generic;

namespace Alpha.Reservation.Data.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public bool Projector { get; set; }

        public bool Board { get; set; }

        public int Seat { get; set; }

        public string Description { get; set; }
        
        public virtual ICollection<Reservation> Reservations { get; set; }

        public Room()
        {
            Reservations = new List<Reservation>();
        }
    }
}