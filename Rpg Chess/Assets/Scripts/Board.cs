using UnityEngine;
using UnityEngine.UI;

public enum CellSate 
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}


public class Board : MonoBehaviour
{

    public GameObject mCellPrefab;
    [HideInInspector]
    public Cell[,] mAllCells = new Cell[8, 8];

    public void create()
    {
        for(int y = 0; y < 8; y++)
        {
            for(int x = 0; x < 8; x++)
            {
                //create the cell
                GameObject newCell = Instantiate(mCellPrefab, transform);
                //positon
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                //setup
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }

        //colour the board
        for(int y = 0; y < 8; y++)
        {
            for(int x = 0; x < 8; x += 2)
            {
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                mAllCells[finalX, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);
            }
        }
    }

    public CellSate ValidateCell(int targetX, int targetY, BasePiece checkingPiece)
    {
        //bounds check
        if(targetX < 0 || targetX > 7)
        {
            return CellSate.OutOfBounds;
        }
        if (targetY < 0 || targetY > 7)
        {
            return CellSate.OutOfBounds;
        }

        Cell targetCell = mAllCells[targetX, targetY];

        if(targetCell.mCurrentPiece != null)
        {
            if(checkingPiece.mColor == targetCell.mCurrentPiece.mColor)
            {
                return CellSate.Friendly;
            }
            if (checkingPiece.mColor != targetCell.mCurrentPiece.mColor)
            {
                return CellSate.Enemy;
            }
        }

        return CellSate.Free;
    }
}
