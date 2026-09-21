using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//sistema de objs clicaveis
//*adicionar sistema de inspeção para obj 3Ds*
public class ClickerManager : MonoBehaviour

{
    //Instance = lógica de singleton -> facilita chamada com ClickerManager.Instance pra acessar func
    public static ClickerManager Instance {get; private set;}

    //cria lista de itens
    [SerializeField] 
    private List<GameObject> itensClicaveis = new List<GameObject>();

    
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
        foreach (GameObject item in itensClicaveis)
        {
            RegistrarItem(item);
        }
    }

     
    private void RegistrarItem(GameObject item)
    {
        //se o obj não é registrado não é citado na func
        if (item == null)
            return;

        EventTrigger trigger = item.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = item.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        entry.callback.AddListener((data) => SeClicou(item, (PointerEventData)data));

        trigger.triggers.Add(entry);
    }

    private void SeClicou(GameObject item, PointerEventData data)
    {
        Debug.Log("Cliquei");
    }

    //deixa adicionar obj no meio do jogo
    private void AdicionarItem(GameObject item)
    {
        if 
            (item == null || itensClicaveis.Contains(item)) 
            return;
            itensClicaveis.Add(item);

            RegistrarItem(item);
     }
}

