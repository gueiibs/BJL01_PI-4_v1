using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    public static Inventario Instance { get; private set; }

    [SerializeField] private GameObject painel;   //painel em tela cheia
    [SerializeField] private Button[] slots;      //slots fixos, na ordem de preenchimento

    private readonly List<ItemData> itens = new List<ItemData>();

    private void Awake()
    {
        Instance = this;

        foreach (Button slot in slots)
            Icone(slot).enabled = false; //slot vazio
    }

    private void Start()
    {
        painel.SetActive(false);
    }

    //ligue nos botões de abrir e fechar
    public void Alternar()
    {
        if (DialogueManager.Instance.Aberto)
            return;

        painel.SetActive(!painel.activeSelf);
    }

    //o nome vem do arquivo do asset (o mesmo usado em "# pegar:" e no Ink)
    public bool Tem(string nome) => itens.Exists(i => i.name == nome);

    //false só se não houver slot livre
    public bool Adicionar(ItemData item)
    {
        if (Tem(item.name))
            return true;

        if (itens.Count >= slots.Length)
        {
            Debug.LogWarning("Inventário cheio");
            return false;
        }

        Button slot = slots[itens.Count];
        itens.Add(item);

        Icone(slot).sprite = item.icone;
        Icone(slot).enabled = true;
        slot.onClick.AddListener(() => InspecaoManager.Instance.Abrir(item.prefabInspecao));
        return true;
    }

    //a imagem do item é o primeiro filho do slot
    private static Image Icone(Button slot) => slot.transform.GetChild(0).GetComponent<Image>();
}