using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class MenuInicial : MonoBehaviour
{
    [SerializeField] TMP_Dropdown opcoesTela;

    [Header("PAINEIS")]
    [SerializeField] GameObject painelMenu;
    [SerializeField] GameObject painelCreditos;
    [SerializeField] GameObject painelConfiguracoes;

    [Header("INTRODUCAO PRE JOGAR")]
    [SerializeField] private VideoPlayer video;
    [SerializeField] private GameObject painelIntroducao;

    private const string CenaJogoUm = "01_AndarUm";

    public void BotaoJogar()
    {
        if (painelIntroducao.activeSelf)
                return; //ignora clique duplo

        painelIntroducao.SetActive(true);
        video.loopPointReached += AoTerminarVideo;
        video.Play();
    }

    public void PularVideo()
    {
        video.Stop();
        AoTerminarVideo(video);
    }

    private void AoTerminarVideo(VideoPlayer vp)
    {
        video.loopPointReached -= AoTerminarVideo;
        Time.timeScale = 1f;
        SceneManager.LoadScene(CenaJogoUm);
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

    private void MostrarPainel(GameObject painel)
    {
        painelMenu.SetActive(false);
        painelCreditos.SetActive(false);
        painelConfiguracoes.SetActive(false);

        painel.SetActive(true);
    }
}