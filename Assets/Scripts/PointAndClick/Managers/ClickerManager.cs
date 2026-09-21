using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickerManager : MonoBehaviour

{
    public static ClickerManager Instance {get; private set;}

    [SerializeField] 
    private List<GameObject> itensClicaveis = new List<GameObject>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (GameObject item in itensClicaveis)
        {
            RegistrarItem(item);
        }
    }

    private void RegistrarItem(GameObject item)
    {
        if (item == null) return;

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

    private void AdicionarItem(GameObject item)
    {
        if 
            (item == null || itensClicaveis.Contains(item)) 
            return;
            itensClicaveis.Add(item);

            RegistrarItem(item);
     }
}




    /*public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Cliquei no: {nomeItem}");
    }



    /*[Header("Paineis")]
    [SerializeField] 
    [SerializeField] 
    [SerializeField] 

    void Start()
    {
        
    }

    void Update()
    {
        
    }*/

