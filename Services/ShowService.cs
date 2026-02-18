using AutoMapper;
using DTOs;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ShowService : IShowService
    {
        IAuth _auth;
        IShowsRepository _repository;
        IMapper _mapper;
        public ShowService(IShowsRepository repository, IMapper mapper, IAuth auth)
        {
            _repository = repository;
            _mapper = mapper;
            _auth = auth;
        }

        public async Task<List<ShowReadDTO>> getAllShows()
        {
            List<Show> shows = await _repository.getAllShows();
            List<ShowReadDTO> showsDTO = _mapper.Map<List<Show>, List<ShowReadDTO>>(shows);
            return showsDTO;
        }
        public async Task<ShowReadDTO> getShowById(int id)
        {
            Show show = await _repository.getShowById(id);
            ShowReadDTO showDTO = _mapper.Map<Show, ShowReadDTO>(show);
            return showDTO;
        }
        public async Task<ShowReadDTO?> addShow(ShowCreateDTO showCDTO, int userId)
        {
            if ( !await _auth.IsManager(userId))
                return null;
            Show show = _mapper.Map<ShowCreateDTO, Show>(showCDTO);
            show = await _repository.addShow(show);
            ShowReadDTO showDTO = _mapper.Map<Show, ShowReadDTO>(show);
            return showDTO;
        }
        public async Task<ShowReadDTO?> updateShow(ShowUpdateDTO showUDTO, int id, int userId)
        {
            if (!await _auth.IsManager(userId))
                return null;
            Show show = _mapper.Map<ShowUpdateDTO, Show>(showUDTO);
            show = await _repository.updateOrder(show, id);
            ShowReadDTO showDTO = _mapper.Map<Show, ShowReadDTO>(show);
            return showDTO;
        }
        public async Task<(IEnumerable<ShowReadDTO> shows, int total)> getAllShows(string? description, int? minPrice, int? maxPrice, int skip, int position, int[] categoryId, string[] sectors, string[] audiences)
        {
            (IEnumerable<Show> shows, int total) shows = await _repository.getAllShows(description, minPrice, maxPrice, skip, position, categoryId, sectors, audiences);
            IEnumerable<ShowReadDTO> showsDTO = _mapper.Map< IEnumerable<Show>, IEnumerable<ShowReadDTO>>(shows.shows);
            return (showsDTO, shows.total);
        }

    }
}
