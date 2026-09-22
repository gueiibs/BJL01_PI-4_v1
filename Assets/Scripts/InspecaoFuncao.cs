using UnityEngine;
using UnityEngine.EventSystems;

public class InspecaoFuncao : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    [Header("Rotação")]
    [SerializeField] private float sensibilidade;

    [Header("Zoom")]
    [SerializeField] private float zoomSensibilidade;
    [SerializeField] private float zoomMinimo;
    [SerializeField] private float zoomMaximo;

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        if (InspecaoManager.Instance == null || InspecaoManager.Instance.CurrentObject == null) return;

        Transform alvo = InspecaoManager.Instance.CurrentObject.transform;

        float rotacaoY = -eventData.delta.x * sensibilidade;
        float rotacaoX = eventData.delta.y * sensibilidade;

        alvo.Rotate(Vector3.up, rotacaoY, Space.World);
        alvo.Rotate(Vector3.right, rotacaoX, Space.World);
    }

    public void OnEndDrag(PointerEventData eventData) { }

    public void OnScroll(PointerEventData eventData)
    {
        if (InspecaoManager.Instance == null || InspecaoManager.Instance.CameraAtual == null) return;

        Camera camera = InspecaoManager.Instance.CameraAtual;

        // scrollDelta.y positivo = rodou pra cima = zoom in (FOV menor)
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - eventData.scrollDelta.y * zoomSensibilidade, zoomMinimo, zoomMaximo);
    }
}