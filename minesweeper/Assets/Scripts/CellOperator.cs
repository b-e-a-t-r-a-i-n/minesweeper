using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellOperator : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    public bool IsBomb { get; set; }
    public GameObject gm;
    GameObject numbersCanvas;

    public bool revealed = false;
    public bool checkd = false;

    Component[] allCells;
    GameManager gameManager;
    FieldOperator fieldOperator;
    int coordsX, coordsY, width, height;
    public int CoordsX { get { return coordsX; } set { coordsX = value; } } // Посредством этих двух свойств экземпляр каждой клетки знает свои координаты
    public int CoordsY { get { return coordsY; } set { coordsY = value; } } //
    public int Width { get { return width; } set { width = value; } }
    public int Height { get { return height; } set { height = value; } }

    public int unrevealedGoal;

    void Start () {
        gm = GameObject.Find("GM");
        gameManager = gm.GetComponent<GameManager>();
        allCells = gm.GetComponentsInChildren<CellOperator>();
        numbersCanvas = GameObject.Find("CanvasNumbers");
        fieldOperator = gm.GetComponent(typeof(FieldOperator)) as FieldOperator;
    }
	

    private void LeftClick()
    {
        if (!checkd) // Если флаг - нажать нельзя
        {
            if (IsBomb == true && revealed == false) // проверяем на подрыв
            {
                Debug.Log("Game Over");
                foreach (CellOperator CO in allCells)
                {
                    CO.RevealMachine();
                }
                for (int i = 0; i < numbersCanvas.transform.childCount; i++)
                {
                    var child = numbersCanvas.transform.GetChild(i).gameObject;
                    if (child != null)
                        child.SetActive(true);
                }
                gameManager.GameOverAppears();
            }
            else
            {
                RevealMachine();
            }
            IsVictory();
        }
    }

    private void RightClick()
    {
        if (!revealed && !checkd)
        {
            fieldOperator.mineCount -= 1;
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("graphics/minesweeper_cell_checked", typeof(Sprite)) as Sprite;
            checkd = true;
            fieldOperator.ChangeMineCountText();
        }
        else
        if (!revealed && checkd)
        {
            fieldOperator.mineCount += 1;
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("graphics/minesweeper_cell", typeof(Sprite)) as Sprite;
            checkd = false;
            fieldOperator.ChangeMineCountText();
        }
        IsVictory();
        // дальнейшая логика переходит в fieldoperator
    }

    private void IsVictory()
    {
        if (fieldOperator.mineCount == 0) // проверка на победу
        {
            unrevealedGoal = 0;
            foreach (CellOperator co in allCells)
            {
                if (!co.revealed)
                {
                    unrevealedGoal++;
                }
            }
            if (unrevealedGoal == fieldOperator.mineCountConst)
            {
                print("Victory. Game Over.");
                gameManager.WinGameOverAppears();
            }
        }
    }

    public void RevealMachine()
    {  
        if (revealed == false)
        {
            if (IsBomb != true)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("graphics/minesweeper_cell_revealed_empty", typeof(Sprite)) as Sprite;
                revealed = true;
                fieldOperator.DrawNumberOfNeighborsForOneField(coordsX, coordsY); // если вокруг есть бомбы - рисовать на клетке их число    
                
                // flood fill
                if (fieldOperator.neighborsCountTotal[coordsY, coordsX] < 1)
                {
                    for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                int b = coordsX + i;
                                int a = coordsY + j;
                            if (a > -1 && a < height && b > -1 && b < width)
                                {
                                    if (!fieldOperator.instancesOfCells[a, b].GetComponent<CellOperator>().revealed)
                                    {
                                        fieldOperator.instancesOfCells[a, b].GetComponent<CellOperator>().RevealMachine();
                                        revealed = true;
                                    }
                                }
                            }
                        }
                    }
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("graphics/minesweeper_cell_boom", typeof(Sprite)) as Sprite;
                revealed = true;
            }
            revealed = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick();
        }
    }
}
