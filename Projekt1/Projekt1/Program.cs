
using HtmlAgilityPack;

namespace Projekt1
{
    class Produkt
    {
        public string Nazwa { get; set; }
        public double Cena { get; set; }
        public string Obrazek { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
           List<string> linkiDoProduktow = PobierzLinkiDoProduktow("https://www.ceneo.pl/Komputery");

            Console.WriteLine("Znaleziono: " + linkiDoProduktow.Count + "produktow");
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
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes();//uzupełnić ścieżke
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