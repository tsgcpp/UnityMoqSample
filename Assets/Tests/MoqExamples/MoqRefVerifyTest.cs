using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Moq;

namespace Tests
{
    /// <summary>
    /// Moqによるin, ref引数のVerifyの動作検証テスト
    /// 
    /// in, ref引数を持つメソッドがコールされたかの確認は`It.IsAny<T>()`では検知できない。
    /// in, ref引数の場合は`It.Ref<Parameter>.IsAny`でコールの検知が確認可能となる。
    /// 
    /// このテストでin, refの検知検証を行う。
    /// </summary>
    public class MoqRefVerifyTest
    {
        [Test]
        public void Verify_ICaller_inとrefどちらも指定されていない引数のメソッドのVerifyはIsAnyのみ検知可能()
        {
            // setup
            var target = new Mock<ICaller>();

            // when
            target.Object.Call(new Parameter { x = 1 });

            // then
            target.Verify(m => m.Call(It.IsAny<Parameter>()), Times.Once());
            target.Verify(m => m.Call(It.Ref<Parameter>.IsAny), Times.Never());
        }

        [Test]
        public void Verify_IRefCaller_refが指定された引数のメソッドのVerifyはRefのみ指定可能()
        {
            // setup
            var target = new Mock<IRefCaller>();

            // when
            var parameter = new Parameter { x = 1 };
            target.Object.Call(ref parameter);

            // then
            // target.Verify(m => m.Call(ref It.IsAny<Parameter>()), Times.Once());  // コンパイルエラー
            target.Verify(m => m.Call(ref It.Ref<Parameter>.IsAny), Times.Once());
        }

        [Test]
        public void Verify_IInCaller_inが指定された引数のメソッドのVerifyはRefのみ検知可能()
        {
            // setup
            var target = new Mock<IInCaller>();

            // when
            target.Object.Call(new Parameter { x = 1 });

            // then
            target.Verify(m => m.Call(It.IsAny<Parameter>()), Times.Never());
            target.Verify(m => m.Call(It.Ref<Parameter>.IsAny), Times.Once());
        }
        
        /// <summary>
        /// 確認用構造体
        /// </summary>
        public struct Parameter
        {
            public int x;
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
