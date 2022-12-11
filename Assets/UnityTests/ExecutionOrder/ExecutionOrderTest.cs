using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;

using Moq;

namespace Tests
{
    /// <summary>
    /// UnityのExecutionOrderとUniTaskのPlayerLoopTimingの実行順序確認テスト
    /// </summary>
    public class ExecutionOrderTest
    {
        private List<string> _callbackMessageList;

        private GameObject _testGameObject;

        private List<BaseProcessOrder> _processorOrderList;
        private List<ProcessorUniTask> _processorUniTaskList;

        [SetUp]
        public void SetUp()
        {
            _callbackMessageList = new List<string>();
            _testGameObject = new GameObject("__TEST_OBJECT__");

            _processorOrderList = new List<BaseProcessOrder>();
            _processorOrderList.Add(CreateProcessorOrder<ProcessorNoOrder>());
            _processorOrderList.Add(CreateProcessorOrder<ProcessorOrderMinValue>());
            _processorOrderList.Add(CreateProcessorOrder<ProcessorOrderMaxValue>());
            _processorOrderList.Add(CreateProcessorOrder<ProcessorOrderP1>());
            _processorOrderList.Add(CreateProcessorOrder<ProcessorOrderM1>());

            _processorUniTaskList = new List<ProcessorUniTask>();
            _processorUniTaskList.Add(CreateProcessorUniTask(PlayerLoopTiming.PreUpdate));
            _processorUniTaskList.Add(CreateProcessorUniTask(PlayerLoopTiming.LastPreUpdate));
            _processorUniTaskList.Add(CreateProcessorUniTask(PlayerLoopTiming.Update));
            _processorUniTaskList.Add(CreateProcessorUniTask(PlayerLoopTiming.LastUpdate));
            _processorUniTaskList.Add(CreateProcessorUniTask(PlayerLoopTiming.PreLateUpdate));
            _processorUniTaskList.Add(CreateProcessorUniTask(PlayerLoopTiming.LastPreLateUpdate));
            _processorUniTaskList.Add(CreateProcessorUniTask(PlayerLoopTiming.PostLateUpdate));

            // FYI: LastPostLateUpdateとコルーチンのWaitForEndOfFrameの順序をRuntimeとEditorで一致されることが難しいため検証からは除外
            // _processorUniTaskList.Add(CreateProcessorUniTask(PlayerLoopTiming.LastPostLateUpdate));

            _processorUniTaskList.Add(CreateProcessorUniTask(PlayerLoopTiming.TimeUpdate));
            _processorUniTaskList.Add(CreateProcessorUniTask(PlayerLoopTiming.LastTimeUpdate));
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(_testGameObject);
        }

        private IRunner CreateRunnerMock(string message)
        {
            var runnerMock = new Mock<IRunner>();

            // MockのIRunner.Run()がコールされた際にメッセージを登録するように仕込む(いわゆるSpy)
            runnerMock
                .Setup(mock => mock.Run())
                .Callback(() => _callbackMessageList.Add(message));
            return runnerMock.Object;
        }

        private BaseProcessOrder CreateProcessorOrder<T>() where T : BaseProcessOrder
        {
            var processor = _testGameObject.AddComponent<T>();
            processor.enabled = false;  // trueになるまで待機

            string className = processor.GetType().Name;

            processor.Runner = CreateRunnerMock(className + " Update");
            processor.LateRunner = CreateRunnerMock(className + " LateUpdate");

            return processor;
        }

        private ProcessorUniTask CreateProcessorUniTask(PlayerLoopTiming timing)
        {
            var processor = new ProcessorUniTask();

            processor.Timing = timing;
            processor.Runner = CreateRunnerMock("UniTask PlayerLoopTiming." + timing);

            return processor;
        }

        [UnityTest]
        public IEnumerator ExecutionOrder_UnitTaskとDefaultExecutionOrder混合の実行順の確認() => UniTask.ToCoroutine(async () =>
        {
            await UniTask.Yield(timing: PlayerLoopTiming.LastPostLateUpdate);
            // まだコールバックが発生していないことの確認
            Assert.AreEqual(0, _callbackMessageList.Count);

            // 各コンポーネントの起動
            _processorOrderList.ForEach(processor => processor.enabled = true);

            // UniTaskの実行
            _processorUniTaskList.ForEach(processor => processor.Process());

            await UniTask.Yield(timing: PlayerLoopTiming.LastPostLateUpdate);
            Assert.AreEqual(19, _callbackMessageList.Count);

            Assert.AreEqual("UniTask PlayerLoopTiming.TimeUpdate", _callbackMessageList[0]);
            Assert.AreEqual("UniTask PlayerLoopTiming.LastTimeUpdate", _callbackMessageList[1]);
            Assert.AreEqual("UniTask PlayerLoopTiming.PreUpdate", _callbackMessageList[2]);
            Assert.AreEqual("UniTask PlayerLoopTiming.LastPreUpdate", _callbackMessageList[3]);
            Assert.AreEqual("UniTask PlayerLoopTiming.Update", _callbackMessageList[4]);
            Assert.AreEqual("ProcessorOrderMinValue Update", _callbackMessageList[5]);
            Assert.AreEqual("ProcessorOrderM1 Update", _callbackMessageList[6]);
            Assert.AreEqual("ProcessorNoOrder Update", _callbackMessageList[7]);
            Assert.AreEqual("ProcessorOrderP1 Update", _callbackMessageList[8]);
            Assert.AreEqual("ProcessorOrderMaxValue Update", _callbackMessageList[9]);
            Assert.AreEqual("UniTask PlayerLoopTiming.LastUpdate", _callbackMessageList[10]);
            Assert.AreEqual("UniTask PlayerLoopTiming.PreLateUpdate", _callbackMessageList[11]);
            Assert.AreEqual("ProcessorOrderMinValue LateUpdate", _callbackMessageList[12]);
            Assert.AreEqual("ProcessorOrderM1 LateUpdate", _callbackMessageList[13]);
            Assert.AreEqual("ProcessorNoOrder LateUpdate", _callbackMessageList[14]);
            Assert.AreEqual("ProcessorOrderP1 LateUpdate", _callbackMessageList[15]);
            Assert.AreEqual("ProcessorOrderMaxValue LateUpdate", _callbackMessageList[16]);
            Assert.AreEqual("UniTask PlayerLoopTiming.LastPreLateUpdate", _callbackMessageList[17]);
            Assert.AreEqual("UniTask PlayerLoopTiming.PostLateUpdate", _callbackMessageList[18]);
        });
    }
}
