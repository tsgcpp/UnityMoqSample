using NUnit.Framework;

public class TestFuncProxy
{
    [Test]
    public void Invoke_ReturnsFalse_IfFuncReturnsFalse()
    {
        // Arrange
        var mock = new Moq.Mock<IFunc>();
        var target = new FuncProxy(mock.Object);

        // Note: Moqの仕様でSetupなしの場合はdefaultを返す (Invokeの場合はfalse)
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
        var mock = new Moq.Mock<IFunc>();
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
        var mock = new Moq.Mock<IFunc>();

        // 渡された引数に関係なくfalse -> true -> false -> throw Exception
        mock.SetupSequence(m => m.Invoke(Moq.It.IsAny<int>()))
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
        var mock = new Moq.Mock<IFunc>();

        mock.Object.Invoke(2);
        mock.Object.Invoke(2);
        mock.Object.Invoke(2);

        mock.Verify(m => m.Invoke(2), Moq.Times.Exactly(3));

        // 引数関係なく1回以上コールされたことの検査
        mock.Verify(m => m.Invoke(Moq.It.IsAny<int>()), Moq.Times.AtLeastOnce);
    }

    public sealed class FuncProxy
    {
        private readonly IFunc func;

        public FuncProxy(IFunc func) => this.func = func;

        public bool Invoke(int number) => this.func.Invoke(number);
    }

    public interface IFunc
    {
        bool Invoke(int number);
    }
    public sealed class ActualFunc : IFunc
    {
        public bool Invoke(int number)
        {
            throw new System.NotImplementedException();
        }
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
