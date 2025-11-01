using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Validators;
using LojaVirtual.Application.Mapper;
using LojaVirtual.Application.Service;
using LojaVirtual.Domain.Interfaces;
using LojaVirtual.Infrastructure.Persistence;
using LojaVirtual.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Configurando o Entity Framework Core com MySQL, para definir a conex�o com o banco
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Nome da conex�o definida no appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)) // Detecta automaticamente a vers�o do MySQL e usa a string de conex�o
);

// Inje��o de Depend�ncia (DI) dos reposit�rios � registrada aqui, sempre que chamar "IProdutoRepository" uma interface, voc� recebera a implementa��o dela "ProdutoRepository", isso evita acoplamento.
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IVariacaoProdutoRepository, VariacaoProdutoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEstoqueRepository, EstoqueRepository>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Inje��o de Depend�ncia (DI) dos servi�os � registrada aqui
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IVariacaoProdutoService, VariacaoProdutoService>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IVendaService, VendaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configurando o AutoMapper, busca todas as classes que herdam de ProfileRegisterRequestDTOValidator
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Configurando o FluentValidator, ele vai no assembly do "RegisterRequestDTOValidator" e busca todas classes que tbm usam o fluentvalidator
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestDTOValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Configurando a autentica��o JWT para proteger os endpoints da API entre os tipos de usu�rios
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();
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
    
app.UseCors(myAllowSpecificOrigins);

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
