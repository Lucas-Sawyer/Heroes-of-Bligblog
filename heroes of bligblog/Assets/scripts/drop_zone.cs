using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class drop_zone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private match_controller match_Controller;
    private card card;
    private mana_bank mana_bank;

    private void Start()
    {
        match_Controller = GameObject.Find("Main Camera").GetComponent<match_controller>();
        mana_bank = GameObject.Find("mana_bank").GetComponent<mana_bank>();
    }

    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<card>())
            {
                card = child.GetComponent<card>();
                for (int n = 0; n < card.efeito_array.Length - 1; n++)
                {
                    string efeito = GetValue(card.efeito_array[i], "efeito:");
                    string quant_string = GetValue(card.efeito_array[i], "quant:");
                    int quant = int.Parse(quant_string);
                    string cond = GetValue(card.efeito_array[i], "cond:");

                    switch (efeito)
                    {
                        case "vira":
                            switch (card.cor)
                            {
                                case "azul":
                                    if (quant <= mana_bank.dis_mana_azul && match_Controller.play)
                                    {
                                        card.playable = true;
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        card d = eventData.pointerDrag.GetComponent<card>();
        if (d.playable)
        {
            switch (d.tipo)
            {
                case "energia":
                    d.parent_to_go = GameObject.Find("mana_bank").transform;
                    match_Controller.mana_play = false;
                    switch (d.subtipo_string)
                    {
                        case "energia azul":
                            GameObject.Find("mana_bank").GetComponent<mana_bank>().max_mana_azul++;
                            break;
                        case "energia vermelha":
                            GameObject.Find("mana_bank").GetComponent<mana_bank>().max_mana_vermelha++;
                            break;
                    }
                    break;
                case "criatura":
                    d.parent_to_go = transform;
                    break;
                case "magica":
                    switch (d.subtipo_string)
                    {
                        case "ritual":
                            d.parent_to_go = GameObject.Find("rituais").transform;
                            break;
                        case "instantanea":
                            d.parent_to_go = GameObject.Find("descarte").transform;
                            break;
                        case "feitiço":
                            d.parent_to_go = GameObject.Find("descarte").transform;
                            break;
                    }
                    break;
            }
            switch (d.cor)
            {
                case "vermelho":
                    GameObject.Find("mana_bank").GetComponent<mana_bank>().spend_mana_vermelha += d.custo;
                    break;
                case "azul":
                    GameObject.Find("mana_bank").GetComponent<mana_bank>().spend_mana_azul += d.custo;
                    break;
            }
        }
        else { }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag)
        {
            card d = eventData.pointerDrag.GetComponent<card>();
            if (d.playable)
            {
                d.placeholder.transform.SetParent(this.transform);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag)
        {
            card d = eventData.pointerDrag.GetComponent<card>();
            if (d.playable)
            {
                d.placeholder.transform.SetParent(d.parent_to_go);
            }
        }
    }
    string GetValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("/")) value = value.Remove(value.IndexOf("/"));
        return value;
    }
}
