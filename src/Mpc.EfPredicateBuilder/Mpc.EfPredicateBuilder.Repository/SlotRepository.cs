namespace Mpc.EfPredicateBuilder.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class SlotRepository : ISlotRepository
    {
        private AppDbContext appContext;

        public SlotRepository(AppDbContext appContext)
        {
            this.appContext = appContext;
        }

        public async Task<Slot> AddAsync(Slot entity)
        {
            var newSlot = await appContext.Slots.AddAsync(entity).ConfigureAwait(false);
            return newSlot.Entity;
        }

        public async Task CommitAsync()
        {
            await appContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Slot>> GetAllAsync()
        {
            return await appContext.Slots
                .AsNoTracking()
                .Include(s => s.Products)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Slot> GetAsync(int id)
        {
            return await appContext.Slots.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Slot>> SearchAsync(Filters.SlotFilter slotFilter)
        {
            var query = appContext.Slots
                .AsQueryable()
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(slotFilter.Name))
            {
                query = query.Where(s => s.Name == slotFilter.Name);
            }

            if (slotFilter.BrandId.HasValue)
            {
                query = query.Where(s => s.Products.Any(p => p.BrandId == slotFilter.BrandId.Value));
            }

            if (slotFilter.SeasonId.HasValue)
            {
                query = query.Where(s => s.Products.Any(p => p.SeasonId == slotFilter.SeasonId.Value));
            }

            var slots = await query
                .Include(s => s.Products)
                .ToListAsync()
                .ConfigureAwait(false);

            return slots;
        }
    }
}
