using System.Collections.Generic;
using UnityEngine;

public class MapaManager : MonoBehaviour
{
    [Header("Paineis")]
    [SerializeField] GameObject painelMapa; //painel do mapita
    [SerializeField] GameObject painelSalas; //painel que contém a visão das salas

    /*[Header("Locais")] //lista dos locais do mapa p/ organiza
    [SerializeField] private List<GameObject> indiceLocalizacao = new List<GameObject>();*/

    //leva o jogador pra sala baseado no indice
    public void IrParaLugar(int indiceSala)
    {
        FecharMapa(); //fecha o mapa

        if (painelSalas != null) painelSalas.SetActive(true); //abre painel das salas

        if (RoomManager.Instance != null) //checa o RoomManager
        {
            RoomManager.Instance.MostrarSala(indiceSala); //pega sala do RoomManager
        }
        else
        {
            Debug.LogWarning("tem naum"); 
        }
    }

    public void AbrirMapa() //abre o painel do mapa
    {
        painelMapa.SetActive(true);
    }

    public void FecharMapa() //fecha o painel do mapa
    {
        painelMapa.SetActive(false);
    }
}
