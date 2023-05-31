using Microsoft.AspNetCore.Builder;

namespace Customer.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseInferastructure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwagger();

            app.UseRouting();
            //app.UseHttpsRedirection(); // for production only

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
