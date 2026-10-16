using System.Collections.Generic;
using UnityEngine;

//aproximação com pilha: zoom dentro de zoom, botão volta um nível por vez
public class ZoomManager : MonoBehaviour
{
    public static ZoomManager Instance { get; private set; }

    [SerializeField] private Transform raizZooms;    //objeto "Zooms"
    [SerializeField] private GameObject botaoVoltar;
    [SerializeField] private GameObject painelSetas; //some durante o zoom

    private readonly Dictionary<string, GameObject> paineis = new Dictionary<string, GameObject>();
    private readonly Stack<GameObject> pilha = new Stack<GameObject>();

    public bool Aberto => pilha.Count > 0;
    public Transform Raiz => raizZooms;

    private void Awake()
    {
        Instance = this;

        //indexa todos os painéis pelo nome normalizado (filhos inativos também)
        foreach (Transform painel in raizZooms)
            paineis[ClickerManager.Nome(painel)] = painel.gameObject;
    }

    private void Start()
    {
        botaoVoltar.SetActive(false);
    }

    public GameObject Buscar(string caminho)
    {
        paineis.TryGetValue(caminho.Replace('.', '_'), out GameObject painel);
        return painel;
    }

    public void Abrir(GameObject painel)
    {
        pilha.Push(painel);
        painel.transform.SetAsLastSibling(); //desenha por cima do zoom anterior
        painel.SetActive(true);
        botaoVoltar.SetActive(true);
        painelSetas.SetActive(false);
    }

    //ligue no OnClick do BotaoVoltar
    public void Voltar()
    {
        if (!Aberto || DialogueManager.Instance.Aberto)
            return;

        pilha.Pop().SetActive(false); //o zoom anterior continua ativo por baixo
        botaoVoltar.SetActive(Aberto);
        painelSetas.SetActive(!Aberto);
    }
}