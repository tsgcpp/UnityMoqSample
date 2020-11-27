/// <summary>
/// Runner実行向けinterface
/// </summary>
public interface IProcessor
{
    /// <summary>
    /// Runnerの指定
    /// </summary>
    IRunner Runner { get; set; }

    /// <summary>
    /// LateUpdate用Runnerの指定
    /// </summary>
    IRunner LateRunner { get; set; }
}
