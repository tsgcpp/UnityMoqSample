using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// UniTaskによるRunner実行
/// </summary>
public class ProcessorUniTask : IProcessor
{
    public IRunner Runner { get; set; } = null;

    // 未使用
    public IRunner LateRunner { get; set; } = null;

    public PlayerLoopTiming Timing { get; set; } = PlayerLoopTiming.Update;

    /// <summary>
    /// Runnerの非同期実行
    /// </summary>
    public void Process()
    {
        ProcessAsync().Forget();
    }

    private async UniTask ProcessAsync()
    {
        await UniTask.Yield(Timing);
        Runner?.Run();
    }
}
