using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject canv;
    public Dropdown ddown;

    bool mainMenuFadeOut = false;
    bool mainMenuFadeIn = false;
    float velocityForAlpha = 0.0f;

    bool gameOverFadeIn = false;
    bool gameOverFadeOut = false;
    float velocityForAlphaGameOver = 0.0f;
    public GameObject gameOverScreen;
    public GameObject gui;
    GameObject rawImage1;    // game over screen colors and text
    GameObject rawImage2;    //
    GameObject gameOverText; //

    public Text difText;
    public Slider slider;

    public void OnButtonNewGameClick()
    {
        FieldOperator fc = gameObject.GetComponent<FieldOperator>();
        switch ((int)slider.value)
        {
            case 0:
                fc.InitializeMe(10, 10, 9, 0);
                fc.StartNewGame();
                break;
            case 1:
                fc.InitializeMe(15, 15, 35, 1);
                fc.StartNewGame();
                break;
            case 2:
                fc.InitializeMe(20, 20, 50, 2);
                fc.StartNewGame();
                break;
        }


        mainMenuFadeOut = true;
        gui.SetActive(true);
    }

    public void GameOverAppears ()
    {
        gameOverScreen.SetActive(true);

        rawImage1 = gameOverScreen.transform.Find("RawImage").gameObject;
        if (rawImage1 != null)
        {
            rawImage1.GetComponent<RawImage>().color = new Color32(116, 22, 22, 240);
        }
        rawImage2 = gameOverScreen.transform.Find("Image").gameObject;
        if (rawImage2 != null)
        {
            rawImage2.GetComponent<Image>().color = new Color32(111, 22, 22, 255);
        }
        gameOverText = gameOverScreen.transform.Find("Text").gameObject;
        if (gameOverText != null)
        {
            gameOverText.GetComponent<Text>().text = "GAME OVER \n click to play again";
        }

        gameOverFadeIn = true;
        gui.SetActive(false);
    }

    public void WinGameOverAppears()
    {
        gameOverScreen.SetActive(true);

        rawImage1 = gameOverScreen.transform.Find("RawImage").gameObject;
        if (rawImage1 != null)
        {
            rawImage1.GetComponent<RawImage>().color = new Color32(248, 248, 238, 230);
        }
        rawImage2 = gameOverScreen.transform.Find("Image").gameObject;
        if (rawImage2 != null)
        {
            rawImage2.GetComponent<Image>().color = new Color32(246, 150, 88, 255);
        }
        gameOverText = gameOverScreen.transform.Find("Text").gameObject;
        if (gameOverText != null)
        {
            gameOverText.GetComponent<Text>().text = "YOU WIN";
        }

        gameOverFadeIn = true;
        gui.SetActive(false);
    }

    public void GameOverDisappears()
    {
        gameOverFadeOut = true;
    }

    public void MainMenuAppears ()
    {
        canv.SetActive(true);
        mainMenuFadeIn = true;
    }

	
	void Update () {
        if (mainMenuFadeOut)
        {
            canv.GetComponent<CanvasGroup>().alpha = Mathf.SmoothDamp(canv.GetComponent<CanvasGroup>().alpha, 0, ref velocityForAlpha, 0.2f);
            if (canv.GetComponent<CanvasGroup>().alpha <= 0.1)
            {
                mainMenuFadeOut = false;
                canv.SetActive(false); 
            }
        }

        if (gameOverFadeIn)
        {
            gameOverScreen.GetComponent<CanvasGroup>().alpha = Mathf.SmoothDamp(gameOverScreen.GetComponent<CanvasGroup>().alpha, 1, ref velocityForAlphaGameOver, 0.3f);
            if (gameOverScreen.GetComponent<CanvasGroup>().alpha >= 0.90)
            {
                gameOverFadeIn = false;
            }
        }

        if (gameOverFadeOut)
        {
            gameOverScreen.GetComponent<CanvasGroup>().alpha = Mathf.SmoothDamp(gameOverScreen.GetComponent<CanvasGroup>().alpha, 0, ref velocityForAlphaGameOver, 0.3f);
            if (gameOverScreen.GetComponent<CanvasGroup>().alpha <= 0.1)
            {
                gameOverFadeOut = false;
                gameOverScreen.SetActive(false);
            }
        }

        if (mainMenuFadeIn)
        {
            canv.GetComponent<CanvasGroup>().alpha = Mathf.SmoothDamp(canv.GetComponent<CanvasGroup>().alpha, 1, ref velocityForAlpha, 0.2f);
            if (canv.GetComponent<CanvasGroup>().alpha >= 0.95)
            {
                mainMenuFadeIn = false;
            }
        }
    }
    
    public void SliderControl()
    {
        switch ((int)slider.value)
        {
            case 0: difText.text = "Easy"; break;
            case 1: difText.text = "Medium"; break;
            case 2: difText.text = "Hard"; break;
        }
        
    }

    private void Start()
    {
        //canv.SetActive(true);
    }
}
