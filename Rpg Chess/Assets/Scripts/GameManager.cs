﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;

    public PieceManager pieceManager;

    // Start is called before the first frame update
    void Start()
    {
        mBoard.create();
        pieceManager.Setup(mBoard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
