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

public partial class Editor : Window
{
    private List<Service> Serv;
    private Service NewService;
    public Editor(Service newService, List<Service> serv)
    {
        InitializeComponent();
        NewService = newService;
        this.DataContext = newService;
        Serv = serv;
    }
    private MySqlConnection conn;
    string connStr = "server=localhost;database=autoservice;port=3307;User Id=root;password=";
    
    private void Saves(object? sender, RoutedEventArgs e)
    {
        var serv = Serv.FirstOrDefault(x => x.id == NewService.id);
        if (serv != null)
        {
            try
            {
                conn = new MySqlConnection(connStr); 
                conn.Open();
                string upd = "UPDATE `service` SET name = '" + name.Text + "', extended =" + Convert.ToInt32(extended.Text) + ", price =" + Convert.ToInt32(price.Text) + ", discount ="+ Convert.ToInt32(discount.Text) + ", image ='" + img.Text + "' WHERE id =" + Convert.ToInt32(id.Text) + ";";
                MySqlCommand cmd = new MySqlCommand(upd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error" + exception);
            }
        }
    }
    private async void File_S(object sender, RoutedEventArgs e)
    {
        try
        {
            OpenFileDialog fileDialog = new OpenFileDialog(); 
            fileDialog.Filters.Add(new FileDialogFilter() { Name = "Image Files", Extensions = { "jpg", "jpeg", "png", "gif" } }); 
            string[]? fileNames = await fileDialog.ShowAsync(this); 
            if (fileNames != null && fileNames.Length > 0) 
            {
                string imagePath = System.IO.Path.GetFileName(fileNames[0]); 
                img.Text = imagePath;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private void Backs(object? sender, RoutedEventArgs e)
    {
        AdminWindow admin = new AdminWindow();
        admin.Show(); 
        this.Close();
    }
}