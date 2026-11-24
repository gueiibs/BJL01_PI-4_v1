using System;
using System.Collections;
using Ink.Runtime;
using Ink.UnityIntegration;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //tipos de tag do Ink (# tipo: valor). Para criar uma ação nova: valor aqui + case no switch
    private enum TagTipo { Sala, Pegar, Zoom, Mapa, Digitar }

    public static DialogueManager Instance { get; private set; }

    [SerializeField] private InkFile arquivoInk; //arquivo Ink main com variaveis do jogo

    [Header("DIGITAÇÃO")]
    [SerializeField] private float caracteresPorSegundo = 40f;

    [Header("NOTIFICAÇÃO")]
    [SerializeField] private GameObject painelNotificacao;
    [SerializeField] private TextMeshProUGUI textoNotificacao;
    [SerializeField] private float duracaoNotificacao = 2.5f;

    [Header("UI")]
    [SerializeField] private GameObject painelDialogos;
    [SerializeField] private TextMeshProUGUI textoDialogos;
    [SerializeField] private GameObject painelEscolhas;
    [SerializeField] private Button botaoEscolhaPrefab;
    [SerializeField] private GameObject botaoContinuar;
    [SerializeField] private GameObject painelSetas; //some enquanto o dialogo ta aberto
    [SerializeField] private GameObject botaoMapa;

    [Header("INPUT")]
    [SerializeField] private TMP_InputField input;

    private Story story;
    private Coroutine digitando;
    private string caminhoAtual;
    private ItemData itemPendente;
    private bool esperandoSenha;

    public bool Aberto => painelDialogos.activeSelf;

    //estado do Ink para o save
    public string EstadoInk => story.state.ToJson();

    public void CarregarEstado(string json)
    {
        story.state.LoadJson(json);
        botaoMapa.SetActive((bool)story.variablesState["tem_mapanormal"]);
    }

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
        if (Aberto && !esperandoSenha && story.currentChoices.Count == 0
            && Keyboard.current.spaceKey.wasPressedThisFrame)
            Continuar();
    }

    public bool ExisteCaminho(string caminho)
    {
        try { return story.ContentAtPath(new Ink.Runtime.Path(caminho)).correctObj != null; }
        catch { return false; }
    }

    public void IniciarDialogo(string caminho)
    {
        if (Aberto || !ExisteCaminho(caminho))
            return;

        caminhoAtual = caminho;
        story.ChoosePathString(caminho);
        painelDialogos.SetActive(true);
        painelSetas.SetActive(false);
        Continuar();
    }

    public void Continuar()
    {
        //ainda digitando: mostra tudo de uma vez
        if (digitando != null)
        {
            CompletarTexto();
            return;
        }

        if (!story.canContinue)
        {
            Sair();
            return;
        }

        string texto = story.Continue().Trim();
        ProcessarTags();

        if (texto == "")
        {
            Continuar(); //pula as linhas sem texto depois de processar a tag
            return;
        }

        textoDialogos.text = texto;
        digitando = StartCoroutine(Digitar());
    }

    //le tags -> "tipo: valor", converte o tipo para enum e executa a função
    private void ProcessarTags()
    {
        foreach (string tag in story.currentTags)
        {
            string[] partes = tag.Split(new[] { ':' }, 2);
            string valor = partes.Length > 1 ? partes[1].Trim() : "";

            if (!Enum.TryParse(partes[0].Trim(), true, out TagTipo tipo))
                continue; //tag desconhecida é ignorada

            switch (tipo)
            {
                case TagTipo.Sala:
                    if (int.TryParse(valor, out int indice))
                        RoomManager.Instance.MostrarSala(indice);
                    break;

                case TagTipo.Pegar:
                    Pegar(valor);
                    break;

                case TagTipo.Zoom:
                    AbrirZoom(valor);
                    break;

                case TagTipo.Mapa:
                    botaoMapa.SetActive(true);
                    EsconderItemAtual();
                    SaveManager.Instance.AgendarSalvar();
                    break;

                case TagTipo.Digitar:
                    esperandoSenha = true;
                    input.text = "";
                    input.ActivateInputField();
                    break;
            }
        }
    }

    private IEnumerator Digitar()
    {
        //escolhas e botao só aparecem quando o texto termina
        painelEscolhas.SetActive(false);
        botaoContinuar.SetActive(false);

        textoDialogos.maxVisibleCharacters = 0;
        textoDialogos.ForceMeshUpdate();

        int total = textoDialogos.textInfo.characterCount;
        WaitForSecondsRealtime espera = new WaitForSecondsRealtime(1f / caracteresPorSegundo);

        for (int i = 1; i <= total; i++)
        {
            textoDialogos.maxVisibleCharacters = i;
            yield return espera;
        }

        digitando = null;
        MostrarEscolhas();
    }

    private void CompletarTexto()
    {
        StopCoroutine(digitando);
        digitando = null;
        textoDialogos.maxVisibleCharacters = int.MaxValue;
        MostrarEscolhas();
    }

    private void Pegar(string nome)
    {
        ItemData item = Resources.Load<ItemData>("Itens/" + nome);

        if (item == null)
        {
            Debug.LogWarning($"Item não encontrado em Resources/Itens: {nome}");
            return;
        }

        if (!Inventario.Instance.Adicionar(item))
            return; //inventário cheio: o objeto continua na cena

        itemPendente = item;
        Notificar(item);
        EsconderItemAtual();
        SaveManager.Instance.AgendarSalvar();
    }

    private void AbrirZoom(string nome)
    {
        GameObject zoom = ZoomManager.Instance.Buscar(nome);

        if (zoom != null)
            ZoomManager.Instance.Abrir(zoom);
        else
            Debug.LogWarning($"Zoom não encontrado: {nome}");
    }

    //esconde o objeto clicado e registra o caminho para o save
    private void EsconderItemAtual()
    {
        SaveManager.Instance.escondidos.Add(caminhoAtual);
        GameObject obj = ClickerManager.Item(caminhoAtual);
        if (obj != null)
            obj.SetActive(false);
    }

    private void Notificar(ItemData item)
    {
        textoNotificacao.text = $"<line-height=120%><size=80%><b>ITEM COLETADO</b></size> \n<i>{item.Nome}</i>";
        painelNotificacao.SetActive(true);
        CancelInvoke(nameof(EsconderNotificacao));
        Invoke(nameof(EsconderNotificacao), duracaoNotificacao);
    }

    private void EsconderNotificacao() => painelNotificacao.SetActive(false);

    public void SeSenhaEstaCorreta()
    {
        story.variablesState["senhacomputador"] = input.text.Trim().Replace(" ", "");
        IniciarDialogo("zoom_computadorsecretaria.verificarsenha");
    }

    private void Sair()
    {
        if (digitando != null)
        {
            StopCoroutine(digitando);
            digitando = null;
        }

        painelDialogos.SetActive(false);
        painelEscolhas.SetActive(false);
        painelSetas.SetActive(!ZoomManager.Instance.Aberto);
        botaoContinuar.SetActive(false);
        esperandoSenha = false;

        if (itemPendente != null)
        {
            InspecaoManager.Instance.Abrir(itemPendente);
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

            int indice = i; //copia local por causa do closure
            botao.onClick.AddListener(() => { story.ChooseChoiceIndex(indice); Continuar(); });
        }
    }
}