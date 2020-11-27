using UnityEngine;

/// <summary>
/// 発射物の発射用コンポーネント
/// </summary>
public class ProjectileLauncher : MonoBehaviour, ILauncher
{
    public void Launch()
    {
        // 実発射処理(今回はログのみ)
        Debug.Log("Launched!");
    }
}
