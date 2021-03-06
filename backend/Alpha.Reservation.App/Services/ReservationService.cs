using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Reservation.App.Models.ReservationModels;
using Alpha.Reservation.App.Services.Contracts;
using Alpha.Reservation.Data;
using Alpha.Reservation.Data.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Reservation.App.Services
{
    public class ReservationService : RepositoryBase<Data.Entities.Reservation, DatabaseContext>, IReservationService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        
        public ReservationService(DatabaseContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Data.Entities.Reservation> GetWithDetailsAsync(Guid id)
        {
            var reservation = await _context.Reservations
                .Include(a => a.User)
                .Include(a => a.Room)
                .SingleOrDefaultAsync(a => a.Id == id);

            return reservation;
        }
        
        

        public async Task<Data.Entities.Reservation> UpdateConfirmationAsync(Guid id, bool confirmation)
        {
            var reservation = await GetAsync(id);            
            reservation.IsConfirmed = confirmation;
            
            return await UpdateAsync(reservation);
        }

        public List<Data.Entities.Reservation> GetAllWithOrder()
        {
            return _context.Reservations
                .AsNoTracking()
                .AsEnumerable()
                .Where(IsValidReservationDate)
                .OrderBy(a => a.BeginTime.Date)
                .ThenBy(a => a.BeginTime.TimeOfDay)
                .ToList();
        }

        public async Task<Data.Entities.Reservation> AddReservationAsync(CreateReservationModel reservationModel)
        {
            var reservation = _mapper.Map<Data.Entities.Reservation>(reservationModel);            
            return await AddAsync(reservation);
        }

        public async Task<Data.Entities.Reservation> UpdateReservationAsync(Guid id, 
            ShortReservationModel reservationModel)
        {
            if (DateTime.Now.Date > reservationModel.BeginTime.Date)
                throw new Exception("Past Date");

            if (reservationModel.BeginTime.TimeOfDay >= reservationModel.EndTime.TimeOfDay)
                throw new Exception("Incorrect time");
            
            var reservation = await GetAsync(id);

            reservation.Title = reservationModel.Title;
            reservation.Description = reservationModel.Description;
            reservation.IsConfirmed = reservationModel.IsConfirmed;
            reservation.BeginTime = reservationModel.BeginTime;
            reservation.EndTime = reservationModel.EndTime;
            reservation.RoomId = reservationModel.RoomId;
            reservation.UserId = reservationModel.UserId;

            return await UpdateAsync(reservation);
        }
        
        public bool IsValidReservationDate(Data.Entities.Reservation reservation)
        {
            var currentDateTime = DateTime.Now;
            var checkDate = reservation.BeginTime.Date >= currentDateTime.Date;
            var checkTime =
                reservation.BeginTime.Date != currentDateTime.Date ||
                reservation.EndTime.TimeOfDay >= currentDateTime.TimeOfDay;

            return checkDate && checkTime;
        }
    }
}