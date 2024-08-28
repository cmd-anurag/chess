using System.Collections.Generic;
using UnityEngine;

public abstract class PieceBase : MonoBehaviour, PieceInterface {
    public Transform CurrentSquare {get; set;}
    public bool IsWhite {get; set;}
    public GameLogicManagerScript gameLogicManagerScript;
    protected BoardScript boardScript;

    protected virtual void Start() {
        gameLogicManagerScript = GameObject.FindGameObjectWithTag("GameLogicManagerTag").GetComponent<GameLogicManagerScript>();
        boardScript = GameObject.FindGameObjectWithTag("BoardTag").GetComponent<BoardScript>();
    }


    public abstract List<string> GenerateMoves(GameObject piece, GameObject currentSquare);
    public virtual bool MovePiece(Transform piece, Transform targetSquare) {

        PieceBase pieceBase = piece.GetComponent<PieceBase>();
        SquareScript originalSquareScript = pieceBase.CurrentSquare.gameObject.GetComponent<SquareScript>();
        SquareScript targetSquareScript = targetSquare.gameObject.GetComponent<SquareScript>();

        List<string> legalMoves = pieceBase.GenerateMoves(piece.gameObject, CurrentSquare.gameObject);
        if(!legalMoves.Contains(targetSquare.name)) {
            Debug.Log("Invalid Move");
            return false;
        }


        if(targetSquareScript.occupiedBy==null) {
            piece.transform.position = targetSquare.position;

            originalSquareScript.occupiedBy = null;
            targetSquareScript.occupiedBy = piece.gameObject;

            Debug.Log($"{piece.name} moved from {pieceBase.CurrentSquare.name} to {targetSquare.name}");
            pieceBase.CurrentSquare = targetSquare;
        }
        else {
            // logging variable
            Transform oldSquare = pieceBase.CurrentSquare;
            bool captureSuccesful = gameLogicManagerScript.CapturePiece(piece.gameObject, targetSquareScript.occupiedBy);
            if(captureSuccesful) {
                Debug.Log($"{piece.gameObject.name} moved from {oldSquare.name} and captured {targetSquareScript.occupiedBy.name} on {targetSquare.name}");
                originalSquareScript.occupiedBy = null;
                targetSquareScript.occupiedBy = piece.gameObject;
            }
        }
        gameLogicManagerScript.ClearAllHighlights();
        return true;

    }
}
