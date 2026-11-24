
using UnityEngine;
using UnityEditor;
using System.IO;

public class Teste
{
    [MenuItem("Projeto/Textures/Gerar Textura Geral")]
    public static void Gerar()
    {
        int tamanho = 512;

        Texture2D textura = new Texture2D(
            tamanho,
            tamanho,
            TextureFormat.RGB24,
            false
        );

        for (int y = 0; y < tamanho; y++)
        {
            for (int x = 0; x < tamanho; x++)
            {
                float u = x / (float)tamanho;
                float v = y / (float)tamanho;

                // Manchas grandes
                float manchas = Mathf.PerlinNoise(
                    u * 5f, v * 5f
                );

                // Variações médias
                float variacao = Mathf.PerlinNoise(
                    u * 18f + 10f,
                    v * 18f + 4f
                );

                // Granulação fina
                float grao = Mathf.PerlinNoise(
                    u * 90f + 30f,
                    v * 90f + 12f
                );

                float valor =
                    0.64f +
                    manchas * 0.18f +
                    variacao * 0.12f +
                    grao * 0.06f;

                valor = Mathf.Clamp01(valor);

                Color cor = new Color(
                    valor, valor, valor, 1f
                );

                textura.SetPixel(x, y, cor);
            }
        }

        textura.Apply();

        Directory.CreateDirectory("Assets/Textures");

        string caminho = "Assets/Textures/Texture_Geral.png";

        File.WriteAllBytes(
            caminho,
            textura.EncodeToPNG()
        );

        Object.DestroyImmediate(textura);

        AssetDatabase.Refresh();

        TextureImporter importer =
            AssetImporter.GetAtPath(caminho)
            as TextureImporter;

        importer.wrapMode = TextureWrapMode.Repeat;
        importer.filterMode = FilterMode.Bilinear;
        importer.textureCompression =
            TextureImporterCompression.Compressed;

        importer.SaveAndReimport();

        Debug.Log("Textura geral criada!");
    }
}
