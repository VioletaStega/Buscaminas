using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ButtonScript : MonoBehaviour
{
    //Conocer la posición de nuestro botón
    public int x,
               y;

    //Para saber si en el botón hay una bomba
    public bool bomb;

    public void Click()
    {
        //Evitar que al primer click salga una bomba
        if (GameManager.gm.firstClick)
        {
            bomb = false;
            GameManager.gm.firstClick = false;
        }

        //Evitamos que nos salga un bucle infinto, al hacer ClickAround()
        if (GetComponent<Button>().interactable == false)
        {
            return;
        }
        
        IsThereABomb();
    }

    private void IsThereABomb()
    {
        GetComponent<Button>().interactable = false;

        if (bomb)
        {
            //GAME OVER
            Debug.Log("GAME OVER");
            GetComponent<Image>().color = Color.red;
            ChangeTextColor(9);
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "*";
            GameManager.gm.ExplodeMap();
        }
        else
        {
            //Guardamos el número de bombas alrededor de nuestro botón
            int num = GameManager.gm.CheckBombNumber(x, y);
            GameManager.gm.DecreaseCounter();

            if (num == 0)
            {
                GameManager.gm.ClickAround(x, y);
                GameManager.gm.imageEmoji.sprite = GameManager.gm.imageSprite[2];
            }
            else
            {
                //Vamos a pintar nuestro botón con el número de bombas
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = num.ToString();
                ChangeTextColor(num);
                GameManager.gm.imageEmoji.sprite = GameManager.gm.imageSprite[2];
            }           
            
        }
    }

    private void ChangeTextColor(int num)
    {
        TextMeshProUGUI changeColor = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        switch (num)
        {
            case 1:
                changeColor.color = Color.blue;
                break;
            case 2:
                changeColor.color = Color.cyan;
                break;
            case 3:
                changeColor.color = Color.green;
                break;
            case 4:
                changeColor.color = Color.yellow;
                break;
            case 5:
                changeColor.color = Color.red;
                break;
            case 6:
                changeColor.color = Color.magenta;
                break;
            case 7:
                changeColor.color = Color.gray;
                break;
            case 8:
                changeColor.color = Color.black;
                break;
            case 9:
                changeColor.color = Color.white;
                break;
            default:
                break;
        }
    }
}
