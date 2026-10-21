using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickerManager : MonoBehaviour
{
    //caminho -> objeto, para o Ink conseguir esconder o item coletado
    private static readonly Dictionary<string, GameObject> itens = new Dictionary<string, GameObject>();

    public static GameObject Item(string caminho)
    {
        itens.TryGetValue(caminho, out GameObject obj);
        return obj;
    }

    private void Start()
    {
        //salas > paredes > itens
        foreach (GameObject sala in RoomManager.Instance.SALAS)
            foreach (Transform parede in sala.transform)
                foreach (Transform item in parede)
                {
                    string caminho = $"{Nome(sala.transform)}.{Nome(parede)}_{Nome(item)}";
                    Registrar(item, caminho);

                    if (!itens.ContainsKey(caminho))
                        Debug.LogWarning($"Sem zoom, inspeção nem texto no Ink: {caminho}");
                }

        //zooms > todos os descendentes (pastas de organização incluídas)
        foreach (Transform zoom in ZoomManager.Instance.Raiz)
            foreach (Transform item in zoom.GetComponentsInChildren<Transform>(true))
                if (item != zoom)
                    Registrar(item, $"{Nome(zoom)}.{Nome(item)}");
    }

    private void Registrar(Transform item, string caminho)
    {
        bool temDestino = ZoomManager.Instance.Buscar(caminho) != null
                          || PrefabInspecao(caminho) != null
                          || DialogueManager.Instance.ExisteCaminho(caminho);

        if (!temDestino)
            return; //pasta, input field e peças de UI ficam de fora

        item.gameObject.AddComponent<ItemClicavel>().caminho = caminho;
        itens[caminho] = item.gameObject;
    }

    public static string Nome(Transform t) => Regex.Replace(t.name.ToLower(), "[^a-z0-9]+", "_").Trim('_');

    public static GameObject PrefabInspecao(string caminho) => Resources.Load<GameObject>("Inspecao/" + caminho.Replace('.', '_'));
}

public class ItemClicavel : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public string caminho;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (DialogueManager.Instance.Aberto)
            return;

        GameObject zoom = ZoomManager.Instance.Buscar(caminho);
        GameObject prefab = ClickerManager.PrefabInspecao(caminho);

        if (zoom != null)
            ZoomManager.Instance.Abrir(zoom);
        else if (prefab != null)
            InspecaoManager.Instance.Abrir(prefab);
        else
            DialogueManager.Instance.IniciarDialogo(caminho);
    }
}