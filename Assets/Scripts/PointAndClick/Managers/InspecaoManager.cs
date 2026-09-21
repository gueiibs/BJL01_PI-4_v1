using UnityEngine;

public class InspecaoManager : MonoBehaviour
{
    public static InspecaoManager Instance { get; private set; }
    public GameObject CurrentObject { get; private set; }

    [SerializeField] private GameObject painelInspecao;
    [SerializeField] private Transform spawnInspecao;
    [SerializeField] private Camera cameraInspecao;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (painelInspecao != null)
        {
            painelInspecao.SetActive(false);
        }
        if (cameraInspecao != null)
        {
            cameraInspecao.enabled = false;
        }
    }
    public void Abrir(GameObject prefab)
    {
        if (prefab == null || spawnInspecao == null)
        {
            Debug.LogWarning("InspectionManager não configurado");
            return;
        }

        if (cameraInspecao != null) cameraInspecao.enabled = true;
        if (painelInspecao != null) painelInspecao.SetActive(true);
    }

    public void Fechar()
    {
        if (CurrentObject != null)
        {
            Destroy(CurrentObject);
            CurrentObject = null;
        }

        if (cameraInspecao != null) cameraInspecao.enabled = false;
        if (painelInspecao != null) painelInspecao.SetActive(false);
    }
}
