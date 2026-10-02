using System.Collections.Generic;
using System.Collections;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("DIALOGO UI")]
    [SerializeField] private GameObject painelDialogos;
    [SerializeField] private TextMeshProUGUI textoDialogos;

    private Story historiaAtual;
    private bool dialogoTocando;

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("mais de um manager");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogoTocando = false;
        painelDialogos.SetActive(false);
    }

    private void Update()
    {
        if (!dialogoTocando)
        { 
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ContinuarHistoria();
        }
    }

    public void AbrirDialogoModo(TextAsset inkJSON)
    {
        historiaAtual = new Story(inkJSON.text);
        dialogoTocando = true;
        painelDialogos.SetActive(true);

        ContinuarHistoria();
    }

    private void SairDialogoModo()
    {
        dialogoTocando = false;
        painelDialogos.SetActive(false);
        textoDialogos.text = "";

    }

    public void ContinuarHistoria()
    {
        if (historiaAtual.canContinue)
        {
            textoDialogos.text = historiaAtual.Continue();
        }

        else
        {
            SairDialogoModo();
        }
    }
}