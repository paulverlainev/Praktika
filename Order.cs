using System.Runtime.InteropServices.JavaScript;

namespace autoservice;

public class Order
{
    public int id { get; set; }
    public int service { get; set; }
    public string number { get; set; }
    public string comment { get; set; }
}