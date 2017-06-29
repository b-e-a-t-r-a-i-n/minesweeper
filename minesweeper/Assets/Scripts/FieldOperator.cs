using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldOperator : MonoBehaviour
{

    int width;
    int height;
    public int mineCount;       // оно меняется в коде; создать свойство ёпт
    public int mineCountConst;  // оно не меняется до конца игры

    float[] camPos = { 5.75f, 8.92f, 12.15f };
    float[] camSize = { 6.8f, 10.17f, 13.5f };
    byte difficulty;
    public Camera mainCamera;

    public GameObject gui;

    int[][] field;
    public GameObject CellUnRevealed;

    public int[,] neighborsCountTotal;
    GameObject[,] instancesOfNumbers;

    public GameObject canvasWithNeighborsCounters;
    public GameObject textOnCanvas;
    public GameObject[,] instancesOfCells;
    float xCoord = 1.28f;
    float yCoord = 1.28f;

    public void InitializeMe(int width, int height, int mineCount, byte difficulty)
    {
        this.width = width;
        this.height = height;
        this.mineCount = mineCount;
        this.difficulty = difficulty;
        mineCountConst = mineCount;
    }


    int[][] GenerateArray(int cols, int rows)
    {
        int[][] arr = new int[rows][];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new int[cols];
        }
        return arr;
    }

    void FillWithMines(int[][] arr, int minesCount) // Расставляем нужное количество мин
    {
        do
        {
            int x = UnityEngine.Random.Range(0, width);
            int y = UnityEngine.Random.Range(0, height);
            if (arr[x][y] != 1)
            {
                arr[x][y] = 1;
                minesCount--;
            }
        }
        while (minesCount > 0);
    }

    void NeighborsCounter()                            // Циклом бежим по финальному массиву с бомбами, 
    {
        neighborsCountTotal = new int[width, height];  // массив со счетчиками бомб в округе
        for (int iBig = 0; iBig < width; iBig++)
        {
            for (int jBig = 0; jBig < height; jBig++)
            {
                if (field[iBig][jBig] != 1)
                {
                    neighborsCountTotal[iBig, jBig] = FindNeighbors(iBig, jBig); // В свою очередь на каждом элементе запуская мини-цикл для проверки соседних клеток на наличие бомб

                }
                else
                {
                    neighborsCountTotal[iBig, jBig] = 0;
                }
            }
        }
    }

    int FindNeighbors(int x, int y)                    // поиск бомб в квадрате 3х3
    {
        int totalOfThem = 0;                           // счетчик бомб
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int a = x + i;
                int b = y + j;
                if (a > -1 && a < height && b > -1 && b < width)
                {
                    if (field[a][b] == 1)
                    {
                        totalOfThem++;
                    }
                }
            }
        }
        return totalOfThem;
    }


    public void StartNewGame()                       // Игра начинается с вызова этого метода
    {
        InitializeCamera();

        field = GenerateArray(width, height);        // Создаём игровое поле - массив массивов (ступенчатый массив)

        FillWithMines(field, mineCount);

        NeighborsCounter();
        DrawNumberOfNeighbors();
        DrawGameField(field);
        ChangeMineCountText();
    }

    private void InitializeCamera()
    {
        mainCamera.transform.position = new Vector3(camPos[difficulty], camPos[difficulty], -10);
        Camera.main.orthographicSize = camSize[difficulty];
    }

    void DrawGameField(int[][] fieldArray)
    {
        if (CellUnRevealed != null)
        {
            float positionX = CellUnRevealed.GetComponent<BoxCollider2D>().size.x;
            float positionY = CellUnRevealed.GetComponent<BoxCollider2D>().size.y;
            instancesOfCells = new GameObject[width, height];

            for (int i = 0; i < fieldArray.Length; i++)
            {
                for (int j = 0; j < fieldArray.Length; j++)
                {
                    instancesOfCells[i, j] = Instantiate(CellUnRevealed, new Vector3(positionX * j, positionY * i, CellUnRevealed.transform.position.z), Quaternion.identity) as GameObject;
                    instancesOfCells[i, j].transform.SetParent(transform);
                    instancesOfCells[i, j].GetComponent<CellOperator>().CoordsX = j;
                    instancesOfCells[i, j].GetComponent<CellOperator>().CoordsY = i;
                    instancesOfCells[i, j].GetComponent<CellOperator>().Width = width;
                    instancesOfCells[i, j].GetComponent<CellOperator>().Height = height;
                    if (fieldArray[i][j] == 1)
                        instancesOfCells[i, j].GetComponent<CellOperator>().IsBomb = true;
                    else
                        instancesOfCells[i, j].GetComponent<CellOperator>().IsBomb = false;
                }
            }
        }
    }

    public void DrawNumberOfNeighbors() // Рисуем все цифры
    {
        instancesOfNumbers = new GameObject[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                if (neighborsCountTotal[i, j] != 0)
                {
                    instancesOfNumbers[i, j] = Instantiate(textOnCanvas, new Vector3(xCoord * j, yCoord * i, textOnCanvas.transform.position.z - 0.20f), Quaternion.identity) as GameObject;
                    instancesOfNumbers[i, j].GetComponent<Text>().text = neighborsCountTotal[i, j].ToString();
                    instancesOfNumbers[i, j].transform.SetParent(canvasWithNeighborsCounters.transform);
                    instancesOfNumbers[i, j].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    instancesOfNumbers[i, j].SetActive(false);
                }
            }
        }
    }

    public void DrawNumberOfNeighborsForOneField(int x, int y) // Рисуем одну цифру
    {
        if (neighborsCountTotal[y, x] != 0)
        {
            instancesOfNumbers[y, x].SetActive(true);
            instancesOfNumbers[y, x].GetComponent<Text>().text = neighborsCountTotal[y, x].ToString();
            instancesOfNumbers[y, x].transform.SetParent(canvasWithNeighborsCounters.transform);
            instancesOfNumbers[y, x].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }

    public void ChangeMineCountText()
    {
        gui.GetComponent<Text>().text = "MINES: " + mineCount.ToString();
    }

    private void Update()
    {
        
    }
}
