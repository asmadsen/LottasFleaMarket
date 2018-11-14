using LottasFleaMarket.Decorators;
using LottasFleaMarket.Interfaces.Decorators;
using Moq;
using Xunit;

namespace LottasFleaMarketTest.Decorators {
    public class CollectorsDecoratorTest {
        [Fact]
        public void ShouldBe25PercentMoreExpensive() {
            var mock = new Mock<IItem>();

            mock.Setup(item => item.Price).Returns(100);

            var collectorsEdition = new CollectorsDecorator(mock.Object);
            
            Assert.Equal(125, collectorsEdition.Price);
        }
    }
}