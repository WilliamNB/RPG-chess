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
    protected List<Cell> mHighLightedCells = new List<Cell>();

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
        Debug.Log("in create path");
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

    protected virtual void CheckPathing()
    {
        Debug.Log("in check Pathing");
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

    protected void ShowCells()
    {
        Debug.Log("in check show cells");
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
        Debug.Log("in begindrag");
        base.OnBeginDrag(eventData);
        //test for cells
        CheckPathing();
        //show cells
        ShowCells();
    }

    public virtual void Move()
    {
        //if enemy piece, remove
        targetCell.RemovePiece();
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
        Debug.Log("in onDrag");
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

}
