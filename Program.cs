var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Geçici bir "veritabanı" (Münzevi usulü bir liste)
var users = new List<User>();

// 1. REGISTER ENDPOINT
app.MapPost("/register", (User newUser) => 
{
    if (users.Any(u => u.Username == newUser.Username))
        return Results.BadRequest("Bu kullanıcı zaten var.");

    users.Add(newUser);
    return Results.Ok($"{newUser.Username} başarıyla kaydedildi.");
});

// 2. LOGIN ENDPOINT
app.MapPost("/login", (User loginAttempt) => 
{
    var user = users.FirstOrDefault(u => u.Username == loginAttempt.Username && u.Password == loginAttempt.Password);

    return user is not null 
        ? Results.Ok($"Hoş geldin {user.Username}!") 
        : Results.Unauthorized();
});

// 3. GET ALL USERNAMES
app.MapGet("/users/names", () => 
{
    // LINQ kullanarak sadece isimleri "seçiyoruz" (Projection)
    var names = users.Select(u => u.Username).ToList();
    
    return Results.Ok(names);
}); 
//tek satırda sşunu da yapabilirdik :
//app.MapGet("/users/names", () => users.Select(u => u.Username));

app.Run();

// Modern C# Özelliği: Record (Tek satırda DTO/Model oluşturma)
record User(string Username, string Password);