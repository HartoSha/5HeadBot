using _5HeadBot.Services.Core.CachingService;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace _5HeadBot.Tests.Services.Tests.Core.Tests.CachingService.Tests
{
    public class MemoryCachingServiceTests
    {
        private MemoryCachingService InitMemoryCachingService()
        {
            var services =
                new ServiceCollection()
                .AddMemoryCache()
                .BuildServiceProvider();

            var memoryCachingService = new MemoryCachingService(
                services.GetRequiredService<IMemoryCache>()
            );

            return memoryCachingService;
        }

        [Fact]
        public void Add_AddsOnNotExpiredDate()
        {
            //arrange
            var memoryCachingService = InitMemoryCachingService();
            var someKey = "key";
            var expected = new { Name = "Petya", Age = 20 };
            var notExpiredDate =
                new DateTimeOffset(DateTime.Today).AddDays(1);

            //act
            var actual = memoryCachingService
                .Set(someKey, expected, notExpiredDate);
            
            //assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_ThrowsOnAlreadyExpiredDate()
        {
            //arrange
            var memoryCachingService = InitMemoryCachingService();
            var someKey = "key";
            var toAdd = new { Name = "Petya", Age = 20 };
            var expiredDate = new DateTimeOffset(DateTime.Today).AddDays(-1);

            //assert
            Assert.Throws<ArgumentOutOfRangeException>(
                //act 
                () => memoryCachingService.Set(someKey, toAdd, expiredDate)
            );
        }

        [Theory]
        [InlineData("wrongKey")]
        [InlineData("")]
        [InlineData("key")]
        [InlineData("big___wrong___Key")]
        public void Get_ShouldReturnNullOnWrongKey(string wrongKey)
        {
            //arrange
            var memoryCachingService = InitMemoryCachingService();

            //act
            var actual = memoryCachingService.Get(wrongKey);
            
            //assert
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("correctKey")]
        [InlineData("")]
        [InlineData("key")]
        [InlineData("big___Long___Key")]
        public void Get_ShouldReturnCorrectValueOnCorrectKey(string correctKey)
        {
            //arrange
            var memoryCachingService = InitMemoryCachingService();
            var expected = new { Name = "Vasya", Age = "25" };

            var absExpiration = new DateTimeOffset(DateTime.Today).AddDays(1);
            memoryCachingService.Set(correctKey, expected, absExpiration);

            //act
            var actual = memoryCachingService.Get(correctKey);

            //assert
            Assert.Equal(expected, actual);
        }
    }
}
