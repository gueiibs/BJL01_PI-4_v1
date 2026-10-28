using UnityEngine;

[CreateAssetMenu(menuName = "Jogo/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string NomeExibicao; //string pra nome usado para jogador
    public Sprite icone;
    public GameObject prefabInspecao;

    //sem nome In Game setado, usa o nome do arquivo
    public string Nome => string.IsNullOrEmpty(NomeExibicao) ? name : NomeExibicao;
}