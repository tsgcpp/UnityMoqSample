using UnityEngine;

public class Gunner : MonoBehaviour
{
    [Tooltip("実発射コンポーネント")]
    [SerializeField] private ProjectileLauncher projectileLauncher;

    [Tooltip("発射遮断コンポーネント")]
    [SerializeField] private GunBlocker gunBlocker;

    [Tooltip("発射毎のクールタイム")]
    [SerializeField] private float fireCoolTime = 0.5f;

    // 発射ボタン
    public KeyCode shootKey = KeyCode.Return;

    // 銃の発射処理オブジェクト(Humble Objectパターン)
    private GunShooter _shooter;

    void Start()
    {
        _shooter = new GunShooter(
            coolTime: fireCoolTime,
            launcher: projectileLauncher,
            blocker: gunBlocker,
            timer: new UnityTimer());
    }

    void Update()
    {
        if (!Input.GetKeyDown(shootKey)) {
            return;
        }

        bool result = _shooter.Shoot();

        if (result) {
            Debug.Log("Succeeded!");
        } else {
            Debug.LogWarning("Failed!");
        }
    }
}
