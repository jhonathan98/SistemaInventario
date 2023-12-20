using Inventario.Data;
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
        public Inventario()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            cbmTipoProducto.DataSource = prdDL.getCategorias().Tables["Productos"];
            cbmTipoProducto.DisplayMember = "Categoria";
            cbmTipoProducto.ValueMember = "Categoria";

            if(cbmTipoProducto.SelectedValue != null)
            {
                Console.WriteLine(cbmTipoProducto.SelectedValue.ToString());
                string categoriaSeleccionada = cbmTipoProducto.SelectedValue.ToString();
                cmbProductos.DataSource = prdDL.getProductosXCategoria(categoriaSeleccionada).Tables["Productos"];
                cmbProductos.DisplayMember = "nombre_producto";
                cmbProductos.ValueMember = "nombre_producto";
            }
            
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
            string categoriaSeleccionada = cbmTipoProducto.SelectedValue.ToString();
            cmbProductos.DataSource = prdDL.getProductosXCategoria(categoriaSeleccionada).Tables["Productos"];
            cmbProductos.DisplayMember = "nombre_producto";
            cmbProductos.ValueMember = "nombre_producto";
        }
    }
}
