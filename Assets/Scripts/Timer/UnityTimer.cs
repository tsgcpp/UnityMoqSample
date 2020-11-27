/// <summary>
/// UnityEngine.Time準拠のTimer
/// </summary>
public class UnityTimer : ITimer
{
    public float Time => UnityEngine.Time.time;
}
