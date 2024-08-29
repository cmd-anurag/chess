using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManagerScript : MonoBehaviour
{
    public bool isWhiteTurn;
    public List<GameObject> highlightedSquares = new();
    // Start is called before the first frame update
    void Start()
    {
        isWhiteTurn = true;
        Debug.Log("Game started. White's turn: " + isWhiteTurn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckTurn(GameObject piece) {
        bool isWhite = piece.GetComponent<PieceInterface>().IsWhite;
        if((isWhite && isWhiteTurn) || (!isWhite && !isWhiteTurn)) {
            return true;
        }
        else {
            return false;
        }
    }
    public void SwitchTurn() {
        isWhiteTurn = !isWhiteTurn;
    }

    public bool CapturePiece(GameObject selectedPiece, GameObject targetPiece) {

       PieceInterface selectedPieceInterface = selectedPiece.GetComponent<PieceInterface>();
       PieceInterface targetPieceInterface = targetPiece.GetComponent<PieceInterface>();

       if((selectedPieceInterface.IsWhite && targetPieceInterface.IsWhite) || (!selectedPieceInterface.IsWhite && !targetPieceInterface.IsWhite)) {
            return false;
       }
       else {
            selectedPiece.transform.position = targetPiece.transform.position;
            selectedPieceInterface.CurrentSquare = targetPieceInterface.CurrentSquare;
            Destroy(targetPiece);
            return true;
       }
    }
    public void HighLightSquare(GameObject square) {
        highlightedSquares.Add(square);
        Color highlightColor = new(80/255f, 200/255f, 120/255f);
        square.GetComponent<SpriteRenderer>().color = highlightColor;
    }

    public void HighlightCapture(GameObject square) {
        highlightedSquares.Add(square);
        square.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void ClearAllHighlights() {
        SpriteRenderer renderer;
        Color colorDark = new(112/255f, 102/255f, 119/255f);
        Color colorLight = new(204/255f, 183/255f, 174/255f);

        foreach (GameObject square in highlightedSquares) {
            renderer = square.GetComponent<SpriteRenderer>();
            char file = square.name[0];
            char rank = square.name[1];
            renderer.color = (file - 'a' + 1 + rank) % 2 == 0? colorLight : colorDark;
        }
        highlightedSquares.Clear();
    }
}
