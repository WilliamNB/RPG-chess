using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Specialized;

public class Cell : MonoBehaviour
{

    public Image mOutLineImage;
    [HideInInspector]
    public Vector2Int mBoardPos;
    [HideInInspector]
    public Board mBoard = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;

    public void setup(Vector2Int newBoardPos, Board newBoard)
    {
        mBoardPos = newBoardPos;
        mBoard = newBoard;

        mRectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
