using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Jogo/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string nome;
    public Sprite icone;
    public GameObject prefabInspecao;

    [TextArea(3, 15)]
    public string descricao;

    [Header("Estilo")]
    public FontStyles estilo = FontStyles.Normal; //combináveis
    public TextAlignmentOptions alinhamento = TextAlignmentOptions.TopLeft;

    public string Nome => string.IsNullOrEmpty(nome) ? name : nome;
}