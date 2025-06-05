using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TileSize
{
    small,
    medium,
    large
}

public enum ColorPalette
{
    standard
}

public enum StandardPalette
{
    yellow,
    red,
    magenta,
    blue,
    cyan,
    green
}

public class ColorGame : MonoBehaviour
{
    [SerializeField]
    int size;

    [SerializeField]
    TileSize tileSize = TileSize.medium;

    [SerializeField]
    ColorPalette colorPalette;

    int numRows;
    int numCols;
    int numMoves = 24;

    [SerializeField]
    GameObject startMenu;
    [SerializeField]
    GameObject colorBoard;
    [SerializeField]
    GameObject colorSwitches;

    [SerializeField]
    static GameObject[,] floodedTiles;

    [SerializeField]
    GameObject remainingMoves;

    StandardPalette selectedColor;

    //public TMP_Text TextComponent;

    public const int YELLOW = 0, COLOR_0 = 0;
    public const int RED = 1, COLOR_1 = 1;
    public const int MAGENTA = 2, COLOR_2 = 2;
    public const int BLUE = 3, COLOR_3 = 3;
    public const int CYAN = 4, COLOR_4 = 4;
    public const int GREEN = 5, COLOR_5 = 5;


    public void StartGame()
    {
        Debug.Log("Start Game");
        GenerateBoard();
    }

    void GenerateBoard()
    {
        Debug.Log("Generating " + tileSize + " Board");
        if (tileSize == TileSize.small)
        {
            size = 9;
        }
        if (tileSize == TileSize.medium)
        {
            size = 12;
        }
        if (tileSize == TileSize.large)
        {
            size = 14;
        }
        colorSwitches.SetActive(true);
        colorBoard.SetActive(true);
        colorBoard.GetComponent<GridLayoutGroup>().constraintCount = size;
        Debug.Log(colorBoard.GetComponent<GridLayoutGroup>().constraintCount);
        colorBoard.GetComponent<ColorBoard>().CreateBoard(size, colorPalette);
        remainingMoves.GetComponent<TMPro.TextMeshProUGUI>().SetText(numMoves.ToString());
        //ColorBoard board = new ColorBoard(size, colorPalette);
    }

    public void SetTileSizeSmall()
    {
        tileSize = TileSize.small;
    }
    public void SetTileSizeMedium()
    {
        tileSize = TileSize.medium;
    }
    public void SetTileSizeLarge()
    {
        tileSize = TileSize.large;
    }

    public void SelectColor(int buttonIndex)
    {
        
        if (buttonIndex == 0)
        {
            selectedColor = StandardPalette.yellow;
        }
        else if(buttonIndex == 1)
        {
            selectedColor = StandardPalette.red;
        }
        else if (buttonIndex == 2)
        {
            selectedColor = StandardPalette.magenta;
        }
        else if (buttonIndex == 3)
        {
            selectedColor = StandardPalette.blue;
        }
        else if (buttonIndex == 4)
        {
            selectedColor = StandardPalette.cyan;
        }
        else if (buttonIndex == 5)
        {
            selectedColor = StandardPalette.green;
        }
        FloodColor(selectedColor);
        DeductRemainingMoves();
    }

    void CloseGame()
    {
        Debug.Log("Close Game");
        ResetGame();
        colorBoard.GetComponent<ColorBoard>().ResetBoard();
        colorBoard.SetActive(false);
        startMenu.SetActive(true);
        colorSwitches.SetActive(false);
    }

    void ResetGame()
    {
        numMoves = 24;
        remainingMoves.GetComponent<TMPro.TextMeshProUGUI>().SetText("");
    }

    void DeductRemainingMoves()
    {
        Debug.Log("Deducting Move");
        --numMoves;
        remainingMoves.GetComponent<TMPro.TextMeshProUGUI>().SetText(numMoves.ToString());
        if (numMoves == 0)
        {
            colorBoard.GetComponent<ColorBoard>().EndGame(false);
            CloseGame();
        }
        else if(colorBoard.GetComponent<ColorBoard>().IsGameOver())
        {
            CloseGame();
        }
    }

    void FloodColor(StandardPalette color)
    {

        colorBoard.GetComponent<ColorBoard>().FloodBoard(color);

    }

}
