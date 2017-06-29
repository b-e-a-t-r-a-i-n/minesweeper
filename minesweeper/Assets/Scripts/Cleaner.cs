using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cleaner : MonoBehaviour, IPointerClickHandler {

    public GameObject gm;   //ссылка на объект в иерархии GM
    GameManager gameManager;
    public GameObject canvasWithNumbers;

    void DeleteAllCells ()
    {
        for (int i = 0; i < gm.transform.childCount; i++)
        {
            Destroy(gm.transform.GetChild(i).gameObject);
        }
    }

    void DeleteAllNumbers ()
    {
        for (int i = 0; i < canvasWithNumbers.transform.childCount; i++)
        {
            Destroy(canvasWithNumbers.transform.GetChild(i).gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DeleteAllCells();
        DeleteAllNumbers();
        gameManager.GameOverDisappears();
        gameManager.MainMenuAppears();
    }


    void Start () {
		gameManager = gm.GetComponent<GameManager>();
    }

}
