using UnityEngine;

/// <summary>
/// 銃の発射遮断コンポーネント
/// </summary>
public class GunBlocker : MonoBehaviour, IBlocker
{
    public KeyCode blockKey = KeyCode.Tab;

    public bool Block()
    {
        // ボタンを押している間は遮断
        return Input.GetKey(blockKey);
    }
}
