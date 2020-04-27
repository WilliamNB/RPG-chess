using System;
using System.Collections.Generic;
using UnityEngine;

public class CreatePiece : MonoBehaviour
{
    public GameObject mPiecePrefab;
    public string pt;
    public int row;
    public int coloum;
   
    private BasePiece testPiece;




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
       // mWhitePieces = CreatePieces(Color.white, new Color32(80, 124, 159, 255), board);
       //mBlackPieces = CreatePieces(Color.black, new Color32(210, 95, 64, 255), board);

        testPiece = CreatePieces(Color.black, new Color32(210, 95, 64, 255), board);

        PlacePieces(row, coloum, testPiece, board);
        //PlacePieces(6, 7, mBlackPieces, board);


    }
    
    private BasePiece CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
       // List<BasePiece> newPieces = new List<BasePiece>();
        //for (int i = 0; i < mPieceOrder.Length; i++)
        //{
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(transform);

            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;

          //  string key = "p";
            Type pieceType = pieceLibary[pt];

            BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);
            PieceManager pieceManager = new PieceManager();
            newPiece.Setup(teamColor, spriteColor, pieceManager);


       // }

        return newPiece;
    }
    
    private void PlacePieces(int row, int coloum, BasePiece piece, Board board)
    {
            piece.Place(board.mAllCells[coloum, row]);
    }
    
}
