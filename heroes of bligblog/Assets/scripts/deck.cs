using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class deck : MonoBehaviour
{
    private string page_text;

    public string[] cards;
    public int num_cards;
    public GameObject card_ob;
    public card data;
    public Text num_display;
    IEnumerator Start()
    {
        using (UnityWebRequest cards_data = UnityWebRequest.Get("https://teste-mikrotik.000webhostapp.com/paginas/cartas_baralhos.php"))
        {
            yield return cards_data.SendWebRequest();
            if (cards_data.isNetworkError || cards_data.isHttpError)
            {
                Debug.Log(cards_data.error);
            }
            else
            {
                page_text = cards_data.downloadHandler.text;

                cards = page_text.Split(';');
                //Debug.Log("teste - " + cards.Length);
                for (int i = 0; i < cards.Length - 1; i++)
                {
                    string nome_card = GetValue(cards[i], "nome_card:");
                    string desc = GetValue(cards[i], "descricao:");
                    string custo = GetValue(cards[i], "custo:");
                    string tp_energia = GetValue(cards[i], "tp_energia:");
                    string ataque = GetValue(cards[i], "ataque:");
                    string vida = GetValue(cards[i], "vida:");
                    string tipo = GetValue(cards[i], "tipo:");
                    string subtipo = GetValue(cards[i], "subtipo:");
                    string turnos = GetValue(cards[i], "turnos:");
                    string efeito = GetValue(cards[i], "efeito:");
                    string id_deck = GetValue(cards[i], "id_deck:");
                    switch (id_deck)
                    {
                        case "teste/1":
                            //print(cards[i]);
                            num_cards = i + 1;
                            //print(num_cards);
                            Instantiate(card_ob, transform);
                            data.name = nome_card;
                            data.desc = desc;
                            data.ataque_string = ataque;
                            data.vida_string = vida;
                            data.custo_string = custo;
                            data.tp_energia = tp_energia;
                            data.tipo = tipo;
                            data.subtipo_string = subtipo;
                            data.turnos_string = turnos;
                            data.efeito_string = efeito;
                            break;
                    }
                }
                embaralhar();
                //print(GetValue(cards[0], "nome_card:"));
            }
        }
    }

    private void Update()
    {
        num_display.text = "" + transform.childCount;
    }

    string GetValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }
    public void draw()
    {
        match_controller match_Controller = GameObject.Find("Main Camera").GetComponent<match_controller>();
        if (match_Controller.draw || match_Controller.mana_play)
        {
            match_Controller.draw = false;
            match_Controller.mana_play = false;
        }
        Transform child = transform.GetChild(0);
        child.SetParent(GameObject.Find("mão").transform);
    }
    public void embaralhar()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(0).SetSiblingIndex(Random.Range(1, transform.childCount));
        }
    }
}
