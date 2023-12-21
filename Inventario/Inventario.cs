using Inventario.Data;
using Inventario.Model;
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
    public partial class Inventario : Form
    {
        private ProductoDL prdDL = new ProductoDL();
        private RegistroVentasDL rgvDL = new RegistroVentasDL();
        public Inventario()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            actualizarGridRegistrosVentas();

            cbmTipoProducto.DataSource = prdDL.getCategorias().Tables["Productos"];
            cbmTipoProducto.DisplayMember = "Categoria";
            cbmTipoProducto.ValueMember = "Categoria";

            if(cbmTipoProducto.SelectedValue != null)
            {
                actualizarGridProductos();
            }
            
        }

        private void actualizarGridRegistrosVentas()
        {
            //cargar el grid
            var datosIniciales = rgvDL.GetRegistroVentas();
            AgregarDatosAlDataGridView(datosIniciales);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Producto pd = new Producto();
            pd.ShowDialog(); 
        }

        private void cbmTipoProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizarGridProductos();
        }

        private void actualizarGridProductos()
        {
            string categoriaSeleccionada = cbmTipoProducto.SelectedValue.ToString();
            cmbProductos.DataSource = prdDL.getProductosXCategoria(categoriaSeleccionada).Tables["Productos"];
            cmbProductos.DisplayMember = "nombre_producto";
            cmbProductos.ValueMember = "nombre_producto";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(txtCantidad.Text) > 0) {

                RegistroVentas registroVentas = new RegistroVentas();
                string IdproductoSeleccionado = lblProductoSeleccionado.Text.Split('-')[0].Trim().ToString();
                Productos pd = new Productos();
                
                pd = prdDL.getProductoXid(Convert.ToInt32(IdproductoSeleccionado));

                registroVentas.categoriaProducto = cbmTipoProducto.SelectedValue.ToString();
                registroVentas.producto = cmbProductos.SelectedValue.ToString();
                registroVentas.cantidadProducto = Convert.ToInt32(txtCantidad.Text);
                registroVentas.precio = pd.precio;
                registroVentas.fechaRegistro = DateTime.Now;

                rgvDL.registrarVenta(registroVentas);
                prdDL.actualizarCantidadProductoxId(Convert.ToInt32(IdproductoSeleccionado), Convert.ToInt32(txtCantidad.Text));
                actualizarProductoSeleccionado();
                actualizarGridRegistrosVentas();
            }
            else
            {
                MessageBox.Show("Ingrese todos los datos para poder registrar");
            }
        }

        private void cmbProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizarProductoSeleccionado();
        }

        public void actualizarProductoSeleccionado()
        {
            Productos pd = new Productos();

            pd = prdDL.getProductoXproductoYcategoria(cbmTipoProducto.SelectedValue.ToString(), cmbProductos.SelectedValue.ToString());

            lblProductoSeleccionado.Text = $"{pd.Id} - {pd.categoria} - {pd.NombreProducto} - {pd.cantidad} - {pd.precio}";
        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void AgregarDatosAlDataGridView(object datos)
        {
            // Limpia las filas existentes antes de agregar nuevas
            GridViewRegistroVentas.Rows.Clear();

            // Verifica que los datos no sean nulos
            if (datos != null)
            {
                // Itera sobre los datos y agrega filas al DataGridView
                foreach (var dato in (System.Collections.IEnumerable)datos)
                {
                    RegistroVentas p = dato as RegistroVentas;
                    GridViewRegistroVentas.Rows.Add(p.fechaRegistro, p.categoriaProducto, p.producto, p.cantidadProducto, p.precio, p.cantidadProducto*p.precio );
                }
            }
        }
    }
}
