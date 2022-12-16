using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Moq;

namespace Tests
{
    public class TestMoqException
    {
        [Test]
        public void Moq_Setup_通常メソッドの場合はモック化できないこと()
        {
            var target = new Mock<MockTarget>();
            Assert.Throws<System.NotSupportedException>(() => target.Setup(m => m.Func()).Returns(3));
            Assert.AreNotEqual(3, target.Object.Func());
        }

        [Test]
        public void Moq_Setup_virtualメソッドの場合はモック化できること()
        {
            var target = new Mock<MockTarget>();
            Assert.DoesNotThrow(() => target.Setup(m => m.FuncVirtual()).Returns(3));
            Assert.AreEqual(3, target.Object.FuncVirtual());
        }

        [Test]
        public void Moq_Setup_abstractメソッドの場合はモック化できること()
        {
            var target = new Mock<MockTarget>();
            Assert.DoesNotThrow(() => target.Setup(m => m.FuncAbstract()).Returns(3));
            Assert.AreEqual(3, target.Object.FuncAbstract());
        }

        public abstract class MockTarget
        {
            public object Func()
            {
                return 1;
            }

            public virtual object FuncVirtual()
            {
                return 2;
            }

            public abstract object FuncAbstract();
        }
    }
}
