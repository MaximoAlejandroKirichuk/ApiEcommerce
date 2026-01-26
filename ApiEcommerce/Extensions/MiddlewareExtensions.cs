namespace ApiEcommerce.Extensions;

public static class MiddlewareExtensions
{
    public static void UseApplicationMiddleware(this WebApplication app)
    {
    // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("AllowSpecificOrigin");
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
    }
}