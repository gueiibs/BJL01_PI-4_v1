using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoHover : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private float duracao = 0.1f;
    [SerializeField] private float forca = 6f; //quanto balança (em graus)
    [SerializeField] private int vibracao = 15; //quanto oscila

    private Vector3 rotacaoOriginal;

    private void Awake() => rotacaoOriginal = transform.localEulerAngles;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill(true); //termina a vibração anterior e volta 
        transform.DOShakeRotation(duracao, new Vector3(0, 0, forca), vibracao, 90f, true)
                 .SetUpdate(true); //funciona com timeScale 0
    }

    //não muda a loc do button
    private void OnDisable()
    {
        transform.DOKill();
        transform.localEulerAngles = rotacaoOriginal;
    }
}