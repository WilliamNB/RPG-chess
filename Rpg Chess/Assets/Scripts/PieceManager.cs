using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{

    public GameObject mPiecePrefab;

    private List<BasePiece> mWhitePieces;
    private List<BasePiece> mBlackPieces;


    private string[] mPieceOrder = new string[16]
    {
        "p", "p", "p", "p", "p", "p", "p", "p",
        "r", "kn", "b", "k", "q", "b", "kn", "r"
    };

    private Dictionary<string, Type> pieceLibary = new Dictionary<string, Type>()
    {
        {"p", typeof(Pawn) },
        {"r", typeof(Rook) },
        {"b", typeof(Bishop) },
        {"kn", typeof(Knight) },
        {"q", typeof(Queen) },
        {"k", typeof(King) },
    };

    public void Setup(Board board)
    {
        mWhitePieces = CreatePieces(Color.white, new Color32(80, 124, 159, 255), board);
        mBlackPieces = CreatePieces(Color.black, new Color32(210, 95, 64, 255), board);

        PlacePieces(1, 0, mWhitePieces, board);
        PlacePieces(6, 7, mBlackPieces, board); 
    }

    private List<BasePiece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<BasePiece> newPieces = new List<BasePiece>();
        for(int i = 0; i < mPieceOrder.Length; i++)
        {
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(transform);

            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;

            string key = mPieceOrder[i];
            Type pieceType = pieceLibary[key];

            BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);
            newPieces.Add(newPiece);

            newPiece.Setup(teamColor, spriteColor, this);


        }

        return newPieces;
    }

    private void PlacePieces(int pawnRow, int royalRow, List<BasePiece> pieces, Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            pieces[i].Place(board.mAllCells[i, pawnRow]);
            pieces[i + 8].Place(board.mAllCells[i, royalRow]);
        }
    }

}
