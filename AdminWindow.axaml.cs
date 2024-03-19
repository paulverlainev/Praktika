using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Media;
using Avalonia.Platform;
using MySql.Data.MySqlClient;

namespace autoservice;

public partial class AdminWindow : Window
{
    private string connString = "server=localhost;database=autoservice;port=3307;User Id=root;password=";
    private List<Service> services;
    private List<Discount> discounts;
    private MySqlConnection connection;
    public AdminWindow()
    {
        InitializeComponent();
        ShowTable(full);
        FillDiscount();
    }
    string full = "select id, name, extended, price, discount, image from service;";
    
    public void ShowTable(string sql)
    {
        services = new List<Service>();
        connection  = new MySqlConnection(connString);
        connection.Open();
        MySqlCommand command = new MySqlCommand(sql, connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            
            var useService = new Service()
            {
                id = reader.GetInt32("id"),
                name = reader.GetString("name"),
                extended = reader.GetInt32("extended"),
                price = reader.GetInt32("price"),
                discount = reader.GetInt32("discount"),
                discountprice = reader.GetInt32("price") -( reader.GetInt32("price") /100* reader.GetInt32("discount")),
                image = LoadImage("avares://autoservice/Image/"+ reader.GetString("image"))
            };
          
            services.Add(useService);
        }
        connection.Close();
        Works.ItemsSource = services;
        Works.LoadingRow += PaintingRow;
    }
    private void Search(object? sender, TextChangedEventArgs e)
    {
        var nm =services;
        nm = nm.Where(x => x.name.Contains(Search_.Text)).ToList();
        Works.ItemsSource = nm;
    }
    
    private void Backs(object? sender, RoutedEventArgs e)
    {
        MainWindow main = new MainWindow();
        main.Show();
        this.Close();
    }

    private void Resets(object? sender, RoutedEventArgs e)
    {
        ShowTable(full);
        Search_.Text = string.Empty;
    }

    private void Deleter(object? sender, RoutedEventArgs e)
    {
        try
        {
            Service services = Works.SelectedItem as Service;
            if (services ==null)
            {
                return;
            }
            connection = new MySqlConnection(connString);
            connection.Open();
            string sql = "DELETE FROM `service` WHERE id =" +services.id;
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            this.services.Remove(services);
            ShowTable(full);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    private void Adder(object? sender, RoutedEventArgs e)
    {
        Service newService = new Service();
        Add add = new Add(newService, services);
        add.Show(); 
        this.Close();
        
    }

    private void Edit(object? sender, RoutedEventArgs e)
    {
        Service changeService = Works.SelectedItem as Service;
        if (changeService == null)
            return;
        Service newService = new Service();
        Editor editor = new Editor(newService, services);
        editor.Show();
        this.Close();
        
    }
    private void Cmb_Discount(object? sender, SelectionChangedEventArgs e)
    {
        var servicesComboBox = (ComboBox)sender;
        var useServices = servicesComboBox.SelectedItem as string;
        
        int minDiscount = 0;
        int maxDiscount = 0;
            
        if (useServices == "Cкидки")
        {
            Works.ItemsSource = services;
        }
        else if (useServices == "0% - 5%")
        {
            minDiscount = 1;
            maxDiscount = 5;
        }
        else if (useServices == "5% - 15%")
        {
            minDiscount = 5;
            maxDiscount = 15;
        }
        else if (useServices == "15% - 30%")
        {
            minDiscount = 15;
            maxDiscount = 30;
        }

        if (minDiscount != 0 && maxDiscount != 0)
        {
            var filteredDiscount = services
                .Where(x => x.discount >= minDiscount && x.discount <maxDiscount)
                .ToList();
            Works.ItemsSource = filteredDiscount;  
        }
    }
    
    public void FillDiscount()
    {
        services = new List<Service>();
        connection = new MySqlConnection(connString);
        connection.Open();
        MySqlCommand command = new MySqlCommand("select * from `service`", connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var useService = new Service()
            {
                id = reader.GetInt32("id"),
                name = reader.GetString("name"),
                extended = reader.GetInt32("extended"),
                price = reader.GetInt32("price"),
                discount = reader.GetInt32("discount"),
                discountprice = reader.GetInt32("price") -( reader.GetInt32("price") /100* reader.GetInt32("discount")),
                image = LoadImage("avares://autoservice/Image/"+ reader.GetString("image")),
                img = reader.GetString("image")
            };
            if (!services.Any(x => x.discount == useService.discount))
            {
                services.Add(useService);
            }
        }
        connection.Close();
        var discountComboBox = this.Find<ComboBox>("CmbDiscount");
        discountComboBox.ItemsSource =  new List<string>
        {
            "Скидки",
            "0% - 5%",
            "5% - 15%",
            "15% - 30%"
        };
    }

    public Bitmap LoadImage(string Uri)
    {
        return new Bitmap(AssetLoader.Open(new Uri(Uri)));
    }
    private void SortsV(object? sender, RoutedEventArgs e)
    {
        var sortedDiscount = Works.ItemsSource.Cast<Service>().OrderBy(s => s.discountprice).ToList();
        Works.ItemsSource = sortedDiscount;
    }
    private void SortsU(object? sender, RoutedEventArgs e)
    {
        var sortedDiscount = Works.ItemsSource.Cast<Service>().OrderByDescending(s => s.discountprice).ToList();
        Works.ItemsSource = sortedDiscount;
    }
    private void  PaintingRow(object sender, DataGridRowEventArgs e)
    {
        Service service = e.Row.DataContext as Service;
        if (services != null)
        {
            if (1 <= service.discount && service.discount <= 50)
            {
                e.Row.Background = Brushes.LightGreen;
            }
            else
            {
                e.Row.Background = Brushes.Beige;
            }
        }
        
    }
}