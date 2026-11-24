using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]

public class SaveData
{
    public string ink;                                      //estado do Ink
    public List<string> inventario = new List<string>();    //nomes dos assets em Resources/Itens
    public List<string> escondidos = new List<string>();    //caminhos dos objetos já coletados
    public int sala;
    public int parede;

}

public class SaveManager : MonoBehaviour
{
    [Header("VOLTAR AO MENU")]
    [SerializeField] private string cenaMenu = "00_MenuInicial"; 

    [SerializeField] private GameObject iconeSalvando;
    [SerializeField] private float duracaoIcone;
    public static SaveManager Instance { get; private set; }

    //defina true no menu ao clicar em "Continuar"
    public static bool CarregarAoIniciar;

    [SerializeField] private float atrasoSave = 5f;

    public readonly HashSet<string> escondidos = new HashSet<string>();
    private string arquivo;

    private void Awake()
    {
        iconeSalvando.SetActive(false);
        Instance = this;
        arquivo = Path.Combine(Application.persistentDataPath, "save.json");
    }

    private IEnumerator Start()
    {
        yield return null; //espera os outros managers terminarem o Start
        if (CarregarAoIniciar)
            Carregar();
    }

    //chame depois de pegar um item; reinicia o contador se já havia um agendado
    public void AgendarSalvar()
    {
        CancelInvoke(nameof(Salvar));
        Invoke(nameof(Salvar), atrasoSave);
    }
    public void VoltarAoMenu()
    {
        Time.timeScale = 1f; //garante que o menu não carregue pausado
        SceneManager.LoadScene(cenaMenu);
    }
    private void Salvar()
    {
        //não salva no meio de uma fala: tenta de novo em 1s
        if (DialogueManager.Instance.Aberto)
        {
            Invoke(nameof(Salvar), 1f);
                return;
        }

        SaveData dados = new SaveData
        {
            ink = DialogueManager.Instance.EstadoInk,
            sala = RoomManager.Instance.SalaAtual,
            parede = RoomManager.Instance.ParedeAtual,
            escondidos = new List<string>(escondidos)
        };

        foreach (ItemData item in Inventario.Instance.Itens)
            dados.inventario.Add(item.name);

        File.WriteAllText(arquivo, JsonUtility.ToJson(dados));

        iconeSalvando.SetActive(true);
        Invoke(nameof(EsconderIcone), duracaoIcone);
    }

    private void EsconderIcone() => iconeSalvando.SetActive(false);
    private void Carregar()
    {
        if (!File.Exists(arquivo))
            return;

        SaveData dados = JsonUtility.FromJson<SaveData>(File.ReadAllText(arquivo));

        DialogueManager.Instance.CarregarEstado(dados.ink);

        foreach (string nome in dados.inventario)
        {
            ItemData item = Resources.Load<ItemData>("Itens/" + nome);
            if (item != null)
                Inventario.Instance.Adicionar(item);
        }

        foreach (string caminho in dados.escondidos)
        {
            escondidos.Add(caminho);
            GameObject obj = ClickerManager.Item(caminho);
            if (obj != null)
                obj.SetActive(false);
        }

        RoomManager.Instance.IrPara(dados.sala, dados.parede);
    }
}
