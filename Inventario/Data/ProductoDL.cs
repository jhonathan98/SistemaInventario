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
                                pd.fechaIngreso = reader.GetDateTime(5);
                                pd.fechaActualizacion = reader.GetDateTime(6);

                                listProductos.Add(pd);                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al seleccionar todos los productos"+ex.Message);
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
                    MessageBox.Show("Error al consultar el ultimo producto ingresado" + ex.Message);
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
                    string query = "insert into Productos (id,nombre_producto,cantidad,categoria,precio,fecha_ingreso,fecha_actualizacion) values(@id,@nombre_producto,@cantidad,@categoria,@precio,@fechaIngreso,@fechaActualizacion)";
                    
                    //si no existe el nombre del producto lo agregamos sino no hacemos nada
                    if (!existeNombreProducto(pd.NombreProducto))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", pd.Id);
                            cmd.Parameters.AddWithValue("@nombre_producto", pd.NombreProducto);
                            cmd.Parameters.AddWithValue("@cantidad", pd.cantidad);
                            cmd.Parameters.AddWithValue("@categoria", pd.categoria);
                            cmd.Parameters.AddWithValue("@precio", pd.precio);
                            cmd.Parameters.AddWithValue("@fechaIngreso", pd.fechaIngreso);
                            cmd.Parameters.AddWithValue("@fechaActualizacion", pd.fechaActualizacion);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el producto"+ex.Message);
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
                    string query = "update Productos set nombre_producto = @nombre_producto, cantidad=@cantidad, categoria = @categoria, precio = @precio, fecha_actualizacion = @fechaActualizacion where id = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", pd.Id);
                        cmd.Parameters.AddWithValue("@nombre_producto", pd.NombreProducto);
                        cmd.Parameters.AddWithValue("@cantidad", pd.cantidad);
                        cmd.Parameters.AddWithValue("@categoria", pd.categoria);
                        cmd.Parameters.AddWithValue("@precio", pd.precio);
                        cmd.Parameters.AddWithValue("@fechaActualizacion", pd.fechaActualizacion);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el producto"+ex.Message);
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
                    MessageBox.Show("Error al eliminar el producto con id:" + id + " error: "+ex.Message);
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
                    MessageBox.Show("Error al seleccionar las diferentes categorias de productos " + ex.Message);
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
                    MessageBox.Show("Error al seleccionar los diferentes productos por categoria:" + categoria + " error:" + ex.Message);
                }
                conn.Close();

            }
            return ds;
        }

        public Productos getProductoXproductoYcategoria(string categoria, string producto)
        {
            Productos pd = new Productos();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "select * from Productos where categoria = @categoria and nombre_producto = @producto";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoria", categoria);
                        cmd.Parameters.AddWithValue("@producto", producto);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {                                
                                pd.Id = reader.GetInt32(0);
                                pd.NombreProducto = reader.GetString(1);
                                pd.categoria = reader.GetString(3);
                                pd.cantidad = reader.GetInt32(2);
                                pd.precio = reader.GetDecimal(4);
                                pd.fechaIngreso = reader.GetDateTime(5);
                                pd.fechaActualizacion = reader.GetDateTime(6);
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al seleccionar el producto por catergoria:" + categoria + " y producto:" + producto + " error:"+ex.Message);
                }
                conn.Close();

            }
            return pd;
        }

        public bool existeNombreProducto(string producto)
        {
            int id = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "select * from Productos where nombre_producto = @producto";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {                        
                        cmd.Parameters.AddWithValue("@producto", producto);
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
                    MessageBox.Show("Error al seleccionar un producto por el nombre" + producto + " error:" + ex.Message);
                }
                conn.Close();                

            }
            if (id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Productos getProductoXid(int id)
        {
            Productos pd = new Productos();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "select * from Productos where id = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {                                
                                pd.Id = reader.GetInt32(0);
                                pd.NombreProducto = reader.GetString(1);
                                pd.categoria = reader.GetString(3);
                                pd.cantidad = reader.GetInt32(2);
                                pd.precio = reader.GetDecimal(4);
                                pd.fechaIngreso = reader.GetDateTime(5);
                                pd.fechaActualizacion = reader.GetDateTime(6);
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener el producto por id:" + id + "->" + ex.Message);
                }
                conn.Close();

            }
            return pd;
        }

        public void actualizarCantidadProductoxId(int id,int cantidad)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Productos pd = new Productos();
                    pd = getProductoXid(id);
                    int resultadoCantidad = pd.cantidad - cantidad;

                    conn.Open();
                    string query = "update Productos set cantidad=@cantidad, fecha_actualizacion=@fechaActualizacion where id = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@cantidad", resultadoCantidad);
                        cmd.Parameters.AddWithValue("@fechaActualizacion", DateTime.Now);
                        
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la cantidad del producto con el id:" + id + " error:" + ex.Message);
                }
                conn.Close();

            }
        }

    }
}
