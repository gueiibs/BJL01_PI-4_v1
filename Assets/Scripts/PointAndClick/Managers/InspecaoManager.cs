using UnityEngine;

public class InspecaoManager : MonoBehaviour
{
    public static InspecaoManager Instance { get; private set; }

    public GameObject CurrentObject { get; private set; }

    [Header("Referências de cena")]
    [SerializeField] private GameObject painelInspecao;
    [SerializeField] private Transform spawnInspecao;
    [SerializeField] private Camera cameraInspecao;

    public Camera CameraAtual => cameraInspecao;

    private float zoomPadrao;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (painelInspecao != null) painelInspecao.SetActive(false);
        if (cameraInspecao != null)
        {
            cameraInspecao.enabled = false;
            zoomPadrao = cameraInspecao.fieldOfView;
        }
    }

    public void Abrir(GameObject prefab)
    {
        if (prefab == null || spawnInspecao == null)
        {
            Debug.LogWarning("InspecaoManager: prefab ou spawnInspecao não configurado.");
            return;
        }

        Fechar();

        CurrentObject = Instantiate(prefab, spawnInspecao.position, spawnInspecao.rotation, spawnInspecao);

        if (cameraInspecao != null)
        {
            cameraInspecao.enabled = true;
            cameraInspecao.fieldOfView = zoomPadrao; //reseta o zoom a cada novo item aberto
        }
        if (painelInspecao != null) painelInspecao.SetActive(true);

        Debug.Log("Abrido");
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