using LottasFleaMarket.Decorators;
using LottasFleaMarket.Interfaces.Decorators;
using Moq;
using Xunit;

namespace LottasFleaMarketTest.Decorators {
    public class BadConditionDecoratorTest {
        [Fact]
        public void ShouldSetPriceToHalf() {
            var mock = new Mock<IItem>();

            mock.Setup(item => item.Price).Returns(100);

            var badItem = new BadConditionDecorator(mock.Object);

            Assert.Equal(50, badItem.Price);
        }
    }
}