using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.DataBase;
using MimicAPI.Helpers;
using MimicAPI.Model;
using MimicAPI.Model.DTO;
using MimicAPI.Repositories.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controller
{
    [Route("api/Palavras")]
    public class PalavrasController : ControllerBase
    {
        private readonly IPalavraRepository _repository;
        private readonly IMapper _mapper;

        //Retorna a instancia do banco de dados
        public PalavrasController(IPalavraRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //WEB -- /api/palavras
        [Route("")]
        [HttpGet]
        public ActionResult BuscarTodas([FromQuery]PalavraUrlQuery query)
        {
            var palavras = _repository.BuscarTodas(query);

            if (query.NumeroPagina > palavras.Paginacao.TotalPaginas)
                return NotFound();

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(palavras.Paginacao));

            return new JsonResult(palavras);
        }

        //WEB -- /api/palavras/1
        [Route("{id}")]
        [HttpGet]
        public ActionResult Buscar(int id)
        {
            var palavra = _repository.Buscar(id);

            if (palavra == null)
                return NotFound();

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);
            palavraDTO.Links = new List<LinkDTO>();
            palavraDTO.Links.Add(
                new LinkDTO("self", $"https://localhost:44342/api/palavras/{palavraDTO.Id}", "GET")
                );

            return new JsonResult(palavraDTO);
        }

        //WEB -- /api/palavras (POST: id, nome, ativo, pontuacao, datacriacao)
        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {
            _repository.Cadastrar(palavra);

            return Created($"api/palavras/{palavra.Id}", palavra);
        }

        //WEB -- /api/palavras/1 (PUT: id, nome, ativo, pontuacao, datacriacao)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody]Palavra palavra)
        {
            var palavraEncontrada = _repository.Buscar(id);

            if (palavraEncontrada == null)
                return NotFound();

            palavra.Id = id;
            palavra.DataAtualizacao = DateTime.Now;

            _repository.Atualizar(palavra);

            return Ok();
        }

        //WEB -- /api/palavras/1 (DELETE: id, nome, ativo, pontuacao, datacriacao)
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(int id)
        {
            var palavraEncontrada = _repository.Buscar(id);

            if (palavraEncontrada == null)
                return NotFound();

            _repository.Deletar(id);

            return NoContent();
        }

    }
}
