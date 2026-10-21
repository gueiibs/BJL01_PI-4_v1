using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuInicial : MonoBehaviour
{
    [SerializeField] TMP_Dropdown opcoesTela;

    [Header("Painéis")]
    [SerializeField] GameObject painelMenu;
    [SerializeField] GameObject painelCreditos;
    [SerializeField] GameObject painelConfiguracoes;

    const string CenaJogo = "01_AndarUm";

    public void BotaoJogar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(CenaJogo);
    }

    public void BotaoCreditos() => MostrarPainel(painelCreditos);
    public void BotaoConfiguracoes() => MostrarPainel(painelConfiguracoes);
    public void BotaoVoltarMenu() => MostrarPainel(painelMenu);
    public void BotaoSair() => Application.Quit();

    public void MudarTela(int modo)
    {
        ModoTela(modo);
        PlayerPrefs.SetInt("ModoTela", modo);
        PlayerPrefs.Save();
    }

    public void ModoTela(int modo)
    {
        switch (modo)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.MaximizedWindow; //---> tela cheia
                break; 
            case 1: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; //---> janela em tela cheia
                break; 
            case 2: Screen.fullScreenMode = FullScreenMode.Windowed; //---> janela
                break;          
        }

    }

    void MostrarPainel(GameObject painel)
    {
        painelMenu.SetActive(false);
        painelCreditos.SetActive(false);
        painelConfiguracoes.SetActive(false);

        painel.SetActive(true);
    }
}