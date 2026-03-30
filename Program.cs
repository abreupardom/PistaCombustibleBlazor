using PistaCombustible.Components;
using PistaCombustible.Data;
using PistaCombustible.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Agregar servicios de DevExpress Blazor
builder.Services.AddDevExpressBlazor();

// Registrar la clase de conexión como servicio
builder.Services.AddScoped<ConexionSQL>(provider => 
    new ConexionSQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar la clase de servicio
builder.Services.AddScoped<EmpleadoService>();
builder.Services.AddScoped<VehiculoService>();
builder.Services.AddScoped<TanqueService>();
builder.Services.AddScoped<AsignacionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
