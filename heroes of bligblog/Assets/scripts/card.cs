using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Transform parent_to_go = null;
    public GameObject placeholder = null, tabuleiro;
    public Image art, mana;
    public new string name;
    public string desc, ataque_string, vida_string, custo_string, tp_energia, tipo, subtipo_string, turnos_string, efeito_string, cor;
    public string[] efeito_array, subtipo;
    public Text nome_text, desc_text, ataque_text, vida_text;
    public float click_count;
    public bool show = false, playable = false, enjoado = true, efeito_done = false;
    public int vida, ataque, turnos, custo, armadura, custo_original;
    public mana_bank mana_Bank;

    private RectTransform rect_transform;
    private string path;


    void Start()
    {
        custo_original = int.Parse(custo_string);
        turnos = int.Parse(turnos_string);
        vida = int.Parse(vida_string);
        ataque = int.Parse(ataque_string);
        efeito_array = efeito_string.Split('-');
        subtipo = subtipo_string.Split('/');
        rect_transform = GetComponent<RectTransform>();
        /*if (File.Exists("Assets/imgs/cards/" + name + ".jpg"))
        {
            path = "Assets/imgs/cards/" + name + ".jpg";
        }*/
        tabuleiro = GameObject.Find("tabuleiro");
        custo = custo_original;
    }
    void Update()
    {
        playable = true;
        nome_text.text = name;
        transform.name = name;
        desc_text.text = desc;
        if (ataque == 0 && vida == 0)
        {
            ataque_text.text = null;
            vida_text.text = null;
        }
        else
        {
            ataque_text.text = ataque_string;
            vida_text.text = vida_string;
        }
        //art.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        for (int i = 0; i < efeito_array.Length - 1; i++)
        {
            string efeito = GetValue(efeito_array[i], "efeito:");
            string quant_string = GetValue(efeito_array[i], "quant:");
            int quant = int.Parse(quant_string);
            string cond = GetValue(efeito_array[i], "cond:");
            switch (efeito)
            {
                case "reduz mana":
                    switch (cond)
                    {
                        case "mago de fogo":
                            bool result = subtipo_check(cond, quant, 0);
                            if (result)
                            {
                                if (!efeito_done)
                                {
                                    custo -= quant;
                                    efeito_done = true;
                                }
                            }
                            else
                            {
                                if (efeito_done)
                                {
                                    custo += quant;
                                    efeito_done = false;
                                }
                            }
                            break;
                    }
                    break;
                case "reduz turno":
                    switch (cond)
                    {
                        case "mago":
                            bool result = subtipo_check(cond, quant, 0);
                            if (result)
                            {
                                if (!efeito_done)
                                {
                                    turnos -= quant;
                                    efeito_done = true;
                                }
                            }
                            else
                            {
                                if (efeito_done)
                                {
                                    turnos += quant;
                                    efeito_done = false;
                                }
                            }
                            break;
                    }
                    break;
                case "reduz dano":
                    if (!efeito_done)
                    {
                        armadura += quant;
                        efeito_done = true;
                    }
                    break;
            }
        }
        switch (tp_energia)
        {
            case "a":
                cor = "azul";
                break;
            case "v":
                cor = "vermelho";
                break;
        }
        if (!show)
        {
            rect_transform.localScale = new Vector3(1, 1, 1);
        }
        if (!playable)
        {
            Image card_image = GetComponent<Image>();
            card_image.color = new Color(card_image.color.r, card_image.color.g, card_image.color.b, 0.5f);
        }
        else
        {
            Image card_image = GetComponent<Image>();
            card_image.color = new Color(card_image.color.r, card_image.color.g, card_image.color.b, 1);
        }
    }

    public static bool subtipo_check(string subtipo, int quant, int vezes)
    {
        for (int i = 0; i < GameObject.Find("tabuleiro").transform.childCount; i++)
        {
            Transform child = GameObject.Find("tabuleiro").transform.GetChild(i);
            if (child.GetComponent<card>())
            {
                foreach (string subtipos in child.GetComponent<card>().subtipo)
                {
                    if (string.Equals(subtipos, subtipo)) vezes++;
                }
            }
        }
        if (vezes == quant) return true; else return false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (playable)
        {
            Debug.Log("begin");
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            placeholder = new GameObject();
            placeholder.transform.SetParent(transform.parent);
            placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
            LayoutElement le = placeholder.AddComponent<LayoutElement>();
            le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
            parent_to_go = transform.parent;
            transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (playable)
        {
            Debug.Log("drag");
            transform.position = eventData.position;
            int new_index = parent_to_go.childCount;
            for (int i = 0; i < placeholder.transform.parent.childCount; i++)
            {
                if (this.transform.position.x < placeholder.transform.parent.GetChild(i).position.x)
                {
                    new_index = i;
                    if (placeholder.transform.GetSiblingIndex() < new_index)
                    {
                        new_index--;
                    }
                    break;
                }
            }
            placeholder.transform.SetSiblingIndex(new_index);
        }
        click_count = 0;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (playable)
        {
            Debug.Log("end");
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(parent_to_go);
            transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            Destroy(placeholder);
        }
    }

    public void OnPointerClick(PointerEventData click)
    {
        print("click");
        if (show)
        {
            transform.SetParent(parent_to_go);
            transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            Destroy(placeholder);
            show = false;
        }
        else
        {
            click_count++;
        }
        if (click_count > 1)
        {
            placeholder = new GameObject();
            placeholder.transform.SetParent(transform.parent);
            placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
            LayoutElement le = placeholder.AddComponent<LayoutElement>();
            le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
            parent_to_go = transform.parent;
            transform.SetParent(GameObject.Find("Canvas").transform);
            rect_transform.anchoredPosition = new Vector2(0, 0);
            rect_transform.anchorMax = new Vector2(0.5f, 0.5f);
            rect_transform.anchorMin = new Vector2(0.5f, 0.5f);
            rect_transform.pivot = new Vector2(0.5f, 0.5f);
            //rect_transform.position = new Vector3(0, 0, 0);
            rect_transform.localScale = new Vector3(6, 6, 6);
            show = true;
            click_count = 0;
        }
    }
    string GetValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("/")) value = value.Remove(value.IndexOf("/"));
        return value;
    }
    void Vira()
    {

    }
}
