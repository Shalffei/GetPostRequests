using GetPostRequests.Models;
using RestSharp;
using System.Text.Json;
using System.Net;
using HtmlAgilityPack;
using System.Web;
using System.Xml.Linq;
using GetPostRequests.EntityFramework;
using System.Linq;

namespace GetPostRequests.Servise
{
    public class GetRequest
    {
        //get Requests*
        //List<Videocard> videocards = myDeserializedClass.Data.Ids.Select(x => new Videocard { Id = x }).ToList();
        //RestClientOptions otioforGetingPrise = new RestClientOptions();

        //for (int i = 2; i <= myDeserializedClass.Data.TotalPages; i++)
        //{

        //    request.AddParameter("page", Convert.ToString(i));
        //    response = client.Execute(request);
        //    myDeserializedClass = JsonSerializer.Deserialize<RootIds>(response.Content);
        //    foreach (var item in myDeserializedClass.Data.Ids)
        //    {
        //        Videocard videocard = new Videocard() { Id = item };
        //        videocards.Add(videocard);
        //    }

        //}


        // proxy "26414:0I45pYUJ@195.123.255.56:2831",
        //       "26414:0I45pYUJ@195.123.193.127:2831",
        //       "26414:0I45pYUJ@195.123.194.209:2831",
        //       "26414:0I45pYUJ@195.123.252.241:2831", на розетке пока не робе!
        public List<Videocard> GetVideocardsFromRozetka() //getting total pages 
        {
            var proxy = new WebProxy("195.123.255.56:2831", false);
            proxy.Credentials = new NetworkCredential("26414", "0I45pYUJ");
            var options = new RestClientOptions();
            options.Proxy = proxy;
            options.BaseUrl = new Uri("https://xl-catalog-api.rozetka.com.ua");
            var client = new RestClient(options);
            var request = new RestRequest("/v4/goods/get", Method.Get);
            request.AddParameter("category_id", "80087");
            var response = client.Execute(request);
            //pase get request here*
            List<Videocard> result = new List<Videocard>();
            return result = GetPriseAndDiscriptions(3, proxy);
        }
        private List<Videocard> GetPriseAndDiscriptions (int totalPages, WebProxy proxy) // Html body requests
        {
            RestClientOptions options = new RestClientOptions();
            options.BaseUrl = new Uri("https://hard.rozetka.com.ua");
            options.Proxy = proxy;
            var client = new RestClient(options);
            var request = new RestRequest("/ua/videocards/c80087/", Method.Get);
            var response = client.Execute(request);
            HashSet<int?> idsInDb = new HashSet<int?>();
            using (ApplicationDb db = new ApplicationDb())
            {
                idsInDb = db.Products.Select(x => x.IdRozetka).ToHashSet();
            }
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(response.Content);
            var obj = document.DocumentNode.SelectNodes("//div[@class = 'goods-tile__inner']");
            List<Videocard> videocards = new List<Videocard>();
            foreach (var item in obj)
            {
                foreach (var Id in idsInDb)
                {
                    if (item.Attributes.Where(x => x.Name == "data-goods-id").Select(x => Convert.ToInt32(x.Value)).FirstOrDefault() == Id)
                        continue;

                    else
                    {
                        Videocard videocard = new Videocard();
                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(item.InnerHtml);
                        var priceHtml = htmlDocument.DocumentNode.SelectSingleNode("//span[@class = 'goods-tile__price-value']").InnerHtml.ToString();
                        var decodePrice = HttpUtility.HtmlDecode(priceHtml).Replace(Convert.ToChar(160).ToString(), "").Trim();
                        videocard.Price = Convert.ToDecimal(decodePrice);
                        var refHtml = htmlDocument.DocumentNode.SelectSingleNode("//a[@class='goods-tile__heading ng-star-inserted']");
                        videocard.ReferenceToCharacteristics = refHtml.Attributes.Where(x => x.Name == "href").Select(x => x.Value).FirstOrDefault();
                        videocard.Name = refHtml.Attributes.Where(x => x.Name == "title").Select(x => x.Value).FirstOrDefault();
                        var idHtml = item.Attributes.Where(x => x.Name == "data-goods-id").Select(x => x.Value).FirstOrDefault();
                        videocard.Id = Convert.ToInt32(idHtml);
                        videocards.Add(videocard);
                    }
                }
            }
            for (int i = 2; i <= totalPages; i++)
            {
                request = new RestRequest("/ua/videocards/c80087/page=" + i + "/");
                response = client.Execute(request);
                document.LoadHtml(response.Content);
                obj = document.DocumentNode.SelectNodes("//div[@class = 'goods-tile__inner']");
                foreach (var item in obj)
                {
                    foreach (var Id in idsInDb)
                    {
                        if (item.Attributes.Where(x => x.Name == "data-goods-id").Select(x => Convert.ToInt32(x.Value)).FirstOrDefault() == Id)
                            continue;

                        else
                        {
                            Videocard videocard = new Videocard();
                            HtmlDocument htmlDocument = new HtmlDocument();
                            htmlDocument.LoadHtml(item.InnerHtml);
                            var priceHtml = htmlDocument.DocumentNode.SelectSingleNode("//span[@class = 'goods-tile__price-value']").InnerHtml.ToString();
                            var decodePrice = HttpUtility.HtmlDecode(priceHtml).Replace(Convert.ToChar(160).ToString(), "").Trim();
                            videocard.Price = Convert.ToDecimal(decodePrice);
                            var refHtml = htmlDocument.DocumentNode.SelectSingleNode("//a[@class='goods-tile__heading ng-star-inserted']");
                            videocard.ReferenceToCharacteristics = refHtml.Attributes.Where(x => x.Name == "href").Select(x => x.Value).FirstOrDefault();
                            videocard.Name = refHtml.Attributes.Where(x => x.Name == "title").Select(x => x.Value).FirstOrDefault();
                            var idHtml = item.Attributes.Where(x => x.Name == "data-goods-id").Select(x => x.Value).FirstOrDefault();
                            videocard.Id = Convert.ToInt32(idHtml);
                            videocards.Add(videocard);
                        }
                    }
                }
            }
            List <Videocard> result = GetCharacteristics(videocards, proxy);
            return result;
            
        }
        private List<Videocard> GetCharacteristics (List<Videocard> videocards, WebProxy proxy)
        {
            foreach (var item in videocards)
            {
                item.CharacteristicsList = new List<Characteristics>();
                RestClientOptions options = new RestClientOptions();
                options.Proxy = proxy;
                var client = new RestClient(options);
                var request = new RestRequest(item.ReferenceToCharacteristics + "characteristics/");
                request.Timeout = TimeSpan.FromSeconds(10).Milliseconds;
                var response = client.Execute(request);
                if (response.IsSuccessful != true)
                {
                    while (response.IsSuccessful == false)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(30).Milliseconds);
                        response = client.Execute(request);
                    }
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(response.Content);
                var lable = document.DocumentNode.SelectNodes("//div[@class = 'characteristics-full__item ng-star-inserted']");
                foreach (var a in lable)
                {
                    Characteristics characteristics = new Characteristics(); 
                    var fullCharacteristicsOfOneElement = new List<Characteristics>();
                    characteristics.CharacteristicsName = a.ChildNodes.Where(x => x.Name == "dt").FirstOrDefault().FirstChild.InnerHtml.ToString();
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(a.InnerHtml);
                    var li = htmlDocument.DocumentNode.SelectNodes("//li[@class = 'ng-star-inserted']");
                    foreach(var itemLi in li)
                    {
                        foreach(var characteristic in itemLi.ChildNodes)
                        {
                            if (characteristic.Name == "a")
                                characteristics.CharacteristicsDescription = characteristic.InnerText;
                            // присвоить значение если там не спан если имя А
                            else if (characteristic.Name == "span")
                                foreach(var itemValue in characteristic.ChildNodes)
                                {
                                    if (characteristic.ChildNodes.Count <= 1)
                                    {
                                        characteristics.CharacteristicsDescription = itemValue.InnerText;
                                    }
                                    else
                                    {
                                        if (characteristics.CharacteristicsDescription != null)
                                        {
                                            if(itemValue.InnerText != "")
                                            {
                                                characteristics.CharacteristicsDescription = (characteristics.CharacteristicsDescription + "\n");
                                                characteristics.CharacteristicsDescription = (characteristics.CharacteristicsDescription + itemValue.InnerText);
                                            }
                                            else
                                            {
                                                continue;
                                            }

                                        }
                                        else
                                        {
                                            if (itemValue.InnerText != "")
                                            {
                                                characteristics.CharacteristicsDescription = itemValue.InnerText;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                }
                        }
                    }
                    item.CharacteristicsList.Add(characteristics);
                }
            }
            return videocards;
        }
    }
}
