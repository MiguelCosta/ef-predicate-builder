namespace Mpc.EfPredicateBuilder.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using LinqKit;
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

        /// <summary>
        /// https://github.com/scottksmith95/LINQKit
        /// https://github.com/scottksmith95/LINQKit#plugging-expressions-into-entitysets--entitycollections-the-solution
        /// https://stackoverflow.com/questions/43057695/cant-extract-this-function-from-an-expression
        /// https://github.com/aspnet/EntityFrameworkCore/issues/8019
        /// </summary>
        /// <param name="slotFilter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Slot>> SearchAsync(Filters.SlotFilter slotFilter)
        {
            var query = appContext.Slots
                .Include(s => s.Products)
                .AsQueryable()
                .AsNoTracking()
                .AsExpandable();

            if (!string.IsNullOrWhiteSpace(slotFilter.Name))
            {
                query = query.Where(s => s.Name == slotFilter.Name);
            }

            Expression<Func<Product, bool>> predicate = PredicateBuilder.New<Product>(true);

            if (slotFilter.BrandId.HasValue)
            {
                predicate = predicate.And(p => p.BrandId == slotFilter.BrandId.Value);
            }

            if (slotFilter.SeasonId.HasValue)
            {
                predicate = predicate.And(p => p.SeasonId == slotFilter.SeasonId.Value);
            }

            query = query.Where(s => s.Products.Any(predicate.Compile()));

            var slots = await query
                .ToListAsync()
                .ConfigureAwait(false);

            return slots;
        }
    }
}
