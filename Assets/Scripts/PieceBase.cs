using UnityEngine;

public abstract class PieceBase : MonoBehaviour, PieceInterface {
    public Transform CurrentSquare {get; set;}
    public bool IsWhite {get; set;}
    public GameLogicManagerScript gameLogicManagerScript;
    public virtual bool MovePiece(Transform piece, Transform targetSquare) {

        PieceInterface pieceInterface = piece.GetComponent<PieceInterface>();
        SquareScript originalSquareScript = pieceInterface.CurrentSquare.gameObject.GetComponent<SquareScript>();
        SquareScript targetSquareScript = targetSquare.gameObject.GetComponent<SquareScript>();

        if(targetSquareScript.occupiedBy==null) {
            piece.transform.position = targetSquare.position;

            originalSquareScript.occupiedBy = null;
            targetSquareScript.occupiedBy = piece.gameObject;

            Debug.Log($"{piece.name} moved from {pieceInterface.CurrentSquare.name} to {targetSquare.name}");
            pieceInterface.CurrentSquare = targetSquare;
        }
        else {
            // logging variable
            Transform oldSquare = pieceInterface.CurrentSquare;
            bool captureSuccesful = gameLogicManagerScript.CapturePiece(piece.gameObject, targetSquareScript.occupiedBy);
            if(captureSuccesful) {
                Debug.Log($"{piece.gameObject.name} moved from {oldSquare.name} and captured {targetSquareScript.occupiedBy.name} on {targetSquare.name}");
                originalSquareScript.occupiedBy = null;
                targetSquareScript.occupiedBy = piece.gameObject;
            }
        }
        return true;

    }
}
