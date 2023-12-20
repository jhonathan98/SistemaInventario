using Inventario.Data;
using Inventario.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventario
{
    public partial class Producto : Form
    {
        private ProductoDL proDL = new ProductoDL();
        public Producto()
        {
            InitializeComponent();            
            
        }

        private void Producto_Load(object sender, EventArgs e)
        {
            actualizarGrid();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void actualizarGrid()
        {
            var datosIniciales = proDL.GetProductos();
            AgregarDatosAlDataGridView(datosIniciales);
        }

        private void btnGuardarProducto_Click(object sender, EventArgs e)
        {
            Productos producto = new Productos();
            
            producto.Id = proDL.getLastIdProducto() + 1;
            producto.NombreProducto = txtNombreProducto.Text;
            producto.categoria = txtCategoria.Text;
            producto.cantidad = Convert.ToInt32(txtCantidad.Text);
            producto.precio = Convert.ToDecimal(txtPrecio.Text);          
            
            proDL.guardarProducto(producto);
            limpiar();
            actualizarGrid();
        }

        private void AgregarDatosAlDataGridView(object datos)
        {
            // Limpia las filas existentes antes de agregar nuevas
            GridViewProductos.Rows.Clear();

            // Verifica que los datos no sean nulos
            if (datos != null)
            {
                // Itera sobre los datos y agrega filas al DataGridView
                foreach (var dato in (System.Collections.IEnumerable)datos)
                {
                    Productos p = dato as Productos;
                    GridViewProductos.Rows.Add(p.Id,p.categoria, p.NombreProducto, p.cantidad, p.precio /* ... */);
                }
            }
        }        

        private void GridViewProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            txtId.Text = GridViewProductos.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if(txtId.Text != "")
            {
                DialogResult elimina = MessageBox.Show("Seguro que desea eliminar el producto con el id:" + txtId.Text, "Salir", MessageBoxButtons.YesNo);
                if(elimina == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(txtId.Text);
                    proDL.eliminarProducto(id);
                    limpiar();
                    actualizarGrid(); 
                }
            }
        }

        public void limpiar()
        {
            txtId.Text = "";
            txtCategoria.Text = "";
            txtNombreProducto.Text = "";
            txtCantidad.Text = "";
            txtPrecio.Text = "";
        }
    }
}
