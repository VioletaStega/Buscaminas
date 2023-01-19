using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public GameObject gameButton;
    public GameObject gamePanel;
    public GameObject winPanel;
    public GameObject menuPanel;

    PointerEventData myPointer;
    public GraphicRaycaster myRaycaster;

    int width,
        height;
    int bombsAmount,
        counter;

    bool die;
    public bool firstClick;

    public Image imageEmoji;
    public Sprite[] imageSprite;

    ButtonScript[,] map;
    
    // Start is called before the first frame update
    void Start()
    {        
        gm = this;        
        die = false;
        firstClick = true;    
    }

    // Update is called once per frame
    void Update()
    {
        DialButton();
    }

    public void StartGame()
    {
        NumbersOfBombs();
        map = new ButtonScript[width, height];
        counter = (width * height) - bombsAmount;
        CreateButtons();
        CreateBombs();
    }
    
    public void DifficultySelector(int num)
    {
        switch (num)
        {
            case 1:
                width = 5;
                height = 5;
                menuPanel.SetActive(false);
                break;
            case 2:
                width = 10;
                height = 10;
                menuPanel.SetActive(false);
                break;
            case 3:
                width = 20;
                height = 10;
                menuPanel.SetActive(false);
                break;
            default:
                break;
        }
        StartGame();
    }
    public void CreateButtons()
    {
        gamePanel.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gamePanel.GetComponent<GridLayoutGroup>().constraintCount = width;


        for (int i = 0; i < width * height; i++)
        {
            int x = i / height;
            int y = i % height;
            map[x, y] = Instantiate(gameButton, gamePanel.transform).GetComponent<ButtonScript>();
            map[x, y].x = x;
            map[x, y].y = y;
        }
    }

    public void CreateBombs()
    {
        for (int i = 0; i < bombsAmount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (map[x, y].bomb == true)
            {
                i--;
            }
            else
            {
                map[x, y].bomb = true;
            }
        }
    }

    public void ClickAround(int x, int y)
    {
        //Botones de la izquierda
        if (x > 0)
        {
            //Izquierda
            map[x - 1, y].Click();
            
            //Izquierda abajo
            if(y > 0)
            {
                map[x - 1, y - 1].Click();
            }

            //Izquierda arriba
            if (y < height - 1)
            {
                map[x - 1, y + 1].Click();
            }
        }

        //Botones de la derecha
        if (x < width - 1)
        {
            //Derecha
            map[x + 1, y].Click();        

            //Derecha abajo
            if (y > 0)
            {
                map[x + 1, y - 1].Click();
            }

            //Derecha arriba
            if (y < height - 1)
            {
                map[x + 1, y + 1].Click();
            }
        }

        //Abajo
        if (y > 0)
        {
            map[x, y - 1].Click();
        }

        //Arriba
        if (y < height - 1)
        {
            map[x, y + 1].Click();
        }

    }

    public int CheckBombNumber(int x, int y)
    {
        int result = 0;

        //Todos los botones a mi izquierda
        if(x > 0)
        {
            //Si hay una bomba a mi izquierda
            if (map[x - 1, y].bomb)
            {
                result++;
            }

            //Si hay una bomba a mi izquierda abajo
            if (y > 0 && map[x - 1, y - 1].bomb)
            {
                result++;
            }

            //Si hay una bomba a mi izquierda arriba
            if (y < height - 1 && map[x - 1, y + 1].bomb == true)
            {
                result++;
            }
        }

        //Todos los botones a mi derecha
        if (x < width - 1)
        {
            //Si hay una bomba a mi derecha
            if (map[x + 1, y].bomb)
            {
                result++;
            }

            //Si hay una bomba a mi derecha abajo
            if (y > 0 && map[x + 1, y - 1].bomb)
            {
                result++;
            }

            //Si hay una bomba a mi derecha arriba
            if (y < height - 1 && map[x + 1, y + 1].bomb)
            {
                result++;
            }
        }

        //Si hay bomba arriba
        if (y < height - 1 && map[x, y + 1].bomb)
        {
            result++;
        }

        //Si hay bomba abajo
        if (y > 0 && map[x, y - 1].bomb)
        {
            result++;
        }


        return result;
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExplodeMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                die = true;
                map[i, j].Click();
            }
        }
        imageEmoji.sprite = imageSprite[1];
    }

    public void DecreaseCounter()
    {
        counter--;
        Debug.Log(counter);

        if (die == false && counter == 0)
        {
            winPanel.SetActive(true);
        }
    }

    private void DialButton()
    {
        //Fire2 para el botón derecho del ratón
        if (Input.GetButtonDown("Fire2"))
        {
            myPointer = new PointerEventData(EventSystem.current);
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            myPointer.position = Input.mousePosition;
            myRaycaster.Raycast(myPointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                for (int i = 0; i < raycastResults.Count; i++)
                {
                    Button buttonResult = raycastResults[i].gameObject.GetComponent<Button>();

                    //Sí es un botón
                    if (buttonResult)
                    {
                        TextMeshProUGUI changeText = raycastResults[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        Image changeImage = raycastResults[i].gameObject.GetComponent<Image>();

                        if (buttonResult.interactable)
                        {
                            changeText.text = "?";
                            changeImage.color = Color.cyan;
                            buttonResult.interactable = false;
                            imageEmoji.sprite = imageSprite[3];
                        }
                        else if (changeText.text.Equals("?"))
                        {
                            changeText.text = "";
                            changeImage.color = Color.white;
                            buttonResult.interactable = true;
                            imageEmoji.sprite = imageSprite[0];
                        }
                    }
                }
                
            }

        }
    }

    private void NumbersOfBombs()
    {
        bombsAmount = Random.Range(10, (width * height) / 2);

        if (height * width <= 10)
        {
            bombsAmount = 1;            
        }
        else if(height * width > 10 && height * width < 30)
        {
            bombsAmount = 5;
        }
    }
}
