using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mpc.EfPredicateBuilder.Repository.Seed
{
    public static class SlotsSeed
    {
        public static async Task SeedDataBaseAsync(ISlotRepository slotRepository)
        {
            var slot1 = new Slot
            {
                Id = 1,
                Created = new DateTime(2018, 1, 1),
                Name = "Slot1",
                Products = new List<Product>
                {
                    new Product
                    {
                        BrandId = 1,
                        CategoryId = 1,
                        Id = 1,
                        Name = "Product 1",
                        SeasonId = 1,
                        SlotId = 1
                    },
                    new Product
                    {
                        BrandId = 2,
                        CategoryId = 2,
                        Id = 2,
                        Name = "Product 2",
                        SeasonId = 2,
                        SlotId = 1
                    },
                }
            };

            var currentSlot1 = await slotRepository.GetAsync(slot1.Id).ConfigureAwait(false);

            if (currentSlot1 == null)
            {
                var slot1Entry = await slotRepository.AddAsync(slot1).ConfigureAwait(false);
                await slotRepository.CommitAsync().ConfigureAwait(false);
            }

            var slots = await slotRepository.GetAllAsync().ConfigureAwait(false);
        }
    }
}
