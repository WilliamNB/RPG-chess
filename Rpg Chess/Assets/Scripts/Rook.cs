using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rook : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        //rook stuff
        mMovement = new Vector3Int(7, 7, 0);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Rook");
    }

    protected override void CheckPathing()
    {
        // horizontal
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);
        //vertical
        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);
        //upper diagonal
        if (branch == 1)
        {
            CreateCellPath(1, 1, mMovement.z);
            CreateCellPath(-1, 1, mMovement.z);
        }
        //lower diagonal
        if (branch == 2)
        {
            CreateCellPath(-1, -1, mMovement.z);
            CreateCellPath(1, -1, mMovement.z);
        }

        if (level == 2)
        {
            //upper diagonal
            if (branch == 1)
            {
                CreateCellPathLv2(1, 1, mMovement2.z);
                CreateCellPathLv2(-1, 1, mMovement2.z);
            }
            //lower diagonal
            if (branch == 2)
            {
                CreateCellPathLv2(-1, -1, mMovement2.z);
                CreateCellPathLv2(1, -1, mMovement2.z);
            }
        }
    }

    public override void SelectUpgrade()
    {
        if (true)
        {
            // upper
            branch = 1;
            mMovement2 = new Vector3Int(0, 0, 7);
        }else if (false)
        {
            // lower
            branch = 2;
            mMovement2 = new Vector3Int(0, 0, 7);
        }
    }
    public override void FinalUpgrade()
    {
        mMovement = new Vector3Int(7, 7, 7);
    }

}
