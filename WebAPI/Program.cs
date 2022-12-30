using ProjetoCrowdsourcing;

var db = new BaseContext();
var mturkConnector = new MTurkConnector(db);
var question1Manager = new Question1(db);
var validation1Manager = new Validation1(db);
var mturkSync = new MturkSynchronizer(mturkConnector, validation1Manager);

mturkSync.NewAssignmentEvent += MturkSync_NewAssignmentEvent;
mturkSync.NewValidationEvent += MturkSync_NewValidationEvent;

// Verifica novas respostas de hits e novas validações a cada 10 segundos - subrotina
mturkSync.RunAsync(10);

/*var novoHIT = question1Manager.CreateHIT("classic guitar");
Console.WriteLine(MTurkUtils.GetURLFromHIT(novoHIT.HIT.HITTypeId));*/


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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




void MturkSync_NewValidationEvent(ProjetoCrowdsourcing.Models.ValidationHIT localValidationHIT)
{
    Console.WriteLine("New Validation Received!");
}

void MturkSync_NewAssignmentEvent(ProjetoCrowdsourcing.Models.HIT localHIT, Amazon.MTurk.Model.Assignment assignment, Amazon.MTurk.Model.CreateHITResponse validationHIT)
{
    Console.WriteLine("New Assignment Received!");
    Console.WriteLine("------ Validation HIT: " + MTurkUtils.GetURLFromHIT(validationHIT.HIT.HITTypeId));
}