using MimicAPI.Helpers;
using MimicAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repositories.Contracts
{
    public interface IPalavraRepository
    {
        PaginationList<Palavra> BuscarTodas(PalavraUrlQuery query);
        Palavra Buscar(int id);
        void Cadastrar(Palavra palavra);
        void Atualizar(Palavra palavra);
        void Deletar(int id);
    }
}
