using System;

namespace BusEvenement.Evenement
{
    public class StandardEvenement
    {
        public StandardEvenement()
        {
            Id = Guid.NewGuid();
            DateCreation = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public DateTime DateCreation { get; }
    }
}
