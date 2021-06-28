using AutoMapper;
using MimicAPI.Model;
using MimicAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            /*
             AutoMapper

            PalavraDTO > Palavra
            palavraDTO.ID = palavra.Id
            palavraDTO.Nome = palavra.Nome
            .
            .
            .
            .
             */

            CreateMap<Palavra, PalavraDTO>();
        }
    }
}
