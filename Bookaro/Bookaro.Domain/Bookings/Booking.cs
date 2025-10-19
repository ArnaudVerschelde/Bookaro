using Bookaro.Domain.Abstractions;

namespace Bookaro.Domain.Bookings;

public sealed class Booking : Entity
{
    private Booking()
    {
        // Required by Entity Framework Core
    }
}
