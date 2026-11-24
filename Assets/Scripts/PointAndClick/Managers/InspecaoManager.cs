using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspecaoManager : MonoBehaviour
{
    //maior dimensão do item, em unidades do mundo
    [SerializeField] private float tamanhoAlvo = 1.5f;
    //Instance = mesma lógica do ClickerManager.cs
    public static InspecaoManager Instance { get; private set; }

    //guarda o obj em inspeção
    public GameObject CurrentObject { get; private set; }

    [Header("Cena")]
    [SerializeField] private GameObject painelInspecao;
    [SerializeField] private Transform spawnInspecao;
    [SerializeField] private Camera cameraInspecao;

    [SerializeField] private GameObject painelDescricao;   //espaço ao lado do item
    [SerializeField] private TextMeshProUGUI textoDescricao; //desc item

    private void Awake()
    {
        //se já tem outro inspecaomanager ele destroi
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; //define como principal

        //painel inicia desativado
        if (painelInspecao != null) painelInspecao.SetActive(false);

        if (cameraInspecao != null)
            cameraInspecao.enabled = false;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Fechar();
        }
    }

    public void Abrir(ItemData item)
    {
        if (item == null) return;
        Abrir(item.prefabInspecao);   // fecha o anterior, esconde a descrição, instancia

        bool temTexto = !string.IsNullOrEmpty(item.descricao);
        painelDescricao.SetActive(temTexto);
        textoDescricao.text = item.descricao;
    }

    //pega o obj guardado e abre a tela de inspeção
    public void Abrir(GameObject prefab)
    {
        if (prefab == null || spawnInspecao == null)
        {
            Debug.LogWarning("prefab nao configurado");
            return;
        }

        //fecha a ultima p/ abrir a prox
        Fechar();

        //inspeções sem ItemData não herdam a descrição anterior
        painelDescricao.SetActive(false);

        //cria uma copia do prefab guardado
        CurrentObject = Instantiate(prefab, spawnInspecao.position, spawnInspecao.rotation, spawnInspecao);

        if (cameraInspecao != null)
            cameraInspecao.enabled = true; //ativa cam da inspeção

        if (painelInspecao != null) painelInspecao.SetActive(true);

        //centraliza e normaliza o tamanho pelo mesh
        Renderer[] rs = CurrentObject.GetComponentsInChildren<Renderer>();
        if (rs.Length > 0)
        {
            Bounds b = rs[0].bounds;
            foreach (Renderer r in rs) b.Encapsulate(r.bounds);

            //1. centraliza
            Vector3 desvio = b.center - CurrentObject.transform.position;
            foreach (Transform filho in CurrentObject.transform)
                filho.position -= desvio;

            //2. escala para a maior dimensão virar "tamanhoAlvo"
            float maior = Mathf.Max(b.size.x, b.size.y, b.size.z);
            if (maior > 0.0001f)
                CurrentObject.transform.localScale *= tamanhoAlvo / maior;
        }

        Debug.Log("Abrido");
    }

   /* public void ResetPosicao()
    {
        if (CurrentObject != null)
        {
            CurrentObject = null;
        }
    }*/ 
   // - > testar botao reset

    //faz a tela fechar
    public void Fechar()
    {
        //destroi copia do prefab  + limpa armazem do obj
        if (CurrentObject != null)
        {
            Destroy(CurrentObject);
            CurrentObject = null;
        }

        //desativa a cam + painel
        if (cameraInspecao != null) cameraInspecao.enabled = false;
        if (painelInspecao != null) painelInspecao.SetActive(false);
        painelDescricao.SetActive(false);
    }
}