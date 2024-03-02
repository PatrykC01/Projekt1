
using HtmlAgilityPack;
using System.Globalization;

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
           List<string> linkiDoProduktow = PobierzLinkiDoProduktow("https://www.ceneo.pl/Komputery");

            Console.WriteLine("Znaleziono: " + linkiDoProduktow.Count + " produktow");
            List<Produkt> produkty = PobierzSzczegolyProdutkow(linkiDoProduktow);
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

        static List<Produkt> PobierzSzczegolyProdutkow(List<string> urls)
        {
            List<Produkt> produkty = new List<Produkt>();
            foreach (string url in urls)
            {
                HtmlDocument document = PobierzDokument(url);
                var titleXPath = "//*[@id=\"body\"]/div[2]/div/div/article/div/div[1]/div[2]/div[1]/div[1]/h1";
                var zlpriceXPath = "//*[@id=\"body\"]/div[2]/div/div/article/div/div[2]/div/div/div[1]/span/span/span[1]";
                var grpriceXPath = "//*[@id=\"body\"]/div[2]/div/div/article/div/div[2]/div/div/div[1]/span/span/span[2]";
                var imgUrlXPath = "//*[@id=\"product-carousel\"]/div/div[1]/div[1]/a/img";

                Produkt produkt= new Produkt();
                produkt.Nazwa = document.DocumentNode.SelectSingleNode(titleXPath).InnerText;
                /*var ObrazekLink = document.DocumentNode.SelectSingleNode(imgUrlXPath);

                string href = ObrazekLink.Attributes[name: "href"].Value;*/

                /*produkt.Obrazek = href; */
                var fullPrice = float.Parse ((document.DocumentNode.SelectSingleNode(zlpriceXPath).InnerText) + (document.DocumentNode.SelectSingleNode(grpriceXPath).InnerText));
                produkt.Cena = fullPrice;
                produkty.Add(produkt);
            }

            return produkty;
        }
        static HtmlDocument PobierzDokument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }

        static List<string> PobierzLinkiDoProduktow(string url)
        {
            List<string> linkiDoProduktow = new List<string>();
            HtmlDocument doc = PobierzDokument(url);
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//div[@class='category-list-body js_category-list-body js_search-results js_products-list-main js_async-container']//div[@class='cat-prod-row__body ']/div[@class='cat-prod-row__foto']/a");//uzupełnić ścieżke
            Uri baseUri = new Uri(url);

            foreach (HtmlNode link in linkNodes)
            {
                string href = link.Attributes[name: "href"].Value;
                linkiDoProduktow.Add(new Uri(baseUri, href).AbsoluteUri);
            }

            return linkiDoProduktow;
        }
    }

}