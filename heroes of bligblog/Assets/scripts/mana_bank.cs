using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class mana_bank : MonoBehaviour
{
    public int max_mana_azul, max_mana_vermelha, dis_mana_azul, dis_mana_vermelha, spend_mana_azul, spend_mana_vermelha;
    private Text qt_mana;


    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("num_mana"))
        {
            qt_mana = GameObject.Find("num_mana").GetComponent<Text>();
            dis_mana_azul = max_mana_azul - spend_mana_azul;
            dis_mana_vermelha = max_mana_vermelha - spend_mana_vermelha;
            qt_mana.text = "A:" + dis_mana_azul + "/" + "V:" + dis_mana_vermelha;
        }
    }
}
