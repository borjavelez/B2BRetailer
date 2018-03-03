using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class WarehouseProgram
{
    static void Main(string[] args)
    {
        List<Product> dkProducts = addProducts(4, 1, 5, 3);
        List<Product> frProducts = addProducts(1, 2, 1, 3);
        List<Product> esProducts = addProducts(1, 8, 0, 7);
        List<Product> iTProducts = addProducts(3, 0, 8, 1);
        List<Product> dEProducts = addProducts(4, 8, 0, 4);

        Task.Factory.StartNew(() => new Warehouse.Warehouse(4, "IT", esProducts).Start());
        Task.Factory.StartNew(() => new Warehouse.Warehouse(1, "DK", dkProducts).Start());
        Task.Factory.StartNew(() => new Warehouse.Warehouse(2, "FR", frProducts).Start());
        Task.Factory.StartNew(() => new Warehouse.Warehouse(3, "ES", esProducts).Start());
        Task.Factory.StartNew(() => new Warehouse.Warehouse(5, "DE", esProducts).Start());

        Console.ReadLine();
    }
    public static List<Product> addProducts(int item1quantity, int item2quantity, int item3quantity, int item4quantity)
    {
        List<Product> products = new List<Product>();
        Product item1 = new Product { Id = 1, Price = 90d };
        Product item2 = new Product { Id = 2, Price = 50d };
        Product item3 = new Product { Id = 3, Price = 45d };
        Product item4 = new Product { Id = 4, Price = 120d };
        var temp = Enumerable.Repeat(item1, item1quantity);
        products.AddRange(temp);
        temp = Enumerable.Repeat(item2, item2quantity);
        products.AddRange(temp);
        temp = Enumerable.Repeat(item3, item3quantity);
        products.AddRange(temp);
        temp = Enumerable.Repeat(item4, item4quantity);
        products.AddRange(temp);

        return products;
    }
}