
using HtmlAgilityPack;

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

            Console.WriteLine("Znaleziono: " + linkiDoProduktow.Count + "produktow");
            List<Produkt> produkty = PobierzSzczegolyProdutkow(linkiDoProduktow);
        }

        static List<Produkt> PobierzSzczegolyProdutkow(List<string> urls)
        {
            List<Produkt> produkty = new List<Produkt>();
            foreach (string url in urls)
            {
                HtmlDocument document = PobierzDokument(url);
                var titleXPath = "";
                var priceXPath = "";
                var imgUrlXPath = "";
                Produkt produkt= new Produkt();
                produkt.Nazwa = document.DocumentNode.SelectSingleNode(titleXPath).InnerText;
                produkt.Obrazek = document.DocumentNode.SelectSingleNode(imgUrlXPath).InnerText;
                produkt.Cena = float.Parse(document.DocumentNode.SelectSingleNode(priceXPath).InnerText);
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
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//div[@class='cat-prod-row js_category-list-item js_clickHashData js_man-track-event']");//uzupełnić ścieżke
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