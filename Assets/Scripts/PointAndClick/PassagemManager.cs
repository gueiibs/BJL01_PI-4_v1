using UnityEngine;
using UnityEngine.EventSystems;

//coloque no gameobj manager no item (porta corredor) = defina o índice da sala destino
public class PassagemManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int salaDestino;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (DialogueManager.Instance.Aberto)
            return;

        RoomManager.Instance.MostrarSala(salaDestino);
    }
}