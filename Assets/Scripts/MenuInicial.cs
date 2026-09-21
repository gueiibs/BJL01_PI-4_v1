using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuInicial : MonoBehaviour
{
    [Header("Paineis")]
    [SerializeField] GameObject painelMenu;
    [SerializeField] GameObject painelCreditos;
    [SerializeField] GameObject painelConfiguracoes;


    public void BotaoJogar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("");
    }
    public void BotaoCreditos()
    {
        FecharTodosPaineis();
        painelCreditos.SetActive(true);
    }
    public void BotaoConfiguracoes()
    {
        FecharTodosPaineis();
        painelConfiguracoes.SetActive(true);
    }
    public void BotaoVoltarMenu()
    {
        FecharTodosPaineis();
        painelMenu.SetActive(true);
    }
    public void BotaoSair()
    {
        Application.Quit();
    }

    void FecharTodosPaineis()
    {
        painelMenu.SetActive(false);
        painelCreditos.SetActive(false);
        painelConfiguracoes.SetActive(false);
    }
}