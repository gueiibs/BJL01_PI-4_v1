using UnityEngine;
using UnityEngine.EventSystems;

public class InspecaoFuncao : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    [Header("Rotação")]
    [SerializeField] private float sensibilidade; //definição do quanto o objeto gira ao arrastar o mouse

    [Header("Zoom")]
    [SerializeField] private float zoomSensibilidade;//definição da velo do zoom
    [SerializeField] private float zoomMinimo;//maior fov que pode ter
    [SerializeField] private float zoomMaximo;//menor

    //só é chamando quando o jogador inicia o arrastar do mouse (segurando mouse1)
    public void OnBeginDrag(PointerEventData eventData) { }

    //se arrastou, chama ele
    public void OnDrag(PointerEventData eventData)
    {
        //verefica se tem obj em inspeção
        if (InspecaoManager.Instance == null || InspecaoManager.Instance.CurrentObject == null) 
            return;

        Transform alvo = InspecaoManager.Instance.CurrentObject.transform; //pega o obj em inspeção

        float rotacaoY = -eventData.delta.x * sensibilidade;//calcula rot horizontal com o mouse
        float rotacaoX = eventData.delta.y * sensibilidade;//na veretical

        alvo.Rotate(Vector3.up, rotacaoY, Space.World);//faz o obj girar em Y
        alvo.Rotate(Vector3.right, rotacaoX, Space.World);//idem no X
    }

    //entende quando o jogador para de arrastar/segurar mouse (button mouse1)
    public void OnEndDrag(PointerEventData eventData) { }

    //faz o scroll detectar
    public void OnScroll(PointerEventData eventData)
    {
        //checa a camera de inspeção pra não ter conflito
        if (InspecaoManager.Instance == null || InspecaoManager.Instance.CameraAtual == null) 
            return;

        //ativa a camera de inspeção
        Camera camera = InspecaoManager.Instance.CameraAtual;

        //altera o fov de acordo com as limitações
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - eventData.scrollDelta.y * zoomSensibilidade, zoomMinimo, zoomMaximo);
    }
}