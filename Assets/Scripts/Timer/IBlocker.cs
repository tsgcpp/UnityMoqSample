/// <summary>
/// 遮断処理向けinterface
/// </summary>
public interface IBlocker
{
    /// <summary>
    /// 発射の遮断判定
    /// </summary>
    /// <returns>false: 非遮断, true: 遮断</returns>
    bool Block();
}
