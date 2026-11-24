using UnityEngine;

public static class LimitadorFPS
{
    public static readonly int[] Opcoes = { 30, 60, 120, 140, 240 };
    private const string Chave = "Fps";

    //roda uma vez ao abrir o jogo, antes de qualquer cena
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Iniciar() => Aplicar(PlayerPrefs.GetInt(Chave, 60));

    public static void Aplicar(int fps)
    {
        QualitySettings.vSyncCount = 0; //o VSync ignora o targetFrameRate
        Application.targetFrameRate = fps;
        PlayerPrefs.SetInt(Chave, fps);
        PlayerPrefs.Save();
    }

    //índice da opção atual (60 se o valor salvo não estiver na lista)
    public static int IndiceAtual()
    {
        int i = System.Array.IndexOf(Opcoes, Application.targetFrameRate);
        return i >= 0 ? i : 1;
    }
}