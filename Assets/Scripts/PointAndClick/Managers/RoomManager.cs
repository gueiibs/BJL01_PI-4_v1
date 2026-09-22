using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

//Direcionamento "360" para cada sala
public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject paredeNorte, paredeOeste, paredeSul, paredeLeste, paredeTeto;

    [SerializeField] GameObject setaEsquerda, setaDireita, setaCima, setaBaixo;

    //paredesLaterais guarda a info das paredes
    private GameObject[] paredesLaterais;
    //localAtual vê qual parede tá sendo observada pelo Jogador
    private int localAtual;

    //localAtual entende que o primeiro é paredeNorte *rever pra salvar 0 como ultimo visto pelo jogador* 
    void Start()
    {
        paredesLaterais = new GameObject[] { paredeNorte, paredeOeste, paredeSul, paredeLeste };
        localAtual = 0;
        ChecarParedeAtual();
    }

    //A partir do indice criado, usa botaoesqueda + 1 no indice (o% 1% 2% 3% 4%)
    public void BotaoEsquerda()
    {
        localAtual = (localAtual + 1) % paredesLaterais.Length;
        ChecarParedeAtual();
    }

    //O da direita diminui o indice
    public void BotaoDireita()
    {
        localAtual = (localAtual - 1 + paredesLaterais.Length) % paredesLaterais.Length;
        ChecarParedeAtual();
    }

    //se o jogador olha pro teto, todas as setas, menos a de baixo são desativadas
    public void BotaoCima()
    {
        SairParedes();
        paredeTeto.SetActive(true);

        setaEsquerda.SetActive(false);
        setaDireita.SetActive(false);
        setaCima.SetActive(false);
        setaBaixo.SetActive(true);
    }

    //se o jogador olha pra baixo, desativa a seta de baixo e reativa as outras
    public void BotaoBaixo()
    {
        ChecarParedeAtual();

        setaEsquerda.SetActive(true);
        setaDireita.SetActive(true);
        setaCima.SetActive(true);
        setaBaixo.SetActive(false);
    }

    //Ativa o SairParedes() e liga a parede atual de acordo com o indice calculado (%)
    void ChecarParedeAtual()
    {
        SairParedes();
        paredesLaterais[localAtual].SetActive(true);
    }

    //desativa as paredes
    void SairParedes()
    {
        paredeNorte.SetActive(false);
        paredeOeste.SetActive(false);
        paredeSul.SetActive(false);
        paredeLeste.SetActive(false);
        paredeTeto.SetActive(false);
    }
}
