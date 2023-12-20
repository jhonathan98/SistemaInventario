using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventario.Model;
using Newtonsoft;
using Newtonsoft.Json;

namespace Inventario.Data
{
    internal class ProductoDL
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Proyectos\Inventario\Inventario\InventarioBD.mdf;Integrated Security=True";
        //private List<Productos> ProductosList;
       

        public List<Productos> GetProductos()
        {
            var listProductos = new List<Productos>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "select * from Productos";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Productos pd = new Productos();
                                pd.Id = reader.GetInt32(0);
                                pd.NombreProducto = reader.GetString(1);
                                pd.categoria = reader.GetString(3);
                                pd.cantidad = reader.GetInt32(2);
                                pd.precio = reader.GetDecimal(4);

                                listProductos.Add(pd);                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
                conn.Close();

            }
            return listProductos;
        }

        public int getLastIdProducto()
        {
            int id = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "select top(1) id from Productos ORDER BY Id DESC ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = reader.GetInt32(0);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                conn.Close();

            }
            return id;
        }

        public void guardarProducto(Productos pd)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "insert into Productos (id,nombre_producto,cantidad,categoria,precio) values(@id,@nombre_producto,@cantidad,@categoria,@precio)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", pd.Id);
                        cmd.Parameters.AddWithValue("@nombre_producto", pd.NombreProducto);
                        cmd.Parameters.AddWithValue("@cantidad", pd.cantidad);
                        cmd.Parameters.AddWithValue("@categoria", pd.categoria);
                        cmd.Parameters.AddWithValue("@precio", pd.precio);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {

                }
                conn.Close();

            }
        }

        public void actualizarProducto(Productos pd)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "update Productos set nombre_producto = @nombre_producto, cantidad=@cantidad, categoria = @categoria, precio = @precio where id = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", pd.Id);
                        cmd.Parameters.AddWithValue("@nombre_producto", pd.NombreProducto);
                        cmd.Parameters.AddWithValue("@cantidad", pd.cantidad);
                        cmd.Parameters.AddWithValue("@categoria", pd.categoria);
                        cmd.Parameters.AddWithValue("@precio", pd.precio);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {

                }
                conn.Close();

            }
        }

        public void eliminarProducto(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "delete from Productos where Id = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {

                }
                conn.Close();

            }
        }

        public DataSet getCategorias()
        {
            DataSet ds = new DataSet();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "select distinct categoria from Productos";

                    using(SqlDataAdapter adapter  = new SqlDataAdapter(query, conn))
                    {
                        adapter.Fill(ds, "Productos");                        
                        
                    }
                }
                catch (Exception ex)
                {

                }
                conn.Close();

            }
            return ds;
        }

        public DataSet getProductosXCategoria(string categoria)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "select distinct nombre_producto from Productos where categoria = @categoria";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@categoria", categoria);
                        adapter.Fill(ds, "Productos");
                    }
                }
                catch (Exception ex)
                {

                }
                conn.Close();

            }
            return ds;
        }

    }
}
