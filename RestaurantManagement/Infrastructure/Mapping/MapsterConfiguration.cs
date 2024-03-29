using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantManagement.Api.Model.Dto;
using RestaurantManagement.Domain.Entity;

namespace RestaurantManagement.Infrastructure.Mapping
{
    public static class MapsterConfiguration
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<Pizza, PizzaDTO>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<Order, OrderDto>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<User, UserDto>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<Address, AddressDto>
                .NewConfig()
                .TwoWays();


        }
    }
}
