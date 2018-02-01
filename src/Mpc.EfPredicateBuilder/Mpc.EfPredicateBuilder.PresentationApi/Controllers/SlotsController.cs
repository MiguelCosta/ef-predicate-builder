namespace Mpc.EfPredicateBuilder.PresentationApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Mpc.EfPredicateBuilder.Repository;
    using Mpc.EfPredicateBuilder.Repository.Filters;

    [Produces("application/json")]
    [Route("api/slots")]
    public class SlotsController : Controller
    {
        private ISlotRepository slotRepository;

        public SlotsController(ISlotRepository slotRepository)
        {
            this.slotRepository = slotRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Slot>> GetAllAsync([FromQuery] SlotFilter slotFilter)
        {
            await Repository.Seed.SlotsSeed.SeedDataBaseAsync(this.slotRepository).ConfigureAwait(false);

            var slots = await this.slotRepository.SearchAsync(slotFilter).ConfigureAwait(false);
            return slots;
        }

        [HttpGet("{id}")]
        public async Task<Slot> GetAsync(int id)
        {
            var slot = await this.slotRepository.GetAsync(id).ConfigureAwait(false);
            return slot;
        }
    }
}
