using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class WarehouseProgram
{
    static void Main(string[] args)
    {
        List<Product> dkProducts = addProducts(0, 0, 0, 0);

        List<Product> frProducts = addProducts(1, 7, 1, 3);
        List<Product> esProducts = addProducts(1, 8, 2, 7);

        Task.Factory.StartNew(() => new Warehouse.Warehouse(1, "DK", dkProducts).Start());
        Task.Factory.StartNew(() => new Warehouse.Warehouse(2, "FR", frProducts).Start());
        //Task.Factory.StartNew(() => new Warehouse.Warehouse(3, "ES", esProducts).Start());

        Console.ReadLine();
    }
    public static List<Product> addProducts(int modelSquantity, int model3quantity, int roadsterquantity, int modelXquantity)
    {
        List<Product> products = new List<Product>();
        Product modelS = new Product { Id = 1, Price = 80.000 };
        Product model3 = new Product { Id = 2, Price = 40.000 };
        Product roadster = new Product { Id = 3, Price = 215.000 };
        Product modelX = new Product { Id = 4, Price = 97.000 };
        var temp = Enumerable.Repeat(modelS, modelSquantity);
        products.AddRange(temp);
        temp = Enumerable.Repeat(model3, model3quantity);
        products.AddRange(temp);
        temp = Enumerable.Repeat(roadster, roadsterquantity);
        products.AddRange(temp);
        temp = Enumerable.Repeat(modelX, modelXquantity);
        products.AddRange(temp);

        return products;
    }
}