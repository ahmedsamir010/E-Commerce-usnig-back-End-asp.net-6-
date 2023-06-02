using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;


namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
            {
                throw new ArgumentException("Invalid basketId");
            }

            try
            {
                return await _database.KeyDeleteAsync(basketId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting basket: {ex.Message}");
                return false;
            }
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
            {
                throw new ArgumentException("Invalid basketId");
            }

            try
            {
                var basketValue = await _database.StringGetAsync(basketId);

                if (basketValue.HasValue && !basketValue.IsNull)
                {
                    var serializedBasket = basketValue.ToString();
                    var deserializedBasket = JsonSerializer.Deserialize<CustomerBasket>(serializedBasket);
                    return deserializedBasket;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing basket: {ex.Message}");
            }
            return null;
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }

            try
            {
                var updateOrCreateBasket = await _database.StringSetAsync(
                    basket.Id,
                    JsonSerializer.Serialize(basket),
                    TimeSpan.FromDays(1)
                );

                if (updateOrCreateBasket)
                {
                    return await GetBasketAsync(basket.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating basket: {ex.Message}");
            }

            return null;
        }
    }
}