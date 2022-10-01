using GetPostRequests.EntityFramework;
using GetPostRequests.Models;
using GetPostRequests.Servise;
using System.Text.Json;

var listVideocards = new List<Videocard>();
using (ApplicationDBContextJson db = new ApplicationDBContextJson())
{
    var takeFromJsonDb = db.Videocards.ToList();
    foreach (var item in takeFromJsonDb)
    {
        var deserialize = JsonSerializer.Deserialize<Videocard>(item.FullJsonVideocard);
        listVideocards.Add(deserialize);
    }    
    
}
using (ApplicationDb db = new ApplicationDb())
{
    AddToDbProduct addToDb = new AddToDbProduct();
    addToDb.AddToDb(db, listVideocards);
}

Console.ReadLine();


