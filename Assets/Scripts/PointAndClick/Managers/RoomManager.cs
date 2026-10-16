using System.Collections.Generic;
using UnityEngine;

//Direcionamento "360" para cada sala
public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] private List<GameObject> salas = new List<GameObject>(); //lista todas as salas do jogo
    public IReadOnlyList<GameObject> SALAS => salas; //acesso as salas sem alterar elas

    //parede que o jogador está observando (mantida ao trocar de sala)
    private int paredeAtual = Norte;
    private int salaAtual;

    //mesmo padrão de paredes para todas as salas (sem teto)
    private const int Norte = 0, Oeste = 1, Sul = 2, Leste = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //desativa todas as salas ao iniciar
        for (int i = 0; i < salas.Count; i++)
        {
            salas[i].SetActive(false);
        }

        //primeira sala começa olhando para o norte
        if (salas.Count > 0)
        {
            paredeAtual = Norte;
            salaAtual = 0;
            salas[0].SetActive(true);
            GetChild(paredeAtual);
        }
    }

    //mostra sala pelo indice, mantendo a parede que o jogador estava olhando
    public void MostrarSala(int indice)
    {
        if (indice < 0 || indice >= salas.Count)
            return;

        salas[salaAtual].SetActive(false);
        salaAtual = indice;
        salas[salaAtual].SetActive(true);

        GetChild(paredeAtual); //mesma parede da sala anterior
    }

    public void BotaoEsquerda()
    {
        paredeAtual = (paredeAtual + 1) % 4;
        GetChild(paredeAtual);
    }

    public void BotaoDireita()
    {
        paredeAtual = (paredeAtual - 1 + 4) % 4;
        GetChild(paredeAtual);
    }

    //ativa só a parede correspondente ao indice
    private void GetChild(int indiceFilho)
    {
        Transform sala = salas[salaAtual].transform;
        for (int i = 0; i < sala.childCount; i++)
        {
            sala.GetChild(i).gameObject.SetActive(i == indiceFilho);
        }
    }
}