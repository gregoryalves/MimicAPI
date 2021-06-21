using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers
{
    public class Paginacao
    {
        protected int NumeroPagina { get; set; }
        protected int RegistrosPorPagina { get; set; }
        protected int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }

        public Paginacao(int? numeroPagina, int? registrosPorPagina, int? totalRegistros)
        {
            NumeroPagina = numeroPagina.Value;
            RegistrosPorPagina = registrosPorPagina.Value;
            TotalRegistros = totalRegistros.Value;
        }           

        public void CalcularTotalDePaginasNecessarias()
        {
            var totalPaginas = Convert.ToDouble(TotalRegistros) / RegistrosPorPagina;
            var totalPaginasArredondado = Math.Ceiling(totalPaginas);
            TotalPaginas = Convert.ToInt32(totalPaginasArredondado);
        }
    }
}
