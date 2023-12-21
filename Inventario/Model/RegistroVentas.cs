using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Model
{
    internal class RegistroVentas
    {
        public int IdRegistro { get; set; }
        public string categoriaProducto { get; set; }
        public string producto { get; set; }
        public int cantidadProducto { get; set; }
        public decimal precio { get; set; }
        public DateTime fechaRegistro { get; set; }

    }
}
