using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Direcionamento "360" para cada sala
/// <summary>
/// Novas modificações 29.09 para utilizar lógica de indices como padrão em todas as salas
/// </summary>
public class RoomManager : MonoBehaviour
{
   //Instance = mesma lógica do ClickerManager.cs
    public static RoomManager Instance { get; private set; }

    [SerializeField] private List<GameObject> salas = new List<GameObject>(); //lista todas as salas do jogo
    public IReadOnlyList<GameObject> Salas => salas; //acesso as salas sem alterar elas

    [Header("SETAS DAS SALAS")]

    [SerializeField] Button setaEsquerda, setaDireita;

    //localAtual vê qual parede tá sendo observada pelo Jogador
    //localAtual trocado por paredeAtual
    private int paredeAtual;
    private int salaAtual;

    //paredesLaterais guarda a info das paredes 
    //paredesLaterais trocada por const com tipos, mantendo um padrao p/ todas as salas 
    private const int Norte = 0, Oeste = 1, Sul = 2, Leste = 3;

    private bool dialogoAberto;

    //localAtual entende que o primeiro é paredeNorte
    private void Awake()
    {
        //checa se ja existe outro RoomManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //coloca o RoomManager como main Instance 
        Instance = this;
    }

    private void Start()
    {
        //desativa todas as salas ao iniciar
        for (int i = 0; i < salas.Count; i++)
        {
            salas[i].SetActive(false);
        }

        //se existir uma sala em primeira ela aparece
        if (salas.Count > 0)
        {
            salas[0].SetActive(true);
            MostrarSala(0);
        }

    }

    //mostra sala pelo indice
    public void MostrarSala(int indice)
    {
        if (indice < 0 || indice >= salas.Count) //se n existe n aparece
            return;

        salas[salaAtual].SetActive(false); //desativa ultima mostrada
        salaAtual = indice; //atualiza indice
        salas[salaAtual].SetActive(true); //ativa a proxima

        paredeAtual = Norte; //reseta sempre pro norte

        GetChild(paredeAtual); //mostra a correspondente
        AtualizarSetas(); //atualiza quais setas tem que aparecer
    }

    //A partir do indice criado, usa botaoesqueda + 1 no indice (o% 1% 2% 3% 4%)
    public void BotaoEsquerda()
    {
        paredeAtual = (paredeAtual + 1) % 4; 
        GetChild(paredeAtual);//nova parede
    }

    //O da direita diminui o indice
    public void BotaoDireita()
    {
        paredeAtual = (paredeAtual - 1 + 4) % 4;
        GetChild(paredeAtual);
    }

    //só ativa o que corresponde no indice
    private void GetChild(int indiceFilho)
    {
        Transform sala = salas[salaAtual].transform; //pega sala atual e passa pelos filhos dela
        for (int i = 0; i < sala.childCount; i++)
        {
            sala.GetChild(i).gameObject.SetActive(i == indiceFilho);
        }
    }

    private void OnEnable()
    {
        DialogueManager.OnEstadoDialogoMudou += AoMudarEstadoDialogo;
    }

    private void OnDisable()
    {
        DialogueManager.OnEstadoDialogoMudou -= AoMudarEstadoDialogo;
    }

    private void AoMudarEstadoDialogo(bool aberto)
    {
        dialogoAberto = aberto;
        AtualizarSetas();
    }

    //desativa as paredes
    private void AtualizarSetas()
    {
        if (setaEsquerda != null)
        {
            setaEsquerda.interactable = !dialogoAberto;
        }
        if (setaDireita != null)
        { 
            setaDireita.interactable = !dialogoAberto;
        }
    }

}
