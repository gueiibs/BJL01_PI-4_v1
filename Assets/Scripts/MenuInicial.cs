using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
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

    [Header("FPS & MODOTELA")]
    [SerializeField] private Image imagemFps;
    [SerializeField] private Sprite[] spritesFps;

    [SerializeField] private Image imagemModoTela;
    [SerializeField] private Sprite[] spritesModoTela;

    private int indiceModoTela;
    private int indiceFps;

    private const string CenaJogoUm = "01_AndarUm";

    private void Start()
    {
        // FPS
        indiceFps = LimitadorFPS.IndiceAtual();
        AtualizarFps();

        // Modo de tela
        indiceModoTela = PlayerPrefs.GetInt("ModoTela", 0);
        AtualizarModoTela();
        ModoTela();
    }

    // FPS
    public void ProximoFps() => MudarFps(1);
    public void AnteriorFps() => MudarFps(-1);
    public void MudarFps(int direcao)
    {
        int total = spritesFps.Length;

        indiceFps = (indiceFps + direcao + total) % total;

        LimitadorFPS.Aplicar(LimitadorFPS.Opcoes[indiceFps]);

        AtualizarFps();
    }
    private void AtualizarFps() => imagemFps.sprite = spritesFps[indiceFps];

    // MODO DE TELA
    public void ProximoTela() => MudarTela(1);
    public void AnteriorTela() => MudarTela(-1);

    public void MudarTela(int direcao)
    {
        int total = spritesModoTela.Length;

        indiceModoTela = (indiceModoTela + direcao + total) % total;

        ModoTela();
        AtualizarModoTela();

        PlayerPrefs.SetInt("ModoTela", indiceModoTela);
        PlayerPrefs.Save();
    }

    private void AtualizarModoTela() => imagemModoTela.sprite = spritesModoTela[indiceModoTela];

    private void ModoTela()
    {
        switch (indiceModoTela)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }
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

    private void MostrarPainel(GameObject painel)
    {
        painelMenu.SetActive(false);
        painelCreditos.SetActive(false);
        painelConfiguracoes.SetActive(false);

        painel.SetActive(true);
    }
}