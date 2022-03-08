using System;
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
            var fooMock = new Mock<IFoo>();

            // when
            fooMock.Setup(m => m.Bar.Baz.Message).Returns("Hello Chain");

            // then
            IFoo target = fooMock.Object;
            Assert.That(target.Bar.Baz.Message, Is.EqualTo("Hello Chain"));

            // 各インスタンスが生成されている
            Assert.That(target.Bar, Is.Not.Null);
            Assert.That(target.Bar.Baz, Is.Not.Null);
        }

        [Test]
        public void Mock_それぞれをモック化する場合()
        {
            // 冗長なパターン
            // setup
            var fooMock = new Mock<IFoo>();
            var barMock = new Mock<IBar>();
            var bazMock = new Mock<IBaz>();

            // when
            fooMock.Setup(m => m.Bar).Returns(barMock.Object);
            barMock.Setup(m => m.Baz).Returns(bazMock.Object);
            bazMock.Setup(m => m.Message).Returns("Hello Redundant");

            // then
            IFoo target = fooMock.Object;
            Assert.That(target.Bar.Baz.Message, Is.EqualTo("Hello Redundant"));
            Assert.That(target.Bar.Baz, Is.Not.Null);

            // 各インスタンスが生成されている
            Assert.That(target.Bar, Is.Not.Null);
            Assert.That(target.Bar.Baz, Is.Not.Null);
        }

        [Test]
        public void Mock_モック化していない場合はNullReferenceException()
        {
            // setup
            var fooMock = new Mock<IFoo>();

            // when
            // fooMock.Setup(m => m.Bar.Baz.Message).Returns("Hello Chain");

            // then
            IFoo target = fooMock.Object;
            Assert.That(() => target.Bar.Baz.Message, Throws.TypeOf<NullReferenceException>());
        }

        public interface IFoo
        {
            IBar Bar { get; }
        }

        public interface IBar
        {
            IBaz Baz { get; }
        }

        public interface IBaz
        {
            string Message { get; }
        }
    }
}
