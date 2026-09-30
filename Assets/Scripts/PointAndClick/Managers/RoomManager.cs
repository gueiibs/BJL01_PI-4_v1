using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] GameObject setaEsquerda, setaDireita, setaCima, setaBaixo;

    //localAtual vê qual parede tá sendo observada pelo Jogador
    //localAtual trocado por paredeAtual
    private int paredeAtual;
    private int salaAtual;
    private bool olhandoTeto;
    //paredesLaterais guarda a info das paredes 
    //paredesLaterais trocada por const com tipos, mantendo um padrao p/ todas as salas 
    private const int Norte = 0, Oeste = 1, Sul = 2, Leste = 3, Teto = 4;

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
        olhandoTeto = false;

        GetChild(paredeAtual); //mostra a correspondente
        AtualizarSetas(); //atualiza quais setas tem que aparecer
    }

    //A partir do indice criado, usa botaoesqueda + 1 no indice (o% 1% 2% 3% 4%)
    public void BotaoEsquerda()
    {
        if (olhandoTeto) //nao pode olhar pros lador se olhar pro teto
            return;

        paredeAtual = (paredeAtual + 1) % 4; 
        GetChild(paredeAtual);//nova parede
    }

    //O da direita diminui o indice
    public void BotaoDireita()
    {
        if (olhandoTeto)
            return;

        paredeAtual = (paredeAtual - 1 + 4) % 4;
        GetChild(paredeAtual);
    }

    //se o jogador olha pro teto, todas as setas, menos a de baixo são desativadas
    public void BotaoCima()
    {
        olhandoTeto = true;
        GetChild(Teto);
        AtualizarSetas();
    }

    //se o jogador olha pra baixo, desativa a seta de baixo e reativa as outras
    public void BotaoBaixo()
    {
        olhandoTeto = false;
        GetChild(paredeAtual);
        AtualizarSetas();
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
    //desativa as paredes
    private void AtualizarSetas()
    {
        if (setaEsquerda != null) setaEsquerda.SetActive(!olhandoTeto);
        if (setaDireita != null) setaDireita.SetActive(!olhandoTeto);
        if (setaCima != null) setaCima.SetActive(!olhandoTeto);
        if (setaBaixo != null) setaBaixo.SetActive(olhandoTeto);
    }

}
