using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.DataBase;
using MimicAPI.Helpers;
using MimicAPI.Model;
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
        private readonly MimicContext _banco;

        //Retorna a instancia do banco de dados
        public PalavrasController(MimicContext banco)
        {
            _banco = banco;
        }

        //WEB -- /api/palavras
        [Route("")]
        [HttpGet]
        public ActionResult BuscarTodas([FromQuery]PalavraUrlQuery query)
        { 
            var palavras = _banco.Palavras.AsQueryable();

            if (query.Data.HasValue)
                palavras = palavras.Where(x => x.DataCriacao > query.Data.Value || x.DataAtualizacao > query.Data.Value);
            
            if (query.NumeroPagina.HasValue)
            {
                var quantidadeTotalRegistros = palavras.Count();
                var qntdPalavasPuladas = (query.NumeroPagina.Value - 1) * query.QntRegistrosPagina.Value;
                
                palavras = palavras.Skip(qntdPalavasPuladas).Take(query.QntRegistrosPagina.Value);

                var paginacao = new Paginacao(query.NumeroPagina.Value,
                                              query.QntRegistrosPagina.Value,
                                              quantidadeTotalRegistros);
                                
                paginacao.CalcularTotalDePaginasNecessarias();

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginacao));

                if (query.NumeroPagina > paginacao.TotalPaginas)
                    return NotFound();

            }
            
            return new JsonResult(palavras);
        }

        //WEB -- /api/palavras/1
        [Route("{id}")]
        [HttpGet]
        public ActionResult Buscar(int id)
        {
            var palavra = _banco.Palavras.Find(id);

            if (palavra == null)
                return NotFound();

            return new JsonResult(palavra);
        }

        //WEB -- /api/palavras (POST: id, nome, ativo, pontuacao, datacriacao)
        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {
            palavra.DataCriacao = DateTime.Now;

            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();

            return Created($"api/palavras/{palavra.Id}", palavra);
        }

        //WEB -- /api/palavras/1 (PUT: id, nome, ativo, pontuacao, datacriacao)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody]Palavra palavra)
        {
            var obj = _banco.Palavras.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound();

            palavra.Id = id;
            palavra.DataAtualizacao = DateTime.Now;

            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();

            return Ok();
        }

        //WEB -- /api/palavras/1 (DELETE: id, nome, ativo, pontuacao, datacriacao)
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(int id)
        {
            var palavraEncontrada = _banco.Palavras.Find(id);

            if (palavraEncontrada == null)
                return NotFound();

            if (palavraEncontrada != null)
                _banco.Palavras.Remove(palavraEncontrada);

            _banco.SaveChanges();

            return NoContent();
        }

    }
}
