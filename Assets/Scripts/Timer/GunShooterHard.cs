using UnityEngine;

/// <summary>
/// 銃の発射クラス
/// </summary>
public class GunShooterHard : IShooter
{
    private float _coolTime;
    private ILauncher _launcher;

    private float _lastLaunchTime = float.MinValue;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="coolTime">発射毎のクールタイム(秒)</param>
    /// <param name="launcher">実発射オブジェクト</param>
    /// <param name="timer">時間提供オブジェクト</param>
    public GunShooterHard(
        float coolTime,
        ILauncher launcher)
    {
        _coolTime = coolTime;
        _launcher = launcher;
    }

    public bool Shoot()
    {
        float currentTime = Time.time;
        if (currentTime < _lastLaunchTime + _coolTime) {
            // クールタイム中であれば失敗
            return false;
        }

        _launcher.Launch();

        // 発射時間を更新
        _lastLaunchTime = currentTime;

        return true;
    }
}
