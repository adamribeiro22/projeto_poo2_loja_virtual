using FluentValidation;
using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Validators;
using LojaVirtual.Application.Mapper;
using LojaVirtual.Application.Service;
using LojaVirtual.Domain.Interfaces;
using LojaVirtual.Infrastructure.Persistence;
using LojaVirtual.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurando o Entity Framework Core com MySQL, para definir a conexão com o banco
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Nome da conexão definida no appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)) // Detecta automaticamente a versão do MySQL e usa a string de conexão
);

// Injeção de Dependência (DI) dos repositórios é registrada aqui, sempre que chamar "IProdutoRepository" uma interface, você recebera a implementação dela "ProdutoRepository", isso evita acoplamento.
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IVariacaoProdutoRepository, VariacaoProdutoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Injeção de Dependência (DI) dos serviços é registrada aqui
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IVariacaoProdutoService, VariacaoProdutoService>();

// Configurando o AutoMapper, busca todas as classes que herdam de Profile
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Configurando o FluentValidator para validar e informar erros de criação/atualização
builder.Services.AddValidatorsFromAssemblyContaining<ProdutoCreateDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<VariacaoProdutoCreateDTOValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
