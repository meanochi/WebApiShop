using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;
using DTOs;
using AutoMapper;
using Entities;

namespace Services
{
    public class ProviderService : IProviderService
    {
        IProviderRepository _repository;
        IMapper _mapper;
        public ProviderService(IProviderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProviderReadDTO> getProviderById(int id)
        {
            Provider provider = await _repository.getProviderById(id);
            ProviderReadDTO providerDTO = _mapper.Map<Provider, ProviderReadDTO>(provider);
            return providerDTO;
        }

        public async Task<List<ProviderReadDTO>> getAllProviders()
        {
            List<Provider> providers = await _repository.getAllProviders();
            List<ProviderReadDTO> providerDTOs = _mapper.Map<List<Provider>, List<ProviderReadDTO>>(providers);
            return providerDTOs;
        }

        public async Task<ProviderReadDTO> addProvider(ProviderCreateDTO provider)
        {
            Provider newProvider = _mapper.Map<ProviderCreateDTO, Provider>(provider);
            newProvider = await _repository.addProvider(newProvider);
            ProviderReadDTO providerDTO = _mapper.Map<Provider, ProviderReadDTO>(newProvider);
            return providerDTO;
        }

    }
}
