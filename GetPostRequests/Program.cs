using GetPostRequests.EntityFramework;
using GetPostRequests.Models;
using GetPostRequests.Servise;
using System.Net.Http.Headers;
using System.Text.Json;

var videocards = new List<Videocard>();
GetRequest requestToBodyHtml = new GetRequest();
videocards = requestToBodyHtml.GetVideocardsFromRozetka();
using (ApplicationDb db = new ApplicationDb())
{
    AddToDbProduct addToDbProduct = new AddToDbProduct();
    addToDbProduct.AddToDb(db, videocards);
}
Console.ReadLine();


