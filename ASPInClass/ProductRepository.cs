using System;
using System.Collections.Generic;
using ASPInClass.Models;
using MySql.Data.MySqlClient;

namespace ASPInClass
{
    public class ProductRepository
    {
        private static string connectionString = System.IO.File.ReadAllText("ConnectionString.txt");

        public List<Product> GetAllProducts()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Products;";

            using (conn)
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                List<Product> allProducts = new List<Product>();

                while(reader.Read() == true)
                {
                    var currentProduct = new Product();
                    currentProduct.ID = reader.GetInt32("ProductID");
                    currentProduct.Name = reader.GetString("Name");
                    currentProduct.Price = reader.GetDecimal("Price");

                    allProducts.Add(currentProduct);
                }

                return allProducts;
            }
        }

        public Product GetProduct(int id)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Products WHERE ProductID = @id;";
            cmd.Parameters.AddWithValue("id", id);

            using (conn)
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                var product = new Product();

                while (reader.Read() == true)
                {
                    product.ID = reader.GetInt32("ProductID");
                    product.Name = reader.GetString("Name");
                    product.Price = reader.GetDecimal("Price");
                    product.CategoryID = reader.GetInt32("CategoryID");
                    product.OnSale = reader.GetInt32("OnSale");

                    if (reader.IsDBNull(reader.GetOrdinal("StockLevel")))
                    {
                        product.StockLevel = null;
                    }
                    else
                    {
                        product.StockLevel = reader.GetString("StockLevel");
                    }

                }

                return product;
            }
        }

        public void UpdateProduct(Product productToUpdate)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText
                = "UPDATE products " +
                  "SET Name = @name, " +
                  "Price = @price " +
                  "WHERE ProductID = @id";

            cmd.Parameters.AddWithValue("name", productToUpdate.Name);
            cmd.Parameters.AddWithValue("price", productToUpdate.Price);
            cmd.Parameters.AddWithValue("id", productToUpdate.ID);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertProduct(Product productToInsert)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO products (NAME, PRICE, CATEGORYID) VALUES (@name, @price, @categoryID);";

            cmd.Parameters.AddWithValue("name", productToInsert.Name);
            cmd.Parameters.AddWithValue("price", productToInsert.Price);
            cmd.Parameters.AddWithValue("categoryID", productToInsert.CategoryID);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Product AssignCategories()
        {
            var catRepo = new CategoryRepository();

            var catList = catRepo.GetCategories();

            Product product = new Product();
            product.Categories = catList;

            return product;
        }

        public void DeleteProduct(int id)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM products WHERE ProductID = @id;";
            cmd.Parameters.AddWithValue("id", id);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProductFromSales(int productID)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM sales WHERE ProductID = @productID;";
            cmd.Parameters.AddWithValue("productID", productID);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProductFromReviews(int productID)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM reviews WHERE ProductID = @productID;";
            cmd.Parameters.AddWithValue("productID", productID);

            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProductFromAllTables(int productID)
        {
            DeleteProductFromSales(productID);
            DeleteProductFromReviews(productID);
            DeleteProduct(productID);
        }
    }
}
