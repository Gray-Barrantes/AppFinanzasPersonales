using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace TestingConsole
{
    public class Program
    {
        // Cadena de conexión asegurarse de cambiar el nombre del servidor si es necesario
        private static string connectionString = "Server=LAPTOP-Q9E9NVPP;Database=OLTPSystem;Integrated Security=True;";

        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n MENÚ PRINCIPAL ");
                Console.WriteLine("1. Agregar Cliente");
                Console.WriteLine("2. Agregar Producto");
                Console.WriteLine("3. Registrar Transacción");
                Console.WriteLine("4. Ver Transacciones");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");

                string choice = Console.ReadLine();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    switch (choice)
                    {
                        case "1":
                            AddCustomer(connection);
                            break;
                        case "2":
                            AddProduct(connection);
                            break;
                        case "3":
                            CreateTransaction(connection);
                            break;
                        case "4":
                            GetTransactions(connection);
                            break;
                        case "5":
                            Console.WriteLine("Saliendo...");
                            return;
                        default:
                            Console.WriteLine("Opción no válida. Intente de nuevo.");
                            break;
                    }
                }
            }
        }

        private static void AddCustomer(SqlConnection connection)
        {
            Console.Write("Nombre: ");
            string firstName = Console.ReadLine();
            Console.Write("Apellido: ");
            string lastName = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Teléfono: ");
            string phone = Console.ReadLine();

            string query = "INSERT INTO Customers (FirstName, LastName, Email, Phone) VALUES (@FirstName, @LastName, @Email, @Phone)";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Cliente agregado correctamente.");
            }
        }

        private static void AddProduct(SqlConnection connection)
        {
            Console.Write("Nombre del Producto: ");
            string productName = Console.ReadLine();
            Console.Write("Precio: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Cantidad en Stock: ");
            int stockQuantity = Convert.ToInt32(Console.ReadLine());

            string query = "INSERT INTO Products (ProductName, Price, StockQuantity) VALUES (@ProductName, @Price, @StockQuantity)";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Producto agregado correctamente.");
            }
        }

        private static void CreateTransaction(SqlConnection connection)
        {
            Console.Write("ID del Cliente: ");
            int customerId = Convert.ToInt32(Console.ReadLine());
            Console.Write("ID del Producto: ");
            int productId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Cantidad: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            string priceQuery = "SELECT Price FROM Products WHERE ProductID = @ProductID";
            decimal price;
            using (SqlCommand cmd = new SqlCommand(priceQuery, connection))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                price = (decimal)cmd.ExecuteScalar();
            }

            decimal totalAmount = price * quantity;
            string query = "INSERT INTO Transactions (CustomerID, ProductID, Quantity, TotalAmount) VALUES (@CustomerID, @ProductID, @Quantity, @TotalAmount)";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Transacción registrada correctamente.");
            }
        }

        private static void GetTransactions(SqlConnection connection)
        {
            string query = "SELECT T.TransactionID, C.FirstName, C.LastName, P.ProductName, T.Quantity, T.TotalAmount, T.TransactionDate " +
                           "FROM Transactions T " +
                           "JOIN Customers C ON T.CustomerID = C.CustomerID " +
                           "JOIN Products P ON T.ProductID = P.ProductID " +
                           "ORDER BY T.TransactionDate DESC";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n LISTADO DE TRANSACCIONES ");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["TransactionID"]} | Cliente: {reader["FirstName"]} {reader["LastName"]} | " +
                                      $"Producto: {reader["ProductName"]} | Cantidad: {reader["Quantity"]} | " +
                                      $"Total: {reader["TotalAmount"]} | Fecha: {reader["TransactionDate"]}");
                }
            }
        }
    }
}
