using UnityEngine;

public class GunnerHard : MonoBehaviour
{
    [Tooltip("実発射コンポーネント")]
    [SerializeField] private ProjectileLauncher projectileLauncher;

    [Tooltip("発射遮断コンポーネント")]
    [SerializeField] private GunBlocker gunBlocker;

    [Tooltip("発射毎のクールタイム")]
    [SerializeField] private float fireCoolTime = 0.5f;

    // 発射ボタン
    public KeyCode shootKey = KeyCode.Return;

    private float _lastLaunchTime = float.MinValue;

    void Update()
    {
        if (!Input.GetKeyDown(shootKey)) {
            return;
        }

        bool result = Shoot();

        if (result) {
            Debug.Log("Succeeded!");
        } else {
            Debug.LogWarning("Failed!");
        }
    }

    private bool Shoot()
    {
        float currentTime = Time.time;
        if (currentTime < _lastLaunchTime + fireCoolTime) {
            // クールタイム中であれば失敗
            return false;
        }

        if (gunBlocker.Block()) {
            // 発射遮断中であれば失敗
            return false;
        }

        projectileLauncher.Launch();

        // 発射時間を更新
        _lastLaunchTime = currentTime;

        return true;
    }
}
