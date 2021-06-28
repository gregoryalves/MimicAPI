using Microsoft.EntityFrameworkCore;
using MimicAPI.DataBase;
using MimicAPI.Helpers;
using MimicAPI.Model;
using MimicAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repositories
{
    public class PalavraRepository : IPalavraRepository
    {
        private readonly MimicContext _banco;

        public PalavraRepository(MimicContext banco)
        {
            _banco = banco;
        }

        public PaginationList<Palavra> BuscarTodas(PalavraUrlQuery query)
        {
            var lista = new PaginationList<Palavra>();
            var palavras = _banco.Palavras.AsNoTracking().AsQueryable();

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
                lista.Paginacao = paginacao;
            }
            lista.AddRange(palavras.ToList());

            return lista;
        }

        public Palavra Buscar(int id)
        {
            return _banco.Palavras.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public void Cadastrar(Palavra palavra)
        {
            palavra.DataCriacao = DateTime.Now;

            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
        }

        public void Atualizar(Palavra palavra)
        {
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
        }

        public void Deletar(int id)
        {
            var palavraEncontrada = Buscar(id);

            if (palavraEncontrada != null)
                _banco.Palavras.Remove(palavraEncontrada);

            _banco.SaveChanges();
        }
    }
}
