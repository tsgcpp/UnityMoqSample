using UnityEngine;

/// <summary>
/// MonoBehaviourによるRunner実行のベース
/// </summary>
public abstract class BaseProcessOrder : MonoBehaviour, IProcessor
{
    public IRunner Runner { get; set; } = null;

    public IRunner LateRunner { get; set; } = null;

    protected virtual void Update()
    {
        Runner?.Run();
    }

    protected virtual void LateUpdate()
    {
        LateRunner?.Run();
    }
}
