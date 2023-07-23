using Microsoft.EntityFrameworkCore;

namespace course_project.Helpers
{
    public static class ServiceCollectionHelper
    {
        public static async Task ApplyMigrationsForDbContext<T>(this IServiceProvider service) where T : DbContext
        {
            using var scope = service.CreateScope();
            var context = scope.ServiceProvider.GetService<T>();
            await DbContextHelper.ApplyMigrations(context);
        }
    }
}
