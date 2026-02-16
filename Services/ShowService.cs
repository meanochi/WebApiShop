using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTOs;
using Entities;
using Repositories;

namespace Services
{
    public class ShowService : IShowService
    {
        IShowsRepository _repository;
        IMapper _mapper;
        public ShowService(IShowsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
        public async Task<ShowReadDTO> addShow(ShowCreateDTO showCDTO)
        {
            Show show = _mapper.Map<ShowCreateDTO, Show>(showCDTO);
            show = await _repository.addShow(show);
            ShowReadDTO showDTO = _mapper.Map<Show, ShowReadDTO>(show);
            return showDTO;
        }
        public async Task<ShowReadDTO> updateShow(ShowUpdateDTO showUDTO, int id)
        {
            Show show = _mapper.Map<ShowUpdateDTO, Show>(showUDTO);
            show = await _repository.updateOrder(show, id);
            ShowReadDTO showDTO = _mapper.Map<Show, ShowReadDTO>(show);
            return showDTO;
        }
        public async Task<(IEnumerable<ShowReadDTO> shows, int total)> getAllShows(string? description, int? minPrice, int? maxPrice, int skip, int position, int[]? categoryId)
        {
            (IEnumerable<Show> shows, int total) shows = await _repository.getAllShows(description, minPrice, maxPrice, skip, position, categoryId);
            IEnumerable<ShowReadDTO> showsDTO = _mapper.Map< IEnumerable<Show>, IEnumerable<ShowReadDTO>>(shows.shows);
            return (showsDTO, shows.total);
        }

    }
}
