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

    RectTransform mRectTransform = null;
    PieceManager mPieceManager;

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

    private void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        Debug.Log("in create path");
        int currentXPos = currentCell.mBoardPos.x;
        int currentYPos = currentCell.mBoardPos.y;

        for(int i = 1; i <= movement; i++)
        {
            currentXPos += xDirection;
            currentYPos += yDirection;

            //TODO: GET STATE OF TARGET CELL

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

    public override void OnDrag(PointerEventData eventData)
    {
        Debug.Log("in onDrag");
        base.OnDrop(eventData);
        //follow pointer
        transform.position += (Vector3)eventData.delta;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        ClearCells();

    }

}
