using System;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class Instrucoes : MonoBehaviour
{
    public enum TipoBotao { Pular, Passo1, Passo2, Passo3, Passo4 }

    [Serializable]
    public class Passo
    {
        public string estado; //nome do estado no Animator (Animation1...)
        public TipoBotao botao; //botão que aparece neste passo
    }

    [SerializeField] private Animator animator;
    [SerializeField] private Passo[] passos;
    [SerializeField] private GameObject painelInstrucoes;

    [Header("BOTÕES")]
    [SerializeField] private GameObject botaoPular;
    [SerializeField] private GameObject botaoPasso1;
    [SerializeField] private GameObject botaoPasso2;
    [SerializeField] private GameObject botaoPasso3;
    [SerializeField] private GameObject botaoPasso4;

    private int atual;
    private Coroutine esperando;

    private void Update()
    {
        //espaço so funciona pra pular se o botão Pular estiver na tela
        if (botaoPular.activeSelf && Keyboard.current.spaceKey.wasPressedThisFrame)
            Proximo();
    }
    private void OnEnable()
    {
        atual = 0;
        Tocar();
    }

    private void OnDisable()
    {
        esperando = null; //a corrotina para sozinha ao desativar o objeto
    }

    private void Tocar()
    {
        animator.Play(passos[atual].estado, 0, 0f);

        //desliga todos: o botão só volta quando a animação acabar
        DesligarBotoes();

        if (esperando != null)
            StopCoroutine(esperando);
        esperando = StartCoroutine(EsperarFimAnimacao());
    }

    private IEnumerator EsperarFimAnimacao()
    {
        yield return null; //espera 1 frame para o Animator entrar no estado novo

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        esperando = null;
        MostrarBotao(passos[atual].botao);
    }

    private void DesligarBotoes()
    {
        botaoPular.SetActive(false);
        botaoPasso1.SetActive(false);
        botaoPasso2.SetActive(false);
        botaoPasso3.SetActive(false);
        botaoPasso4.SetActive(false);
    }

    private void MostrarBotao(TipoBotao tipo)
    {
        switch (tipo)
        {
            case TipoBotao.Pular: botaoPular.SetActive(true); 
                break;
            case TipoBotao.Passo1: botaoPasso1.SetActive(true); 
                break;
            case TipoBotao.Passo2: botaoPasso2.SetActive(true); 
                break;
            case TipoBotao.Passo3: botaoPasso3.SetActive(true); 
                break;
            case TipoBotao.Passo4: botaoPasso4.SetActive(true); 
                break;
        }
    }

    //ligue no OnClick de TODOS os botões
    public void Proximo()
    {
        atual++;

        if (atual >= passos.Length)
            painelInstrucoes.SetActive(false);
        else
            Tocar();
    }
}