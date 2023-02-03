using ClienteApiFinal.Services;
using ClienteApiFinal.Db;
using ClienteApiFinal.Dtos;
using ClienteApiFinal.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ClienteDb>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("Cliente")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpClient<ViaCEPService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<ClienteDb>();
    await db.Database.EnsureCreatedAsync();
    await db.Database.MigrateAsync();

    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

var cliente = app.MapGroup("/cliente");
var mapper = new ClienteMapper();

cliente.MapGet("/list", async (ClienteDb db) =>
{
    var result = await db.Clientes.Include(c => c.Contatos).Include(c => c.Endereco).AsNoTracking().ToListAsync();
    var resultDto = mapper.ClienteToClienteDto(result);
    return resultDto;
})
.WithName("GetAllCliente")
.WithOpenApi();

cliente.MapGet("/nome", async ([Required, FromQuery] string nome, ClienteDb db) =>
{
    var result = await db.Clientes.Where(w => w.Nome == nome).Include(c => c.Contatos).Include(c => c.Endereco).AsNoTracking().ToListAsync();
    var resultDto = mapper.ClienteToClienteDto(result);
    return resultDto;
})
.WithName("GetFindCliente")
.WithOpenApi();


cliente.MapGet("/{id}", async (int id, ClienteDb db) =>
{
    var result = await db.Clientes.Where(w => w.Id == id).Include(c => c.Contatos).Include(c => c.Endereco).AsNoTracking().FirstOrDefaultAsync();
    if (result is null)
    {
        return Results.NotFound();
    }
    var resultDto = mapper.ClienteToClienteDto(result);
    return Results.Ok(resultDto);
})
.WithName("GetCliente")
.WithOpenApi();

cliente.MapPost("/", async (ClienteDTO clienteDto, ClienteDb db, ViaCEPService viaCEPService) =>
{
    if (!string.IsNullOrEmpty(clienteDto.Endereco?.Cep))
    {
        var enderecoCep = await viaCEPService.GetCep(clienteDto.Endereco.Cep);
        if (enderecoCep is not null)
        {
            clienteDto.Endereco.Cep = enderecoCep.Cep;
            clienteDto.Endereco.Cidade = enderecoCep.Localidade;
            clienteDto.Endereco.Logradouro = enderecoCep.Logradouro;
            clienteDto.Endereco.Complemento = enderecoCep.Complemento;
        }
        if (!clienteDto.Endereco.Cep.Contains("-"))
        {
            clienteDto.Endereco.Cep = clienteDto.Endereco.Cep.Insert(5, "-");
        }
    }
    var cliente = mapper.ClienteDtoToCliente(clienteDto);
    await db.Clientes.AddAsync(cliente);
    await db.SaveChangesAsync();
    return Results.CreatedAtRoute("GetCliente", new { clienteDto.Id });
})
.WithName("CreateCliente")
.WithOpenApi();

cliente.MapPut("/{id}", async (int id, ClienteDTO clienteDto, ClienteDb db, ViaCEPService viaCEPService) =>
{
    var clienteEncontrado = await db.Clientes.FindAsync(id);
    if (clienteEncontrado is null)
    {
        return Results.NotFound();
    }

    if (!string.IsNullOrEmpty(clienteDto.Endereco?.Cep))
    {
        var enderecoCep = await viaCEPService.GetCep(clienteDto.Endereco.Cep);
        if (enderecoCep is not null)
        {
            clienteDto.Endereco.Cep = enderecoCep.Cep;
            clienteDto.Endereco.Cidade = enderecoCep.Localidade;
            clienteDto.Endereco.Logradouro = enderecoCep.Logradouro;
            clienteDto.Endereco.Complemento = enderecoCep.Complemento;
        }
        if (!clienteDto.Endereco.Cep.Contains("-"))
        {
            clienteDto.Endereco.Cep = clienteDto.Endereco.Cep.Insert(5, "-");
        }
    }

    var clienteMapeado = mapper.ClienteDtoToCliente(clienteDto);

    clienteEncontrado.Nome = clienteMapeado.Nome;
    clienteEncontrado.Endereco = clienteMapeado.Endereco;
    clienteEncontrado.Contatos = clienteMapeado.Contatos;

    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("UpdateCliente")
.WithOpenApi();

cliente.MapDelete("/{id}", async (int id, ClienteDb db) =>
{
    var clienteEncontrado = await db.Clientes.Where(w => w.Id == id).Include(c => c.Contatos).Include(c => c.Endereco).AsNoTracking().FirstOrDefaultAsync();

    if (clienteEncontrado is null)
    {
        return Results.NotFound();
    }
    db.Clientes.Remove(clienteEncontrado);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteCliente")
.WithOpenApi();

app.Run();