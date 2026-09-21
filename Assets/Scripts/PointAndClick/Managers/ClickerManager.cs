using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable] public class ItemClicavel
{
    //criacao pra inspecao
    public GameObject prefabInspecao;
    public GameObject item;
    public string nomeItem;
    public bool inspecionavel;
}
//sistema de objs clicaveis
public class ClickerManager : MonoBehaviour

{
    //Instance = lógica de singleton -> facilita chamada com ClickerManager.Instance pra acessar func
    public static ClickerManager Instance {get; private set;}

    //cria lista de itens
    [SerializeField]
    private List<ItemClicavel> itensClicaveis = new List<ItemClicavel>();


    private void Awake()
    {
        //impede 2+ ClickerManager de serem ativos se jogador clicar muitas vezes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //Checa todos os objetos da lista
        //item trocado por dados + ItemClicavel = gameobj
        foreach (ItemClicavel dados in itensClicaveis)
        {
            RegistrarItem(dados);
        }
    }

     
    private void RegistrarItem(ItemClicavel dados)
    {
        //se o obj não é registrado não é citado na func
        if (dados == null || dados.item == null)
            return;

        EventTrigger trigger = dados.item.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = dados.item.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        entry.callback.AddListener((eventData) => SeClicou(dados));

        trigger.triggers.Add(entry);
    }

    private void SeClicou(ItemClicavel dados)
    {
        if (dados.inspecionavel)
        {
            if (InspecaoManager.Instance != null)
            {
                InspecaoManager.Instance.Abrir(dados.prefabInspecao);
            }
            else
            {
                Debug.LogWarning("item inspecionável sem InspectionManager");
            }
        }
        else
        {
            Debug.Log($"Cliquei no: {dados.nomeItem}");
        }
    }

    //deixa adicionar obj no meio do jogo
    public void AdicionarItem(ItemClicavel dados)
    {
        if (dados == null || dados.item == null)
            return;
        
        if (!itensClicaveis.Contains(dados))
        
        itensClicaveis.Add(dados);
        RegistrarItem(dados);
        
    }
}

