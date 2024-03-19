
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace autoservice;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private string connStr = "server=localhost; database=autoservice;port=3307;User Id=root;password=";
    public void Authorization(object? sender, RoutedEventArgs e)
    {
        string login = Login.Text;
        string password = Password.Text;
        bool client = Client(login, password);
        bool employee = Employee(login, password);
        if (client || employee)
        {
            if (client)
            {
                HideAdmin();
            }
            else
            {
                ShowAdmin();
            } 
            
        }
        else
        {
            Console.WriteLine("Неверный логин или пароль");
        }
    }
    
    private bool Employee(string login, string password)
        {
            bool isValid = false;
        
            using (MySqlConnection connection = new MySqlConnection(connStr))
            {
                string query = "SELECT COUNT(1) FROM atorization WHERE login = @login AND password= @password";
        
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);
        
                    connection.Open();
        
                    object result = command.ExecuteScalar();
                    int count = Convert.ToInt32(result);
        
                    if (count == 1)
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }
    private bool Client(string login, string password) //проверка пользователей по БД
    {
        bool isValid = false;
    
        using (MySqlConnection connection = new MySqlConnection(connStr))
        {
            string query = "SELECT COUNT(1) FROM auto WHERE login = @login AND password= @password";
        
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);

    
                connection.Open();
    
                object result = command.ExecuteScalar();
                int count = Convert.ToInt32(result);
    
                if (count == 1)
                {
                    isValid = true;
                }
            }
        }
        return isValid;
    }
    private void HideAdmin()
    {
        ClientWindow client = new ClientWindow();
        client.Show();
        this.Close();
    }

    private void  ShowAdmin()
    {
        AdminWindow admin = new AdminWindow();
        admin.Show();
        this.Close();
    }
    
    public void Exit(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}