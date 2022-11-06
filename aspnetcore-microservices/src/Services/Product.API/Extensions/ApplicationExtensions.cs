namespace Product.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseInferastructure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

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
