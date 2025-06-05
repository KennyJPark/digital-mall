using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBoard : MonoBehaviour
{
    [SerializeField]
    //static ColorTile[,] colorGrid;
    static GameObject[,] colorGrid;
    //ColorTile[][] colorGrid;
    public GameObject colorTilePrefab;
    ColorPalette colorPalette;

    int boardSize;

    public GameObject colorSwitches;

    bool gameOver = false;

    public void CreateBoard(int size, ColorPalette cp)
    {
        boardSize = size;
        Debug.Log("Board Size: " + boardSize);
        //colorGrid = new ColorTile[size, size];
        colorGrid = new GameObject[size, size];

        colorPalette = cp;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                //colorGrid[i][j] = new ColorTile();
                //colorGrid[i, j] = new ColorTile();
                colorGrid[i, j] = Instantiate(colorTilePrefab, this.gameObject.transform);
                colorGrid[i, j].GetComponent<ColorTile>().RandomizeColor();
                //Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
                Debug.Log("(" + i + ", " + j + ") = " + colorGrid[i,j].GetComponent<ColorTile>().color);
            }

        }
        colorGrid[0, 0].GetComponent<ColorTile>().SetFlooded();
        Percolate(0, 0);
        //FloodBoard(colorGrid[0, 0].GetComponent<ColorTile>().GetColor());
        //ColorTile colorGrid = 
    }

    public bool IsTileFlooded(int row, int col)
    {
        
        return colorGrid[row, col].GetComponent<ColorTile>().IsFlooded();
    }

    public StandardPalette GetTileColor(int row, int col)
    {
        return colorGrid[row, col].GetComponent<ColorTile>().GetColor();
    }

    void Percolate(int row, int col)
    {
        Debug.Log("Percolating: [" + row + ", " + col + "]");
        StandardPalette startingColor = colorGrid[row, col].GetComponent<ColorTile>().GetColor();
        if (col - 1 >= 0 && startingColor == colorGrid[row, col - 1].GetComponent<ColorTile>().GetColor() && !colorGrid[row, col - 1].GetComponent<ColorTile>().IsFlooded())
        {
            colorGrid[row, col - 1].GetComponent<ColorTile>().SetFlooded();
            if(CheckAdjacentTileFlooded(row, col - 1))
                Percolate(row, col - 1);
        }
        if (row - 1 >= 0 && startingColor == colorGrid[row - 1, col].GetComponent<ColorTile>().GetColor() && !colorGrid[row - 1, col].GetComponent<ColorTile>().IsFlooded())
        {
            colorGrid[row - 1, col].GetComponent<ColorTile>().SetFlooded();
            if (CheckAdjacentTileFlooded(row - 1, col))
                    Percolate(row-1, col);
        }
        if (col + 1 < boardSize && startingColor == colorGrid[row, col + 1].GetComponent<ColorTile>().GetColor() && !colorGrid[row, col + 1].GetComponent<ColorTile>().IsFlooded())
        {
            colorGrid[row, col + 1].GetComponent<ColorTile>().SetFlooded();
            if (CheckAdjacentTileFlooded(row, col + 1))
                Percolate(row, col+1);
        }
        if (row + 1 < boardSize && startingColor == colorGrid[row + 1, col].GetComponent<ColorTile>().GetColor() && !colorGrid[row + 1, col].GetComponent<ColorTile>().IsFlooded())
        {
            colorGrid[row + 1, col].GetComponent<ColorTile>().SetFlooded();
            if (CheckAdjacentTileFlooded(row + 1, col))
                Percolate(row + 1, col);
        }
    }

    public bool CheckAdjacentTileFlooded(int row, int col)
    {
        Debug.Log("[" + row + " , " + col + "]");
        // Top Left
        // Should only be used at the start of the game
        if (row == 0 && col == 0)
        {
            if (IsTileFlooded(row, col + 1) || IsTileFlooded(row + 1, col))
            {
                return true;
            }

        }        
        // Top
        else if (row == 0 && col > 0 && col < boardSize - 1)
        {
            if (IsTileFlooded(row, col - 1) || IsTileFlooded(row + 1, col) || IsTileFlooded(row, col + 1))
            {
                return true;
            }
        }
        // Top Right
        else if (row == 0 && col == boardSize - 1)
        {
            if (IsTileFlooded(row + 1, col) || IsTileFlooded(row, col - 1))
            {
                return true;
            }
        }
        // Right
        else if (row > 0 && row < boardSize - 1 && col == boardSize-1)
        {
            if (IsTileFlooded(row, col - 1) || IsTileFlooded(row - 1, col) || IsTileFlooded(row + 1, col))
            {
                return true;
            }
        }
        // Bottom Right
        else if (row == boardSize - 1 && col == boardSize - 1)
        {
            if (IsTileFlooded(row, col - 1) || IsTileFlooded(row - 1, col))
            {
                return true;
            }
        }
        // Bottom
        else if (row == boardSize - 1 && col > 0 && col < boardSize - 1)
        {
            if (IsTileFlooded(row, col - 1) || IsTileFlooded(row - 1, col) || IsTileFlooded(row, col + 1))
            {
                return true;
            }
        }
        // Bottom Left
        else if (row == boardSize - 1 && col == 0)
        {
            if (IsTileFlooded(row - 1, col) || IsTileFlooded(row, col + 1))
            {
                return true;
            }
        }
        // Left
        else if (row > 0 && row < boardSize - 1 && col == 0)
        {
            if (IsTileFlooded(row - 1, col) || IsTileFlooded(row, col + 1) || IsTileFlooded(row + 1, col))
            {
                return true;
            }
        }
        // Center
        else
        {
            if (IsTileFlooded(row, col - 1) || IsTileFlooded(row - 1, col) || IsTileFlooded(row, col + 1) || IsTileFlooded(row + 1, col))
            {
                return true;
            }
        }
        return false;
    }

    bool CheckIfBoardIsUniform()
    {
        StandardPalette color = colorGrid[0, 0].GetComponent<ColorTile>().GetColor();
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if(color != colorGrid[i, j].GetComponent<ColorTile>().GetColor())
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void FloodTile(int row, int col, StandardPalette color)
    {
        colorGrid[row, col].GetComponent<ColorTile>().SetFlooded();
        colorGrid[row, col].GetComponent<ColorTile>().SetColor(color);
        
    }

    public void CheckWinCondition()
    {
        if (CheckIfBoardIsUniform())
        {
            EndGame(true);
        }
    }

    public void EndGame(bool playerWins)
    {
        if(playerWins)
        {
            Debug.Log("You win!");
            gameOver = true;
        }
        else
        {
            Debug.Log("No more moves left.. Game over..");
        }
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void FloodBoard(StandardPalette color)
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                // Ignore tiles that are already flooded
                if (!IsTileFlooded(i, j))
                {
                    // Check if the tile's color matches the color we are about to flood with
                    // If it does, make sure that the tile is adjacent to a flooded tile
                    if (color == GetTileColor(i, j) && CheckAdjacentTileFlooded(i, j))
                    {
                        colorGrid[i, j].GetComponent<ColorTile>().SetFlooded();
                    }
                }
            }
        }

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (IsTileFlooded(i, j))
                {
                    FloodTile(i, j, color);
                }
            }
        }

        CheckWinCondition();
    }

    public void ResetBoard()
    {

    }
}
