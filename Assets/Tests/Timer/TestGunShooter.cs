using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Moq;

namespace Tests
{
    public class TestGunShooter
    {
        GunShooter _target;

        Mock<ITimer> _timerMock;  // Timerのモック
        Mock<IBlocker> _blockerMock;  // 発射遮断のモック
        Mock<ILauncher> _launcherMock;  // 実発射のモック(Spyとして使用)

        private const float CoolTime = 0.2f;  // テスト用のクールタイム
        private const float CurrentTime = 123.45f;  // テスト用の現在時刻

        [SetUp]
        public void SetUp()
        {
            // SetUp時は正常系になる用に仕込むこと推奨

            // モックの初期化
            _timerMock = new Mock<ITimer>();
            _blockerMock = new Mock<IBlocker>();
            _launcherMock = new Mock<ILauncher>();

            // 疑似パラメータの仕込み
            _timerMock.Setup(mock => mock.Time).Returns(CurrentTime);
            _blockerMock.Setup(mock => mock.Block()).Returns(false);

            // テスト対象の生成
            _target = new GunShooter(
                coolTime: CoolTime,
                blocker: _blockerMock.Object,
                launcher: _launcherMock.Object,
                timer: _timerMock.Object);
        }

        [Test]
        public void Shoot_OK_初発射かつ遮断無しの場合に発射実施されること()
        {
            // when
            bool actual = _target.Shoot();

            // then
            Assert.True(actual);

            // 発射処理が実施されたことの確認
            _launcherMock.Verify(mock => mock.Launch(), Times.Once());
        }

        [Test]
        public void Shoot_OK_発射後かつクールタイムが過ぎた後の場合に再度発射実施されること()
        {
            // setup
            _target.Shoot();  // 初期発射の実施(_lastLaunchTimeの更新)
            _launcherMock.Verify(mock => mock.Launch(), Times.Once());  // 1回目の発射が行われたことの確認

            // 次にShoot()が実施された際のTimerの時間を変更
            _timerMock.Setup(mock => mock.Time).Returns(CurrentTime + CoolTime + 0.001f);

            // when
            bool actual = _target.Shoot();  // 再度発射試行

            // then
            Assert.True(actual);

            // 再度、発射処理が実施されたことの確認
            _launcherMock.Verify(mock => mock.Launch(), Times.Exactly(2));  // 2回目の発射が行われたことの確認
        }

        /*
         異常系の場合は成功パターンからモックのパラメータのうち、1つのみを異常値に上書きして確認していく
         */

        [Test]
        public void Shoot_NG_発射遮断中であれば発射が実施されないこと()
        {
            // setup
            _blockerMock.Setup(mock => mock.Block()).Returns(true);  // Blockメソッドの戻り値を異常値に上書き

            // when
            bool actual = _target.Shoot();

            // then
            Assert.False(actual);

            // 発射処理が"実施されていないこと"の確認
            _launcherMock.Verify(mock => mock.Launch(), Times.Never());
        }

        [Test]
        public void Shoot_NG_発射後かつクールタイムが過ぎる前の場合に再度発射実施されないこと()
        {
            // setup
            _target.Shoot();  // 初期発射の実施(_lastLaunchTimeの更新)
            _launcherMock.Verify(mock => mock.Launch(), Times.Once());  // 1回目の発射が行われたことの確認

            // 次にShoot()が実施された際のTimerの時間を変更
            _timerMock.Setup(mock => mock.Time).Returns(CurrentTime + CoolTime - 0.001f);

            // when
            bool actual = _target.Shoot();  // 再度発射試行

            // then
            Assert.False(actual);

            // 再度、発射処理が実施"されていないこと"(メソッドのコール回数に変更がないこと)の確認
            _launcherMock.Verify(mock => mock.Launch(), Times.Once());
        }

        [Test]
        public void Shoot_NG_発射後かつクールタイムが過ぎた後だが遮断中の場合には発射されないこと()
        {
            /*
            他のテストでコードカバレッジはすでに100%となっていますが、
            満たしたい条件であるなら入れておくべきと思います。
             */

            // setup
            _target.Shoot();  // 初期発射の実施(_lastLaunchTimeの更新)
            _launcherMock.Verify(mock => mock.Launch(), Times.Once());  // 1回目の発射が行われたことの確認

            // 次にShoot()が実施された際のTimerの時間を変更
            _timerMock.Setup(mock => mock.Time).Returns(CurrentTime + CoolTime + 0.001f);
            _blockerMock.Setup(mock => mock.Block()).Returns(true);  // Blockメソッドの戻り値を異常値に上書き

            // when
            bool actual = _target.Shoot();  // 再度発射試行

            // then
            Assert.False(actual);

            // 再度、発射処理が実施"されていないこと"(メソッドのコール回数に変更がないこと)の確認
            _launcherMock.Verify(mock => mock.Launch(), Times.Once());
        }
    }
}
