using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BasePiece: EventTrigger
{
    [HideInInspector]
    public Color mColor = Color.clear; 

    protected Cell originalCell = null;
    protected Cell currentCell = null;
    protected Cell targetCell = null;

    RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    protected Vector3Int mMovement = Vector3Int.one;
    protected Vector3Int mMovement2 = Vector3Int.one;
    protected List<Cell> mHighLightedCells = new List<Cell>();

    protected int level = 1;
    protected int branch = 0;

    //PieceMangare not createpiece
    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;
        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void Place(Cell newCell)
    {
        originalCell = newCell;
        currentCell = newCell;
        currentCell.mCurrentPiece = this;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }
    public void Reset()
    {
        Kill();

        Place(originalCell);
    }

    public virtual void Kill()
    {
        currentCell.mCurrentPiece = null;
        gameObject.SetActive(false);
    }

    private void CreateCellPath(int xDirection, int yDirection, int movement) 
    {
        int currentXPos = currentCell.mBoardPos.x;
        int currentYPos = currentCell.mBoardPos.y;

        for(int i = 1; i <= movement; i++)
        {
            currentXPos += xDirection;
            currentYPos += yDirection;

            //GET STATE OF TARGET CELL
            CellSate cellState = CellSate.None;
            cellState = currentCell.mBoard.ValidateCell(currentXPos, currentYPos, this);

            if(cellState == CellSate.Enemy)
            {
                mHighLightedCells.Add(currentCell.mBoard.mAllCells[currentXPos, currentYPos]);
                break;
            }

            if(cellState != CellSate.Free)
            {
                break;
            }


            mHighLightedCells.Add(currentCell.mBoard.mAllCells[currentXPos, currentYPos]);
        }


    }

    protected void CreateCellPathLv2(int xDirection, int yDirection, int movement)
    {
        int currentXPos = currentCell.mBoardPos.x;
        int currentYPos = currentCell.mBoardPos.y;

        for (int i = 1; i <= movement; i++)
        {
            currentXPos += xDirection;
            currentYPos += yDirection;

            //GET STATE OF TARGET CELL
            CellSate cellState = CellSate.None;
            cellState = currentCell.mBoard.ValidateCell(currentXPos, currentYPos, this);

            if (cellState != CellSate.Free)
            {
                break;
            }


            mHighLightedCells.Add(currentCell.mBoard.mAllCells[currentXPos, currentYPos]);
        }
    }

    protected virtual void CheckPathing()
    {
        // horizontal
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);
        //vertical
        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);
        //upper diagonal
        CreateCellPath(1, 1, mMovement.z);
        CreateCellPath(-1, 1, mMovement.z);
        //lower diagonal
        CreateCellPath(-1, -1, mMovement.z);
        CreateCellPath(1, -1, mMovement.z);

    }

    private bool MatchesState(int targetX, int targetY, CellSate targetState)
    {
        CellSate cellstate = CellSate.None;
        cellstate = currentCell.mBoard.ValidateCell(targetX, targetY, this);

        if (cellstate == targetState)
        {
            mHighLightedCells.Add(currentCell.mBoard.mAllCells[targetX, targetY]);
            return true;
        }
        return false;
    }

    protected void ShowCells()
    {
        foreach (Cell cell in mHighLightedCells)
        {
            cell.mOutLineImage.enabled = true;
        }
    }

    protected void ClearCells()
    {
        foreach (Cell cell in mHighLightedCells)
        {
            cell.mOutLineImage.enabled = false;
        }
        mHighLightedCells.Clear();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("piece level " + level);
        base.OnBeginDrag(eventData);
        //test for cells
        CheckPathing();
        //show cells
        ShowCells();
    }

    public virtual void Move()
    {
        //if enemy piece, remove & level up piece
        CellSate takenCell = targetCell.mBoard.ValidateCell(targetCell.mBoardPos.x, targetCell.mBoardPos.y, targetCell.mCurrentPiece);
        targetCell.RemovePiece();
        if (takenCell == CellSate.Friendly)  // Friendly cause it compares the piece color to the color of the piece on the square
         {
             currentCell.mCurrentPiece.LevelUp();
         }

        //clear current cell
        currentCell.mCurrentPiece = null;
        //switch cells
        currentCell = targetCell;
        currentCell.mCurrentPiece = this;
        //move on board
        transform.position = currentCell.transform.position;
        targetCell = null;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        //follow pointer
        transform.position += (Vector3)eventData.delta;
        //check for overlapping aviable squares
        foreach(Cell cell in mHighLightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                targetCell = cell;
                break;
            }
            // if mouse is not in a highlighted cell we dont have a valid move
            targetCell = null;
        }
        if (Input.GetKeyDown("k"))
        {
            Kill();
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        ClearCells();

        //return cell to original pos if no target cell
        if (!targetCell)
        {
            transform.position = currentCell.gameObject.transform.position;
            return;
        }

        Move();

        //end turn

        mPieceManager.SwitchSides(mColor);
    }

    //////////////////////////////////////// my stuff
    
    public int GetLevel()
    {
        return level;
    }

    public void LevelUp()
    {
        if(level < 3)
        {
            level += 1;
               
            if (level == 2)
            {
                SelectUpgrade();
            }
            if(level == 3)
            {
                FinalUpgrade();
            }

        }
    }
    
    public virtual void SelectUpgrade()
    {
        //mMovement = new Vector3Int(7, 7, 7);
    }

    public virtual void FinalUpgrade()
    {

    }
    
}
