using System.Collections.Generic;
using NUnit.Framework;
using Moq;

public class TestFuncProxy
{
    public interface IFunc
    {
        bool Invoke(int number);
    }

    [Test]
    public void Invoke_ReturnsFalse_IfFuncReturnsFalse()
    {
        // Arrange
        var mock = new Mock<IFunc>();
        var target = new FuncProxy(mock.Object);

        // Note: Moqの仕様でSetupなしの場合はdefaultを返す (bool Invoke(...) の場合はfalse)
        // FYI: 実際のテストではテストパターンを明確にするために明示しましょう！

        // Act
        bool actual = target.Invoke(3);

        // Assert
        Assert.That(actual, Is.False);
    }

    [Test]
    public void Invoke_ReturnsTrue_IfFuncReturnsTrue()
    {
        // Arrange
        var mock = new Mock<IFunc>();
        var target = new FuncProxy(mock.Object);

        // Note: 引数3を渡されたらtrueを返す
        mock.Setup(m => m.Invoke(3)).Returns(true);

        // Act
        bool actual = target.Invoke(3);

        // Assert
        Assert.That(actual, Is.True);
    }

    [Test]
    public void Example_SetupSequence()
    {
        var mock = new Mock<IFunc>();

        // 渡された引数に関係なくfalse -> true -> false -> throw Exception
        mock.SetupSequence(m => m.Invoke(It.IsAny<int>()))
            .Returns(false)
            .Returns(true)
            .Returns(false)
            .Throws(new System.Exception("Unexpected Call"));

        Assert.That(mock.Object.Invoke(default), Is.False);
        Assert.That(mock.Object.Invoke(default), Is.True);
        Assert.That(mock.Object.Invoke(default), Is.False);
        Assert.Throws<System.Exception>(() => mock.Object.Invoke(default));
    }

    [Test]
    public void Example_Verify()
    {
        var mock = new Mock<IFunc>();

        mock.Object.Invoke(2);
        mock.Object.Invoke(2);
        mock.Object.Invoke(2);

        mock.Verify(m => m.Invoke(2), Times.Exactly(3));

        // 引数関係なく1回以上コールされたことの検査
        mock.Verify(m => m.Invoke(It.IsAny<int>()), Times.AtLeastOnce);
    }

    [Test]
    public void Example_Callback()
    {
        var messageList = new List<string>();

        var mock1 = new Mock<IFunc>();
        var mock2 = new Mock<IFunc>();
        var mock3 = new Mock<IFunc>();

        // コールされたら messageList に文字列を追加
        mock1.Setup(m => m.Invoke(It.IsAny<int>())).Callback(() => messageList.Add("From 1"));
        mock2.Setup(m => m.Invoke(It.IsAny<int>())).Callback(() => messageList.Add("From 2"));
        mock3.Setup(m => m.Invoke(It.IsAny<int>())).Callback(() => messageList.Add("From 3"));

        mock3.Object.Invoke(0);
        mock1.Object.Invoke(0);
        mock2.Object.Invoke(0);
        mock1.Object.Invoke(0);

        // 合計のコール回数 及び コールされた順番を検査
        Assert.That(messageList.Count, Is.EqualTo(4));
        Assert.That(messageList[0], Is.EqualTo("From 3"));
        Assert.That(messageList[1], Is.EqualTo("From 1"));
        Assert.That(messageList[2], Is.EqualTo("From 2"));
        Assert.That(messageList[3], Is.EqualTo("From 1"));
    }

    public sealed class FuncProxy
    {
        private readonly IFunc func;

        public FuncProxy(IFunc func) => this.func = func;

        public bool Invoke(int number) => this.func.Invoke(number);
    }

    public sealed class ActualFunc : IFunc
    {
        public bool Invoke(int number) => number > 0;
    }
}

public class TestConverter
{
    // リファレンス検索確認用

    public interface IConverter<in TIn, out TOut>
    {
        TOut Convert(TIn value);
    }

    public sealed class IntegerToBooleanConverter : IConverter<int, bool>
    {
        public bool Convert(int value)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class MockIntegerToBooleanConverter : IConverter<int, bool>
    {
        public bool Convert(int value)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class StringToFloatConverter : IConverter<string, float>
    {
        public float Convert(string value)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class MockStringToFloatConverter : IConverter<string, float>
    {
        public float Convert(string value)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class SpecialMockStringToFloatConverter : IConverter<string, float>
    {
        public float Convert(string value)
        {
            throw new System.NotImplementedException();
        }
    }
}
