using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dica Visual")]
    [SerializeField] private GameObject dicaVisual;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerNaRange;
    private void Awake()
    {
        playerNaRange = false;
        dicaVisual.SetActive(false);
    }

    private void Update()
    {
        if (playerNaRange)
        {
            dicaVisual.SetActive(true);
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                DialogueManager.GetInstance().AbrirDialogoModo(inkJSON);
            }

            else
            {
                dicaVisual.SetActive(false);
            }
        }
    }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                playerNaRange = true;
            }
        }
    
}
