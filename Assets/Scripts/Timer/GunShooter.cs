/// <summary>
/// 銃の発射クラス
/// </summary>
public class GunShooter : IShooter
{
    private float _coolTime;
    private IBlocker _blocker;
    private ILauncher _launcher;
    private ITimer _timer;

    private float _lastLaunchTime = float.MinValue;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="coolTime">発射毎のクールタイム(秒)</param>
    /// <param name="blocker">発射遮断オブジェクト</param>
    /// <param name="launcher">実発射オブジェクト</param>
    /// <param name="timer">時間提供オブジェクト</param>
    public GunShooter(
        float coolTime,
        IBlocker blocker,
        ILauncher launcher,
        ITimer timer)
    {
        _coolTime = coolTime;
        _blocker = blocker;
        _launcher = launcher;
        _timer = timer;
    }

    public bool Shoot()
    {
        float currentTime = _timer.Time;
        if (currentTime < _lastLaunchTime + _coolTime) {
            // クールタイム中であれば失敗
            return false;
        }

        if (_blocker.Block()) {
            // 発射遮断中であれば失敗
            return false;
        }

        _launcher.Launch();

        // 発射時間を更新
        _lastLaunchTime = currentTime;

        return true;
    }
}
