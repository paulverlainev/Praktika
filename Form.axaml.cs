using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using System;

namespace autoservice;

public partial class Form : Window
{
    private List<Order> _Order;
    private Order NewOrder;
    public Form()
    {
        InitializeComponent();
    }
    private string connString = "server=localhost;database=autoservice;port=3307;User Id=root;password=";
    private MySqlConnection connection;
    private void Saves(object? sender, RoutedEventArgs e)
    {
            try
            {
                connection = new MySqlConnection(connString);
                connection.Open();
                string add = "INSERT INTO `ord`(service, number, comment) VALUES ('" + service.Text + "','" + number.Text + "', '" + comment.Text + "');";
                MySqlCommand cmd = new MySqlCommand(add, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error" + exception);
            }
    }
    
    private void Backs(object? sender, RoutedEventArgs e)
    { 
        ClientWindow client = new ClientWindow();
        client.Show(); 
        this.Close();
    }
    
}