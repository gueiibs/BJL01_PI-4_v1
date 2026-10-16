using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Ink.UnityIntegration;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private InkFile arquivoInk; //arquivo Ink unico do jogo

    [Header("UI")]
    [SerializeField] private GameObject painelDialogos;
    [SerializeField] private TextMeshProUGUI textoDialogos;
    [SerializeField] private GameObject painelEscolhas;
    [SerializeField] private Button botaoEscolhaPrefab;
    [SerializeField] private GameObject painelSetas; //some enquanto o dialogo ta aberto

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
    }

    private void Update()
    {

        //avança com espaço so se o dialogo ta aberto e sem escolhas pendentes
        if (painelDialogos.activeSelf && story.currentChoices.Count == 0 && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Continuar();
        }
    }

    public bool ExisteCaminho(string caminho)
    {
        return story.ContentAtPath(new Ink.Runtime.Path(caminho)).correctObj != null;
    }


    public void IniciarDialogo(string caminho)
    {
        if (painelDialogos.activeSelf)
            return; //ignora cliques enquanto ja tem um dialogo aberto

        story.ChoosePathString(caminho);
        painelDialogos.SetActive(true);
        Continuar();
    }

    public void Continuar()
    {
        if (!story.canContinue)
        {
            Sair();
                return;
        }

        textoDialogos.text = story.Continue();
        ProcessarTags();
        MostrarEscolhas();
    }
    private void ProcessarTags()
    {
        foreach (string tag in story.currentTags)
        {
            //Debug.Log($"Tag recebida sala '{tag}'");
            if (tag.StartsWith("sala:") && int.TryParse(tag.Substring(5).Trim(), out int indice))
            {
                //Debug.Log($"Indo para a sala {indice}");
                RoomManager.Instance.MostrarSala(indice);
            }
        }
    }

    private void Sair()
    {
        painelDialogos.SetActive(false);
        painelEscolhas.SetActive(false);
        painelSetas.SetActive(!ZoomManager.Instance.Aberto);
    }

    private void MostrarEscolhas()
    {
        foreach (Transform filho in painelEscolhas.transform)
            Destroy(filho.gameObject);

        painelEscolhas.SetActive(story.currentChoices.Count > 0);

        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            Button botao = Instantiate(botaoEscolhaPrefab, painelEscolhas.transform);
            botao.GetComponentInChildren<TextMeshProUGUI>().text = story.currentChoices[i].text;

            int indice = i; //copy local por causa do closure
            botao.onClick.AddListener(() => {story.ChooseChoiceIndex(indice);Continuar();});
        }
    }
}