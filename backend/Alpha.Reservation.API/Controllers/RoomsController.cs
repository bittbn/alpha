using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Reservation.App.Models.RoomModels;
using Alpha.Reservation.App.Services.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Reservation.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public RoomsController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<RoomModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<RoomModel>>(await _roomService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<RoomModel> Get(Guid id)
        {
            return _mapper.Map<RoomModel>(await _roomService.GetAsync(id));
        }

        [HttpGet("WithDetails")]
        public async Task<List<RoomWithDetailsModel>> GetWithDetails()
        {
            return _mapper.Map<List<RoomWithDetailsModel>>(await _roomService.GetWithDetailsAsync());
        }
        
        [HttpGet("WithDetails/{id}")]
        public async Task<RoomWithDetailsModel> GetWithDetailsById([FromRoute] Guid id)
        {
            return _mapper.Map<RoomWithDetailsModel>(await _roomService.GetWithDetailsAsync(id));
        }

        [Authorize(Roles = "Office Manager")]
        [HttpPost]
        public async Task<RoomModel> Post([FromBody] ShortRoomModel roomModel)
        {
            return _mapper.Map<RoomModel>(await _roomService.AddRoomAsync(roomModel));
        }

        [Authorize(Roles = "Office Manager")]
        [HttpPut("{id}")]
        public async Task<RoomModel> Put([FromRoute] Guid id, [FromBody] ShortRoomModel roomModel)
        {
            return _mapper.Map<RoomModel>(await _roomService.UpdateRoomAsync(id, roomModel));
        }

        [Authorize(Roles = "Office Manager")]
        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] Guid id)
        {
            await _roomService.DeleteAsync(id);
        }
    }
}