using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickerManager : MonoBehaviour
{
    private void Start()
    {
        //salas > paredes > itens
        foreach (GameObject sala in RoomManager.Instance.SALAS)
            foreach (Transform parede in sala.transform)
                foreach (Transform item in parede)
                    Registrar(item, $"{Nome(sala.transform)}.{Nome(parede)}_{Nome(item)}");

        //zooms > itens (o painel de zoom faz o papel de sala)
        foreach (Transform zoom in ZoomManager.Instance.Raiz)
            foreach (Transform item in zoom)
                Registrar(item, $"{Nome(zoom)}.{Nome(item)}");
    }

    private void Registrar(Transform item, string caminho)
    {
        item.gameObject.AddComponent<ItemClicavel>().caminho = caminho;

        if (ZoomManager.Instance.Buscar(caminho) == null
            && PrefabInspecao(caminho) == null
            && !DialogueManager.Instance.ExisteCaminho(caminho))
            Debug.LogWarning($"Sem zoom, inspeção nem texto no Ink: {caminho}");
    }

    //minúsculo, qualquer símbolo ou espaço vira "_"
    public static string Nome(Transform t) =>
        Regex.Replace(t.name.ToLower(), "[^a-z0-9]+", "_").Trim('_');

    public static GameObject PrefabInspecao(string caminho) =>
        Resources.Load<GameObject>("Inspecao/" + caminho.Replace('.', '_'));
}

public class ItemClicavel : MonoBehaviour, IPointerClickHandler
{
    public string caminho;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (DialogueManager.Instance.Aberto)
            return;

        GameObject zoom = ZoomManager.Instance.Buscar(caminho);
        if (zoom != null)
        {
            ZoomManager.Instance.Abrir(zoom);
            return;
        }

        GameObject prefab = PrefabInspecao(caminho);
        if (prefab != null)
            InspecaoManager.Instance.Abrir(prefab);
        else
            DialogueManager.Instance.IniciarDialogo(caminho);
    }

    private static GameObject PrefabInspecao(string caminho) => ClickerManager.PrefabInspecao(caminho);
}