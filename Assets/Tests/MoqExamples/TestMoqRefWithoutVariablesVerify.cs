using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Moq;

namespace Tests
{
    /// <summary>
    /// Moqによるin, ref引数のVerifyの動作検証テストのおまけ
    /// 
    /// 引数に使用する構造体に変数が全く無い場合は、引数にinの有無関係なくIsAnyとRefどちらでも検証できてしまう。
    /// </summary>
    public class TestMoqRefWithoutVariablesVerify
    {
        [Test]
        public void Verify_ICaller_inとrefどちらも指定されていなくても変数を持たない構造体の場合はIsAnyとRefどちらでも検知可能()
        {
            // setup
            var target = new Mock<ICaller>();

            // when
            target.Object.Call(new Parameter());

            // then
            target.Verify(m => m.Call(It.IsAny<Parameter>()), Times.Once());
            target.Verify(m => m.Call(It.Ref<Parameter>.IsAny), Times.Once());
        }

        [Test]
        public void Verify_IRefCaller_refが指定された引数のメソッドのVerifyはRefのみ指定可能()
        {
            // setup
            var target = new Mock<IRefCaller>();

            // when
            var parameter = new Parameter();
            target.Object.Call(ref parameter);

            // then
            // target.Verify(m => m.Call(ref It.IsAny<Parameter>()), Times.Once());  // コンパイルエラー
            target.Verify(m => m.Call(ref It.Ref<Parameter>.IsAny), Times.Once());
        }

        [Test]
        public void Verify_IInCaller_inが指定されていても変数を持たない構造体の場合はIsAnyとRefどちらでも検知可能()
        {
            // setup
            var target = new Mock<IInCaller>();

            // when
            target.Object.Call(new Parameter());

            // then
            target.Verify(m => m.Call(It.IsAny<Parameter>()), Times.Once());
            target.Verify(m => m.Call(It.Ref<Parameter>.IsAny), Times.Once());
        }

        /// <summary>
        /// 確認用構造体
        /// </summary>
        public struct Parameter
        {
            // 変数無し
        }

        /// <summary>
        /// in, ref指定なしのメソッドを持つクラス
        /// </summary>
        public interface ICaller
        {
            void Call(Parameter parameter);
        }

        /// <summary>
        /// ref指定のメソッドを持つクラス
        /// </summary>
        public interface IRefCaller
        {
            void Call(ref Parameter parameter);
        }

        /// <summary>
        /// in指定のメソッドを持つクラス
        /// </summary>
        public interface IInCaller
        {
            void Call(in Parameter parameter);
        }
    }
}
