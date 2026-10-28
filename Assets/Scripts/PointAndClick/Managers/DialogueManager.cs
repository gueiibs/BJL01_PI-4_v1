using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Ink.UnityIntegration;
using UnityEngine.InputSystem.Utilities;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private InkFile arquivoInk; //arquivo Ink main com variaveis do jogo

    public static DialogueManager Instance { get; private set; }
    [Header("NOTIFICAÇÃO")]
    [SerializeField] private GameObject painelNotificacao;
    [SerializeField] private TextMeshProUGUI textoNotificacao;
    [SerializeField] private float duracaoNotificacao;

    private ItemData itemPendente;

    [Header("UI")]
    [SerializeField] private GameObject painelDialogos;
    [SerializeField] private TextMeshProUGUI textoDialogos;
    [SerializeField] private GameObject painelEscolhas;
    [SerializeField] private Button botaoEscolhaPrefab;
    [SerializeField] private GameObject botaoContinuar;
    //[SerializeField] private Button botaoVoltarZoom; //botao voltar painel/tela anterior ao zoom(mesmo se for "zoom->zoom->zoom")
    [SerializeField] private GameObject painelSetas; //some enquanto o dialogo ta aberto
    [SerializeField] private GameObject botaoMapa;

    [Header("INPUT")]
    [SerializeField] private GameObject painelInput;
    [SerializeField] private TMP_InputField input;

    private bool esperandoSenha;

    public bool Aberto => painelDialogos.activeSelf;



    [System.Serializable] 
    public class PainelTag
    {
        public string tag;
        public GameObject painel;
    }

    private Story story;

    private void Awake()
    {
        Instance = this;
        story = new Story(arquivoInk.storyJson);

        story.BindExternalFunction("TemItem", (string nome) => Inventario.Instance.Tem(nome));
        painelNotificacao.SetActive(false);
    }

    private void Update()
    {
        //avança com espaço so se o dialogo ta aberto e sem escolhas pendentes
        if (painelDialogos.activeSelf && !esperandoSenha && story.currentChoices.Count == 0 && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Continuar();
        }
    }

    public bool ExisteCaminho(string caminho)
    {
        try
        {
            return story.ContentAtPath(new Ink.Runtime.Path(caminho)).correctObj != null;
        }
        catch
        {
            return false;
        }
    }


    private string caminhoAtual;

    public void IniciarDialogo(string caminho)
    {
        if (painelDialogos.activeSelf || !ExisteCaminho(caminho))
            return;

        caminhoAtual = caminho;
        story.ChoosePathString(caminho);
        painelDialogos.SetActive(true);
        painelSetas.SetActive(false);
        //botaoVoltarZoom.SetActi
        Continuar();
    }
    public void Continuar()
    {
        if (!story.canContinue)
        {
            Sair();
                return;
        }

        string texto = story.Continue().Trim();
        ProcessarTags();

        if (texto == "")
        {
            Continuar(); //pula linhas sem texto, mas a tag ja foi processada
                return;
        }

        textoDialogos.text = texto;
        MostrarEscolhas();
    }
    private void ProcessarTags()
    {
        foreach (string tag in story.currentTags)
        {
            if (tag.StartsWith("sala:") && int.TryParse(tag.Substring(5).Trim(), out int indice))
            {
                RoomManager.Instance.MostrarSala(indice);
            }
            else if (tag.StartsWith("pegar:"))
            {
                string nome = tag.Substring(6).Trim();
                ItemData item = Resources.Load<ItemData>("Itens/" + nome);

                if (item == null)
                {
                    Debug.LogWarning($"Item não encontrado em Resources/Itens: {nome}");
                    continue;
                }

                if (!Inventario.Instance.Adicionar(item))
                    continue; //inventáario cheio = o objeto continua na cena

                if (!Inventario.Instance.Adicionar(item))
                    continue; //inventário cheio: o objeto continua na cena

                itemPendente = item;
                Notificar(item);

                GameObject obj = ClickerManager.Item(caminhoAtual); //esconde o objeto clicado
                if (obj != null)
                    obj.SetActive(false);
            }

            else if (tag == "digitar")
            {
                esperandoSenha = true;
                input.text = "";
                input.ActivateInputField();
            }

            else if (tag.StartsWith("zoom:"))
            {
                GameObject zoom = ZoomManager.Instance.Buscar(tag.Substring(5).Trim());
                if (zoom != null)
                    ZoomManager.Instance.Abrir(zoom);
                else
                    Debug.LogWarning($"Zoom naum encontrado: {tag}");
            }

            else if (tag == "mapa")
            {
                botaoMapa.SetActive(true);

                GameObject obj = ClickerManager.Item(caminhoAtual); //esconde o objeto clicado
                if (obj != null)
                    obj.SetActive(false);
            }
        }
    }
    private void Notificar(ItemData item)
    {
        textoNotificacao.text = $"<line-height=120%><size=80%><b>ITEM COLETADO</b></size> \n<i>{item.Nome}</i>";
        painelNotificacao.SetActive(true);
        CancelInvoke(nameof(EsconderNotificacao));
        Invoke(nameof(EsconderNotificacao), duracaoNotificacao);
    }

    public void SeSenhaEstaCorreta()
    {
        story.variablesState["senhacomputador"] = input.text.Trim().Replace(" ", "");
        IniciarDialogo("zoom_computadorsecretaria.verificarsenha");

    }

    private void EsconderNotificacao() => painelNotificacao.SetActive(false);
    private void Sair()
    {
        painelDialogos.SetActive(false);
        painelEscolhas.SetActive(false);
        painelSetas.SetActive(!ZoomManager.Instance.Aberto);

        botaoContinuar.SetActive(false);
        esperandoSenha = false;

        if (itemPendente != null)
        {
            if (itemPendente.prefabInspecao != null)
                InspecaoManager.Instance.Abrir(itemPendente.prefabInspecao);
            itemPendente = null;
        }
    }

    private void MostrarEscolhas()
    {
        foreach (Transform filho in painelEscolhas.transform)
            Destroy(filho.gameObject);

        painelEscolhas.SetActive(story.currentChoices.Count > 0);
        botaoContinuar.SetActive(story.currentChoices.Count == 0);

        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            Button botao = Instantiate(botaoEscolhaPrefab, painelEscolhas.transform);
            botao.GetComponentInChildren<TextMeshProUGUI>().text = story.currentChoices[i].text;

            int indice = i; //copy local por causa do closure
            botao.onClick.AddListener(() => {story.ChooseChoiceIndex(indice);Continuar();});
        }

    }
}