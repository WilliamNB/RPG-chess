using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BasePiece: MonoBehaviour
{
    [HideInInspector]
    public Color mColor = Color.clear; 

    protected Cell originalCell = null;
    protected Cell currentCell = null;

    RectTransform mRectTransform = null;
    PieceManager mPieceManager;

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

}
