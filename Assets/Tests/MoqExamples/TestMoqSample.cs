using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Moq;

namespace Tests
{
    /// <summary>
    /// Moqの使用例テスト
    /// </summary>
    public class TestMoqSample
    {
        [Test]
        public void Setup_IGetter_メソッドのモック化のサンプル_実体を仕込む場合()
        {
            // IGetterのモックオブジェクトの作成
            var getterMock = new Mock<IGetter>();

            // モックオブジェクトからIGetterインスタンスとして取り出し
            IGetter mockAsGetter = getterMock.Object;

            // モック化していない場合はdefaultを返す
            Assert.Null(mockAsGetter.Get());

            // Getメソッドのモック化("Hello Moq!"を返すように仕込み)
            getterMock.Setup(m => m.Get()).Returns("Hello Moq!");

            // then
            Assert.AreEqual("Hello Moq!", mockAsGetter.Get());
        }

        [Test]
        public void Setup_IGetter_メソッドのモック化のサンプル_nullを仕込む場合()
        {
            // IGetterのモックオブジェクトの作成
            var getterMock = new Mock<IGetter>();

            // モックオブジェクトからIGetterインスタンスとして取り出し
            IGetter mockAsGetter = getterMock.Object;

            // モック化していない場合はdefaultを返す
            Assert.Null(mockAsGetter.Get());

            // Getメソッドのモック化("Hello Moq!"を返すように仕込み)
            getterMock.Setup(m => m.Get()).Returns(null);

            // then
            Assert.Null(mockAsGetter.Get());
        }

        [Test]
        public void Setup_IHoler_プロパティのモック化のサンプル()
        {
            // IHolderのモックオブジェクトの作成
            var holderMock = new Mock<IHolder>();

            // モックオブジェクトからIHolderインスタンスとして取り出し
            IHolder mockAsHolder = holderMock.Object;

            // モック化していない場合はdefaultを返す
            Assert.Null(mockAsHolder.Value);

            // Valueプロパティのモック化("Hi Moq!"を返すように仕込み)
            holderMock.Setup(m => m.Value).Returns("Hi Moq!");

            // then
            Assert.AreEqual("Hi Moq!", mockAsHolder.Value);
        }

        [Test]
        public void Setup_IProcessor_プロパティのモック化_interface型をnullとして返す場合()
        {
            // IProcessorのモックオブジェクトの作成
            var processorMock = new Mock<IProcessor>();

            // モックオブジェクトからIProcessorインスタンスとして取り出し
            var mockAsProcessor = processorMock.Object;

            // Runnerプロパティの戻り値としてnullを設定(as IRunnerが必要)
            processorMock.Setup(m => m.Runner).Returns(null as IRunner);

            // then
            Assert.Null(mockAsProcessor.Runner);
        }

        [Test]
        public void Verifiable_SetUpした内容が実施されたことの確認()
        {
            // IHolderのモックオブジェクトの作成
            var converterMock = new Mock<IConverter>();

            converterMock.Setup(m => m.Convert(2)).Returns("2").Verifiable();
            converterMock.Setup(m => m.Convert(3)).Returns("3").Verifiable();

            var y = converterMock.Object.Convert(2);
            var z = converterMock.Object.Convert(3);

            // Verifiableしたケースがすべてコールされたら正常
            converterMock.Verify();
        }

        [Test]
        public void Verifiable_IsAnyがある場合はその他パターンのコールが必要なことの確認()
        {
            // IHolderのモックオブジェクトの作成
            var converterMock = new Mock<IConverter>();

            converterMock.Setup(m => m.Convert(It.IsAny<int>())).Returns("None").Verifiable();
            converterMock.Setup(m => m.Convert(2)).Returns("2").Verifiable();
            converterMock.Setup(m => m.Convert(3)).Returns("3").Verifiable();

            var x = converterMock.Object.Convert(1);
            var y = converterMock.Object.Convert(2);
            var z = converterMock.Object.Convert(3);

            // Verifiableしたケースがすべてコールされたら正常
            converterMock.Verify();
        }

        [Test]
        public void Verify_ILogger_Logメソッドがコールされていないことの確認()
        {
            // IHolderのモックオブジェクトの作成
            var loggerMock = new Mock<ILogger>();

            // コールされていないことの確認はNever()を使用する
            loggerMock.Verify(m => m.Log(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void Verify_ILogger_Logメソッドが複数コールされたことの確認()
        {
            // ILoggerのモックオブジェクトの作成
            var loggerMock = new Mock<ILogger>();
            var mockAsLogger = loggerMock.Object;

            // 複数回(3回)コール
            mockAsLogger.Log("One");
            mockAsLogger.Log("Two");
            mockAsLogger.Log("Two");

            // 3回コールされたことの確認
            loggerMock.Verify(m => m.Log(It.IsAny<string>()), Times.Exactly(3));

            // 複数回(1回以上)コールされたことの確認
            loggerMock.Verify(m => m.Log(It.IsAny<string>()), Times.AtLeastOnce());

            // "One"でコールは1回であることの確認
            loggerMock.Verify(m => m.Log("One"), Times.Once());

            // "Two"でコールは2回であることの確認
            loggerMock.Verify(m => m.Log("Two"), Times.Exactly(2));
        }

        [Test]
        public void Verify_ILogger_Logメソッドが文字列付きでコールされたことの検証サンプル()
        {
            // ILoggerのモックオブジェクトの作成
            var loggerMock = new Mock<ILogger>();

            // モックオブジェクトからILoggerインスタンスとして取り出し
            ILogger mockAsLogger = loggerMock.Object;

            // loggerインスタンスのLogメソッドをコール
            mockAsLogger.Log("Aloha Moq!");

            // VerifyによりLogメソッドが特定の引数でコールされたことの検証
            loggerMock.Verify(m => m.Log("Aloha Moq!"), Times.Once());

            // 引数は任意でコールされたかどうかの検証のみの場合
            loggerMock.Verify(m => m.Log(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Verify_ILogger_Logメソッドがnullでコールされたことの検証サンプル()
        {
            // ILoggerのモックオブジェクトの作成
            var loggerMock = new Mock<ILogger>();
            ILogger mockAsLogger = loggerMock.Object;

            // loggerインスタンスのLogメソッドをnullでコール
            mockAsLogger.Log(null);

            // VerifyによりLogメソッドが特定の引数でコールされたことの検証
            loggerMock.Verify(m => m.Log(null), Times.Once());

            // nullの場合でもIt.IsAny<T>()は有効
            loggerMock.Verify(m => m.Log(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Verify_ILogger_Logメソッドにコールバックを仕込むサンプル()
        {
            // コールバック時のパラメータ格納用List
            var messageList = new List<string>();

            // ILoggerのモックオブジェクトの作成
            var loggerMock = new Mock<ILogger>();

            // モックオブジェクトにコールバックを仕込む
            loggerMock
                .Setup(m => m.Log(It.IsAny<string>()))
                .Callback<string>(message => messageList.Add(message));

            // ILogger.Logメソッドをコール
            ILogger mockAsLogger = loggerMock.Object;
            mockAsLogger.Log("Top secret!");

            // コールバックによりメソッドに渡された引数の確認
            Assert.AreEqual(1, messageList.Count);
            Assert.AreEqual("Top secret!", messageList[0]);
        }

        // 戻り値有りのメソッドを持つinterface
        public interface IGetter
        {
            object Get();
        }

        // プロパティを持つinterface
        public interface IHolder
        {
            object Value { get; }
        }

        // 引数を持つinterface
        public interface ILogger
        {
            void Log(string message);
        }

        public interface IConverter
        {
            string Convert(int value);
        }
    }
}
