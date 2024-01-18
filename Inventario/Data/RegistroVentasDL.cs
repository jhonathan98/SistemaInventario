using Inventario.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventario.Data
{
    internal class RegistroVentasDL
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Proyectos\Inventario\Inventario\InventarioBD.mdf;Integrated Security=True";

        public List<RegistroVentas> GetRegistroVentas()
        {
            var listVentas = new List<RegistroVentas>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "select * from RegistroVentas";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RegistroVentas rgv = new RegistroVentas();
                                rgv.IdRegistro = reader.GetInt32(0);
                                rgv.categoriaProducto = reader.GetString(1);
                                rgv.producto = reader.GetString(2);
                                rgv.cantidadProducto = reader.GetInt32(3);
                                rgv.precio = reader.GetDecimal(4);
                                rgv.fechaRegistro = reader.GetDateTime(5);

                                listVentas.Add(rgv);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al seleccionar los registros de ventas" + ex.Message);
                }
                conn.Close();

            }
            return listVentas;
        }
        public void registrarVenta(RegistroVentas registroVenta)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "insert into RegistroVentas (categoriaProducto,producto,cantidadProducto,precio,fechaRegistro) values " +
                        "(@categoriaProducto,@producto,@cantidadProducto,@precio,@fechaRegistro)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoriaProducto", registroVenta.categoriaProducto);
                        cmd.Parameters.AddWithValue("@producto", registroVenta.producto);
                        cmd.Parameters.AddWithValue("@cantidadProducto", registroVenta.cantidadProducto);
                        cmd.Parameters.AddWithValue("@precio", registroVenta.precio);
                        cmd.Parameters.AddWithValue("@fechaRegistro", registroVenta.fechaRegistro);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar una venta " + ex.Message);
                }
                conn.Close();

            }
        }
    }
}
