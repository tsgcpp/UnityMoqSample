using UnityEngine;

/// <summary>
/// 銃の発射向けinterface
/// </summary>
public interface IShooter
{
    /// <summary>
    /// 発射試行の実施
    /// </summary>
    /// <returns>false: 発射失敗, true: 発射成功</returns>
    bool Shoot();
}
