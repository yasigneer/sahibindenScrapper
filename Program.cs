using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace sahibinden
{
    class Program
    {   
        public static List<Product> products = new List<Product>();
        //created product list globally at program class to make reachable from other methods
        static void Main(string[] args)
        {
            string url = "https://www.sahibinden.com";
            HtmlWeb web = new HtmlWeb();
            //used HAP package to get source code
            HtmlDocument document = web.Load(url);
            TextWriter writer = new StreamWriter("D:\\products.txt");
            var ParentNode = document.DocumentNode.SelectNodes("/html/body/div[5]/div[3]/div/div[3]/div[3]/ul/li/a");
            int mean = 0;
            //reached products links from uibox-showcase sahibinden homepage with full xpath
            foreach (var node in ParentNode)
            {
                GenerateProduct(url + node.GetAttributeValue("href", "").ToString());
                //used to create products
            }
            foreach(var product in products)
            {
                mean += Int32.Parse(String.Join("", product.Price.Split(".")));
                //split price from dots and join after joinin turn into integer and sum
                Console.WriteLine(product.Title + "         " + product.Price);
                writer.WriteLine(product.Title + "         " + product.Price);
            }
            mean = mean / products.Count;
            // take average of total price
            Console.WriteLine("Ortalama Fiyat: " + mean);
        }
        public static void GenerateProduct(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument productDocument = web.Load(url);
            // reached product pages
            try
                // in case of misleading pages or ads, prevent to exception
            {
                string title = productDocument.DocumentNode.SelectNodes("//div[contains(@class, 'classifiedDetailTitle')]/h1").FirstOrDefault().InnerText;
                string price = productDocument.DocumentNode.SelectNodes("//div[contains(@class, 'classifiedInfo ')]/h3").FirstOrDefault().InnerText.Trim().Split(" ")[0];
                // reached title and price info with xpath filters and string functions
                Product product = new Product()
                {
                    URL = url,
                    Price = price,
                    Title = title,
                };
                // created product object and added to list
                products.Add(product);
            }catch{
                return;
                // in case of catching exceptions do not do anything to maintain program flow
            }
        }
    }

    class Product
    {   
        // product object
        public string URL { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
    }
}
