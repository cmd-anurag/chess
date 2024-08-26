using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManagerScript : MonoBehaviour
{
    public bool isWhiteTurn;

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
}
