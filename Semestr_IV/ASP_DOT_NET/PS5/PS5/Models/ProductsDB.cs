﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace PS5.Models
{
    public class ProductsDB
    {
        public static List<Product> GetProducts(List<Product> products, IConfiguration configuration)
        {
            products = new List<Product>();

            string ConnectionString = configuration.GetConnectionString("PS5DB");

            SqlConnection con = new SqlConnection(ConnectionString);
            string sql = "SELECT * FROM Products";
            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Product product = new Product();
                product.Id = Int32.Parse(reader["Id"].ToString());
                product.Name = reader["Name"].ToString();
                product.Price = Double.Parse(reader["Price"].ToString());
                product.Description = reader["Description"].ToString();
                products.Add(product);
            }
            reader.Close(); con.Close();
            return products;
        }
        public static void AddProduct(Product product, IConfiguration configuration)
        {
            String query = @$"INSERT INTO dbo.Products (Name, Price, Description) VALUES (@Name, @Price, @Description);";
            string connectionString = configuration.GetConnectionString("PS5DB");

            using(SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = product.Name;
                cmd.Parameters.Add("@Price", SqlDbType.Decimal, 18).Value = product.Price;
                cmd.Parameters.Add("@Description", SqlDbType.VarChar, 500).Value = product.Description;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static void RemoveProduct(int id, IConfiguration configuration)
        {
            String query = @$"DELETE FROM dbo.Products WHERE Id = @id;";
            string connectionString = configuration.GetConnectionString("PS5DB");

            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static Product GetProduct(int id, IConfiguration configuration)
        {
            string ConnectionString = configuration.GetConnectionString("PS5DB");

            SqlConnection con = new SqlConnection(ConnectionString);
            string sql = "SELECT * FROM Products WHERE Id = @id";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Product product = new Product();
            while (reader.Read())
            {
                product.Id = Int32.Parse(reader["Id"].ToString());
                product.Name = reader["Name"].ToString();
                product.Price = Double.Parse(reader["Price"].ToString());
                product.Description = reader["Description"].ToString();
            }
            reader.Close(); con.Close();
            return product;
        }
    }
}
