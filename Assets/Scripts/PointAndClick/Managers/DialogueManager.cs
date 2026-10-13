using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("DIALOGO UI")]
    [SerializeField] private GameObject painelDialogos;
    [SerializeField] private TextMeshProUGUI textoDialogos;

    [Header("ESCOLHAS")]
    [SerializeField] private GameObject painelEscolhas;
    [SerializeField] private Button botaoEscolhaPrefab;

    [System.Serializable]
    public class PainelPorTag
    {
        public string tag;
        public GameObject painel;
    }

    [Header("Painéis ativados por tag do Ink")]
    [SerializeField] private List<PainelPorTag> paineisPorTag = new List<PainelPorTag>();

    private readonly List<GameObject> botoesEscolhaInstanciados = new List<GameObject>();

    private Story historiaAtual;
    private bool dialogoTocando;

    private static DialogueManager instance;

    public static bool PainelDialogoAberto => instance != null && instance.dialogoTocando;
    public static event System.Action<bool> OnEstadoDialogoMudou;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("mais de um manager");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogoTocando = false;
        painelDialogos.SetActive(false);
        painelEscolhas.SetActive(false);
    }

    private void Update()
    {
        if (!dialogoTocando)
            return;

        //só avança por espaço se NÃO houver escolhas esperando resposta
        if (historiaAtual.currentChoices.Count == 0 && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ContinuarHistoria();
        }
    }

    public void AbrirDialogoModo(TextAsset inkJSON)
    {
        historiaAtual = new Story(inkJSON.text);
        dialogoTocando = true;
        painelDialogos.SetActive(true);
        OnEstadoDialogoMudou?.Invoke(true);

        ContinuarHistoria();
    }

    private void SairDialogoModo()
    {
        dialogoTocando = false;
        painelDialogos.SetActive(false);
        textoDialogos.text = "";
        LimparEscolhas();
        OnEstadoDialogoMudou?.Invoke(false);
    }

    public void ContinuarHistoria()
    {
        if (historiaAtual.canContinue)
        {
            textoDialogos.text = historiaAtual.Continue();
            ProcessarTags(historiaAtual.currentTags);
            MostrarEscolhasSeHouver();
        }
        else if (historiaAtual.currentChoices.Count > 0)
        {
            MostrarEscolhasSeHouver();
        }
        else
        {
            SairDialogoModo();
        }
    }

    private void ProcessarTags(List<string> tags)
    {
        foreach (string tagCompleta in tags)
        {
            string[] partes = tagCompleta.Split(':');
            if (partes.Length < 2)
                continue;

            string chave = partes[0].Trim();
            string valor = partes[1].Trim();

            if (chave == "painel")
            {
                AtivarPainel(valor);
            }
        }
    }

    private void AtivarPainel(string nomeTag)
    {
        foreach (PainelPorTag item in paineisPorTag)
        {
            if (item.tag == nomeTag && item.painel != null)
            {
                item.painel.SetActive(true);
            }
        }
    }

    private void MostrarEscolhasSeHouver()
    {
        LimparEscolhas();

        if (historiaAtual.currentChoices.Count == 0)
        {
            painelEscolhas.SetActive(false);
            return;
        }

        painelEscolhas.SetActive(true);

        for (int i = 0; i < historiaAtual.currentChoices.Count; i++)
        {
            Choice escolha = historiaAtual.currentChoices[i];
            Button botao = Instantiate(botaoEscolhaPrefab, painelEscolhas.transform);

            TextMeshProUGUI texto = botao.GetComponentInChildren<TextMeshProUGUI>();
            if (texto != null)
                texto.text = escolha.text;

            int indice = i; //cópia local, evita bug de closure
            botao.onClick.AddListener(() => EscolherOpcao(indice));

            botoesEscolhaInstanciados.Add(botao.gameObject);
        }
    }

    private void EscolherOpcao(int indice)
    {
        historiaAtual.ChooseChoiceIndex(indice);
        LimparEscolhas();
        ContinuarHistoria();
    }

    private void LimparEscolhas()
    {
        foreach (GameObject botao in botoesEscolhaInstanciados)
        {
            Destroy(botao);
        }
        botoesEscolhaInstanciados.Clear();
        painelEscolhas.SetActive(false);
    }
}