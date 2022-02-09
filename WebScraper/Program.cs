using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Text;

namespace WebScraper
{
    using System;

    class Program
    {
        static async Task Main(string[] args)
        {
            //СКРАПЕР РЕЦЕПТИ
            //Console.OutputEncoding = Encoding.UTF8;
            //var config = Configuration.Default.WithDefaultLoader();

            ////Create a new context for evaluating webpages with the given config
            //var context = BrowsingContext.New(config);

            //var document = await context.OpenAsync("https://receptite.com/%D1%80%D0%B5%D1%86%D0%B5%D0%BF%D1%82%D0%B0/%D1%82%D0%B8%D0%BA%D0%B2%D0%B5%D0%BD%D0%B0-%D0%BA%D1%80%D0%B5%D0%BC-%D1%81%D1%83%D0%BF%D0%B0");

            ////var elements = document.QuerySelectorAll(".recepta_produkti > ul > li");
            //var elements = document.QuerySelectorAll(".recepta_prigotviane");

            //foreach (var element in elements)
            //{
            //    Console.WriteLine(element.TextContent);
            //}


            // СКРАПЕР КОЛИ
            Console.OutputEncoding = Encoding.UTF8;
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);

            //Get first 20 cars
            for (int page = 1; page <= 10; page++)
            {
                string pageUrl = $"https://www.mobile.bg/pcgi/mobile.cgi?act=3&slink=na2rzi&f1={page}";
                IDocument carPageDocument = await context.OpenAsync(pageUrl);

                List<string> carsUrls = carPageDocument
                    .QuerySelectorAll(".valgtop > a")
                    .Cast<IHtmlAnchorElement>()
                    .Where(x => x.Href != "javascript:;" && x.Href.Contains("adv"))
                    .Select(x => x.Href)
                    .ToList();

                foreach (var carUrl in carsUrls)
                {
                    //Get car details
                    IDocument carDocument = await context.OpenAsync(carUrl);

                    IElement makeModelElement = carDocument.QuerySelector("h1");
                    Console.WriteLine($"Марка/Модел: {makeModelElement.TextContent}");

                    IElement carPriceElement = carDocument.QuerySelector("#details_price");
                    Console.WriteLine($"Цена: {carPriceElement?.TextContent}");

                    IHtmlCollection<IElement> carInfoElements = carDocument.QuerySelectorAll(".dilarData > li");

                    for (int i = 0; i < carInfoElements.Length; i += 2)
                    {
                        string outputInfo = $"{carInfoElements[i].TextContent}: {carInfoElements[i + 1].TextContent}";
                        Console.WriteLine(outputInfo);
                    }

                    Console.WriteLine(new string('-', 60));
                }
            }

        }
    }
}
