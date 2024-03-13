﻿
using HtmlAgilityPack;
using System.Globalization;
using System.Reflection.Metadata;

namespace Projekt1
{
    class Produkt
    {
        public string Nazwa { get; set; }
        public float Cena { get; set; }
        public string Obrazek { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            List<Produkt> produkty = PobierzSzczegolyProdutkow("https://www.ceneo.pl/Komputery");
            
            Export(produkty);
        }

        static void Export(List<Produkt> produkty)
        {
            using (var writer = new StreamWriter("C:\\Users\\20pat\\OneDrive\\Pulpit\\dane.txt"))
            {
                foreach (var produkt in produkty)
                {
                    writer.WriteLine($"Nazwa: {produkt.Nazwa}, Cena: {produkt.Cena}, Img: {produkt.Obrazek}");
                }
            }
        }

        /*static List<Produkt> PobierzSzczegolyProdutkow(List<string> urls)
        {
            List<Produkt> produkty = new List<Produkt>();
            foreach (string url in urls)
            {
                HtmlDocument document = PobierzDokument(url);
                var titleXPath = "//div[@class='product-top__title']/h1";
                *//*var zlpriceXPath = "//*[@id=\"body\"]/div[2]/div/div/article/div/div[2]/div/div/div[1]/span/span/span[1]";
                var grpriceXPath = "//*[@id=\"body\"]/div[2]/div/div/article/div/div[2]/div/div/div[1]/span/span/span[2]";
                var imgUrlXPath = "//*[@id=\"product-carousel\"]/div/div[1]/div[1]/a/img";*//*

                Produkt produkt= new Produkt();
                produkt.Nazwa = document.DocumentNode.SelectSingleNode(titleXPath).InnerText;
                Console.WriteLine(document.DocumentNode.SelectSingleNode(titleXPath).InnerText);
                *//*var ObrazekLink = document.DocumentNode.SelectSingleNode(imgUrlXPath);

                string href = ObrazekLink.Attributes[name: "href"].Value;*/

                /*produkt.Obrazek = href; */
                /*var fullPrice = float.Parse ((document.DocumentNode.SelectSingleNode(zlpriceXPath).InnerText) + (document.DocumentNode.SelectSingleNode(grpriceXPath).InnerText));
                produkt.Cena = fullPrice;*//*
                produkty.Add(produkt);
            }

            return produkty;
        }*/
        static HtmlDocument PobierzDokument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }

        static string ZastepowanieZnakowSpecjalnych(string input)
        {
            
            var replacements = new List<string>
            { ";", "'", ",", ".", "/", " ", "&", "(", "+", "”"};

            Dictionary<char, char> polskieZnaki = new Dictionary<char, char>
            {
                { 'ą', 'a' },
                { 'ć', 'c' },
                { 'ę', 'e' },
                { 'ł', 'l' },
                { 'ń', 'n' },
                { 'ó', 'o' },
                { 'ś', 's' },
                { 'ź', 'z' },
                { 'ż', 'z' }
            };


            char previousChar = 'a';

            var result = "";
            
            foreach (var c in input)
            {
                
                if (replacements.Contains(c.ToString()))
                {
                    if (previousChar != '-')
                    {
                        result += '-';
                        previousChar = '-';
                    }
                }
                else
                {
                   if(c != ')' && c != '"')
                    {
                        if (polskieZnaki.ContainsKey(c))

                            foreach (var x in polskieZnaki)
                            {
                                if (x.Key == c)
                                    result += x.Value;
                            }


                        else if (c.ToString() != "quot")
                            result += c;
                        
                    }
                    previousChar = c;
                }
            }
            
            return result;
        }

        static List<Produkt> PobierzSzczegolyProdutkow(string url)
        {
            List<Produkt> produkty = new List<Produkt>();
            HtmlDocument doc = PobierzDokument(url);
            HtmlNodeCollection productNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, \"category-list-body\")]//div[contains(@class, \"cat-prod-row js_category-list-item\")]");

            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, \"category-list-body\")]//div[contains(@class, \"cat-prod-row js_category-list-item\")]//a[contains(@class, \"js_clickHash js_seoUrl product-link go-to-product\")]/img");

            Console.WriteLine("PN: " + productNodes.Count + ", IN: " + imgNodes.Count);
            Uri baseUri = new Uri(url);


            Console.WriteLine("Znaleziono: " + productNodes.Count + " produktow");

            for(int i = 0; i < imgNodes.Count; i++)
            {
                string title = productNodes[i].Attributes["data-productname"].Value;
                string datapid = productNodes[i].Attributes["data-pid"].Value;
                string price = productNodes[i].Attributes["data-productminprice"].Value;

                //string imgUrl = imgNodes[i].Attributes["src"].Value;

                /*Console.WriteLine($"https://www.bing.com/images/search?q={title}&form=HDRSC4&first=1");
                HtmlDocument doc2 = PobierzDokument($"https://www.bing.com/images/search?q={title}&form=HDRSC4&first=1");
                HtmlNode productNode = doc2.DocumentNode.SelectSingleNode("//div[contains(@class, \"imgpt\")]//a[contains(@class, \"iusc\")]");*/

                Console.WriteLine(title);
                string cleanName = ZastepowanieZnakowSpecjalnych(title);

                string url2 = "//image.ceneostatic.pl/data/products/" + datapid + "/f-" + cleanName + ".jpg";
                

                Console.WriteLine(price);
                Console.WriteLine(url2);

                /*HtmlDocument doc3 = PobierzDokument(fullUrl);
                HtmlNode productNode2 = doc3.DocumentNode.SelectSingleNode("//div[contains(@class, \"mainContainer\")]//img[contains(@class, \"nofocus\")]");*/

                /*string imgUrl = productNode2.Attributes["src"].Value;*/

                Console.WriteLine("_____________________________________________________");

                Produkt produkt = new Produkt();
                produkt.Nazwa = title;
                /*var ObrazekLink = document.DocumentNode.SelectSingleNode(imgUrlXPath);

                string href = ObrazekLink.Attributes[name: "href"].Value;*/

                /*produkt.Obrazek = href; */
                /*var fullPrice = float.Parse ((document.DocumentNode.SelectSingleNode(zlpriceXPath).InnerText) + (document.DocumentNode.SelectSingleNode(grpriceXPath).InnerText));
                produkt.Cena = fullPrice;*/
                produkty.Add(produkt);
            }

            return produkty;
        }
    }

}