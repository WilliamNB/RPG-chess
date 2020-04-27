using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);


        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Knight");
    }

    private void CreateCellPath(int flipper)
    {
        int currentX = currentCell.mBoardPos.x;
        int currentY = currentCell.mBoardPos.y;
        //left
        MatchesState(currentX - 2, currentY + (flipper * 1));
        //right
        MatchesState(currentX + 2, currentY + (flipper * 1));
        //upper left
        MatchesState(currentX - 1, currentY + (flipper * 2));
        //upper right
        MatchesState(currentX + 1, currentY + (flipper * 2));
    }

    protected override void CheckPathing()
    {
        CreateCellPath(1);
        CreateCellPath(-1);
    }

    private void MatchesState(int targetX, int targetY)
    {
        CellSate cellState = CellSate.None;
        cellState = currentCell.mBoard.ValidateCell(targetX, targetY, this);

        if(cellState != CellSate.Friendly && cellState != CellSate.OutOfBounds)
        {
            mHighLightedCells.Add(currentCell.mBoard.mAllCells[targetX, targetY]);
        }

    }
}
