using NUnit.Framework;
using Moq;

namespace Tests
{
    public class MoqPropertyChainTest
    {
        [Test]
        public void Mock_プロパティチェーンでモック化する場合()
        {
            // setup
            var fooMock = new Mock<Foo>();

            // when
            fooMock.Setup(m => m.Bar.Baz.Message).Returns("Hello Chain");

            // then
            Foo target = fooMock.Object;
            Assert.That(target.Bar.Baz.Message, Is.EqualTo("Hello Chain"));
        }

        [Test]
        public void Mock_それぞれをモック化する場合()
        {
            // 冗長なパターン
            // setup
            var fooMock = new Mock<Foo>();
            var barMock = new Mock<Bar>();
            var bazMock = new Mock<Baz>();

            // when
            fooMock.Setup(m => m.Bar).Returns(barMock.Object);
            barMock.Setup(m => m.Baz).Returns(bazMock.Object);
            bazMock.Setup(m => m.Message).Returns("Hello Redundant");

            // then
            Foo target = fooMock.Object;
            Assert.That(target.Bar.Baz.Message, Is.EqualTo("Hello Redundant"));
        }

        public interface Foo
        {
            Bar Bar { get; }
        }

        public interface Bar
        {
            Baz Baz { get; }
        }

        public interface Baz
        {
            string Message { get; }
        }
    }
}
