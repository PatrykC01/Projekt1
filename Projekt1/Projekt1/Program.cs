
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
    }

}