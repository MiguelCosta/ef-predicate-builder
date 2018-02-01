namespace Mpc.EfPredicateBuilder.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISlotRepository
    {
        Task<Slot> AddAsync(Slot entity);

        Task<IEnumerable<Slot>> GetAllAsync();

        Task<Slot> GetAsync(int id);

        Task<IEnumerable<Slot>> SearchAsync(Filters.SlotFilter slotFilter);

        Task CommitAsync();
    }
}
