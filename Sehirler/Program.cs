using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Sehirler.Extensions;
using Sehirler.Helpers;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Add services to the container.
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureDataRepository();
builder.Services.ConfigureIAutoRepository();
builder.Services.AddAutoMapper(typeof(Program));


/*
 * Bu kod bloğu, bir ASP.NET Core uygulamasında kullanılacak bir JWT (JSON Web Token) için gerekli olan anahtarın 
 * oluşturulmasını sağlar. JWT'ler genellikle kullanıcı kimlik doğrulama ve yetkilendirme amacıyla kullanılır. 
 * Token, sunucu ve istemci arasında güvenli bir şekilde bilgi alışverişi yapmak için kullanılır.
 */
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer=false,
		ValidateAudience = false
	};
});
/*
 * Bu kod bloğu, ASP.NET Core uygulamanızda JSON dönüşlerini yapılandırmak için kullanılır. Özellikle, JSON dönüşleri sırasında döngüsel
 * referansları korumak amacıyla kullanılır. ReferenceHandler.Preserve özelliği, döngüsel referansları koruyarak JSON nesneleri arasındaki 
 * referansları işler.
 * Bu kod, AddControllers() metodu ile Controller'ları ekleyen bir servis koleksiyonu oluşturur ve ardından AddJsonOptions metodu ile JSON dönüşlerini
 * yapılandırır. ReferenceHandler.Preserve özelliği, JSON dönüşleri sırasında döngüsel referansları koruyacak şekilde ayarlanır.

Döngüsel referanslar, bir nesnenin kendisine veya bir başka nesneye referans içermesi durumudur. Bu durum, özellikle karmaşık veri modelleri veya
ilişkisel veri yapıları kullanıldığında ortaya çıkabilir. ReferenceHandler.Preserve özelliği, bu tür durumlarla başa çıkmak için kullanılır ve 
döngüsel referansları doğru bir şekilde işler.

Bu ayarlar, özellikle ASP.NET Core Web API'lerde, istemcilere gönderilen JSON verilerindeki döngüsel referans hatalarını çözmek için kullanışlıdır.
Ancak, bu ayarların performansı etkileyebileceğini ve kullanılmadan önce iyice değerlendirilmesi gerektiğini unutmayın.
 * */


builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
	});


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
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();


app.Run();
