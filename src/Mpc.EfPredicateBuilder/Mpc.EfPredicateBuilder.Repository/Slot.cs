namespace Mpc.EfPredicateBuilder.Repository
{
    using System;
    using System.Collections.Generic;

    public class Slot
    {
        public DateTime Created { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
