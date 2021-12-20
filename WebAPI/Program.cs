using Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var auctionController = new AuctionController();
app.MapPost("/start", auctionController.CreateAuction);
app.MapPost("/join", auctionController.Join);

app.Run("http://localhost:5212");