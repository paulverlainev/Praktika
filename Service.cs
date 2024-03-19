namespace autoservice;

using Avalonia.Media.Imaging;

public class Service
{
    public int id { get; set; }
    public string name { get; set; }
    public int extended { get; set; }
    public int price { get; set; }
    public int discountprice { get; set; }
    public int discount { get; set; }
    public string img { get; set; }
    public Bitmap? image { get; set; }
}