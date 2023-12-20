using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Model
{
    public partial class Productos
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public string categoria { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
    }
}
