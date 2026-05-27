using InformatikaiEszkozok_LIB.DATA;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InfotermekDbContext>(); // DbContext regisztrálása

builder.Services.AddCors(options =>  // uj resz kell hogy html js fusson c# bckendel cors => letrehozs ez mskepp hibát dob cors rá
{
    options.AddPolicy("EngedjMindent", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();


app.UseCors("EngedjMindent");  // kellllllll 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
