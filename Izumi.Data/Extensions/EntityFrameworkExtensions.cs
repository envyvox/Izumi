using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Data.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static async Task<T> CreateEntity<T>(this AppDbContext db, T entity)
        {
            var created = db.Add(entity);

            created.State = EntityState.Added;

            await db.SaveChangesAsync();

            created.State = EntityState.Detached;

            return (T) created.Entity;
        }

        public static async Task<T> CreateEntityAsync<T>(this AppDbContext db, T entity)
        {
            var created = await db.AddAsync(entity);

            created.State = EntityState.Added;

            await db.SaveChangesAsync();

            created.State = EntityState.Detached;

            return (T) created.Entity;
        }
    }
}
