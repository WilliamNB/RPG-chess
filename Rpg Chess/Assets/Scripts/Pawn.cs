using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    private bool firstMove = true;
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        //reset
        firstMove = true;

        //pawn stuff
        mMovement = mColor == Color.white ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Pawn");
    }

    public override void Move()
    {
        base.Move();

        firstMove = false;
    }
    
    private bool MatchesState(int targetX, int targetY, CellSate targetState)
    {
        CellSate cellstate = CellSate.None;
        cellstate = currentCell.mBoard.ValidateCell(targetX, targetY, this);

        if(cellstate == targetState)
        {
            mHighLightedCells.Add(currentCell.mBoard.mAllCells[targetX, targetY]);
            return true;
        }
        return false;
    }
    
    protected override void CheckPathing()
    {
        int currentX = currentCell.mBoardPos.x;
        int currentY = currentCell.mBoardPos.y;

        MatchesState(currentX - mMovement.z, currentY + mMovement.z, CellSate.Enemy);

        if(MatchesState(currentX, currentY + mMovement.y, CellSate.Free))
        {
            if (firstMove)
            {
                MatchesState(currentX, currentY + (mMovement.y * 2), CellSate.Free);
            }
        }

        MatchesState(currentX + mMovement.z, currentY + mMovement.z, CellSate.Enemy);
    }
}
