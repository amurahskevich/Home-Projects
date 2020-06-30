using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;

namespace Parser
{
    class Program
    {
        private const string Path = @"https://www.amalgama-lab.com/songs/s/starset/back_to_the_earth.html";

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1254");

            while (true)
            {
                Console.Clear();
                var path = Console.ReadLine();
                var httpClient = new HttpClient();
                var html = httpClient.GetStringAsync(path).GetAwaiter().GetResult();

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);


                var ss = htmlDocument.DocumentNode.Descendants(0)
                    .Where(x => x.HasClass("string_container"))
                    .Select(p =>
                    {
                        var original = p.Descendants(0).First(y => y.HasClass("original")).InnerText;
                        var translate = p.Descendants(0).First(y => y.HasClass("translate")).InnerText;
                        return new Song
                        {
                            Original = original.Replace("\n", ""),
                            Translate = translate.Replace("\n", ""),
                        };
                    })
                    .ToList();

                foreach (var s in ss)
                {
                    Console.WriteLine($"{s.Original}\t{s.Translate}");
                }

                Console.ReadLine();
            }
        }
    }

    public class Song
    {
        public string Original { get; set; }

        public string Translate { get; set; }
    }
}
