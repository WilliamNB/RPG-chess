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

    public override void SelectUpgrade()
    {
        if (true)
        {
            // vertical
            branch = 1;
            mMovement2 = new Vector3Int(0, 0, 7);
        }
    }
    public override void FinalUpgrade()
    {
        switch (branch)
        {
            case 1:
                mMovement = new Vector3Int(0, 7, 7);
                break;
            case 2:
                mMovement = new Vector3Int(7, 0, 7);
                break;
        }

    }

}
