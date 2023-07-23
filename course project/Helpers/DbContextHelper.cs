using Microsoft.EntityFrameworkCore;

namespace course_project.Helpers
{
    public class DbContextHelper
    {
        public static async Task ApplyMigrations(DbContext context)
        {
            await context.Database.MigrateAsync();
        }
    }
}
