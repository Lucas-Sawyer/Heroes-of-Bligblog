using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class match_controller : MonoBehaviour
{
    private GameObject canvas, draw_b, deck, mao, mana_bank;

    public bool next = false, combate = false, conect = false, insta_play = false, play = false, mana_play = false, draw = false, custo_vermelho = false, custo_azul = false, enemy_turn;


    IEnumerator Start()
    {
        canvas = GameObject.Find("Canvas");
        draw_b = GameObject.Find("draw_b");
        deck = GameObject.Find("deck");
        mao = GameObject.Find("mão");
        mana_bank = GameObject.Find("mana_bank");
        canvas.SetActive(false);
        draw_b.SetActive(false);
        using (UnityWebRequest cards_data = UnityWebRequest.Get("https://teste-mikrotik.000webhostapp.com/paginas/cartas.php"))
        {
            yield return cards_data.SendWebRequest();
            if (cards_data.isNetworkError || cards_data.isHttpError)
            {
                Debug.Log(cards_data.error);
            }
            else
            {
                conect = true;
                canvas.SetActive(true);
                StartCoroutine("inicial_draw");
            }
        }
    }

    private void Update()
    {
        Screen.fullScreen = true;
        /*for (int i = 0; i < mao.transform.childCount; i++)
        {
            Transform child = mao.transform.GetChild(i);
            if (child.GetComponent<card>())
            {
                if (custo_vermelho || custo_azul)
                {
                    switch (child.GetComponent<card>().tipo)
                    {
                        case "magica":
                            switch (child.GetComponent<card>().subtipo_string)
                            {
                                case "instantanea":
                                    if (insta_play || play)
                                    {
                                        child.GetComponent<card>().playable = true;
                                    }
                                    else
                                    {
                                        child.GetComponent<card>().playable = false;
                                    }
                                    break;
                            }
                            if (play)
                            {
                                child.GetComponent<card>().playable = true;
                            }
                            else
                            {
                                child.GetComponent<card>().playable = false;
                            }
                            break;
                        case "criatura":
                            if (play)
                            {
                                child.GetComponent<card>().playable = true;
                            }
                            else
                            {
                                child.GetComponent<card>().playable = false;
                            }
                            break;
                        case "energia":
                            if (mana_play)
                            {
                                child.GetComponent<card>().playable = true;
                            }
                            else
                            {
                                child.GetComponent<card>().playable = false;
                            }
                            break;
                    }
                }
            }
        }*/
        if (draw)
        {
            draw_b.SetActive(true);
        }
        else
        {
            draw_b.SetActive(false);
        }
    }

    public void next_f()
    {
        next = true;
    }

    IEnumerator inicial_draw()
    {
        yield return new WaitForSeconds(0);
        for (int i = 0; i < 5; i++)
        {
            deck.GetComponent<deck>().draw();
        }
        mana_bank.GetComponent<mana_bank>().spend_mana_azul = 0;
        mana_bank.GetComponent<mana_bank>().spend_mana_vermelha = 0;
        int moeda = Random.Range(0, 100);
        StartCoroutine("compra_inicial");
        /*if (moeda < 50)
        {
            enemy_turn = true;
        }
        else
        {
            
        }*/
    }
    IEnumerator compra_inicial()
    {
        yield return new WaitForSeconds(0);
        deck.GetComponent<deck>().draw();
        StartCoroutine("fase_principal");
    }
    IEnumerator fase_principal()
    {
        mana_play = true;
        play = true;
        yield return new WaitUntil(() => next == true);
        next = false;
        StartCoroutine("fase_combate");
    }
    IEnumerator fase_combate()
    {
        combate = true;
        mana_play = false;
        play = false;
        insta_play = true;
        yield return new WaitUntil(() => next == true);
        next = false;
    }
    void combate_f()
    {
        for (int i = 0; i < GameObject.Find("tabuleiro").transform.childCount; i++)
        {
            Transform child = GameObject.Find("tabuleiro").transform.GetChild(i);
            if (child.GetComponent<card>().ataque != 0 && !child.GetComponent<card>().enjoado)
            {
                child.GetComponent<card>().playable = true;
            }
            else
            {
                child.GetComponent<card>().playable = false;
            }
        }
    }
}