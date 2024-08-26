using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : MonoBehaviour, PieceInterface
{
    public Transform CurrentSquare {get; set;}
    public bool IsWhite {get; set;}
    public GameLogicManagerScript gameLogicManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameLogicManagerScript = GameObject.FindGameObjectWithTag("GameLogicManagerTag").GetComponent<GameLogicManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool MovePiece(Transform piece, Transform targetSquare) {

        PieceInterface pieceInterface = piece.GetComponent<PieceInterface>();
        SquareScript originalSquareScript = pieceInterface.CurrentSquare.gameObject.GetComponent<SquareScript>();
        SquareScript targetSquareScript = targetSquare.gameObject.GetComponent<SquareScript>();

        if(targetSquareScript.occupiedBy==null) {
            piece.transform.position = targetSquare.position;

            originalSquareScript.occupiedBy = null;
            targetSquareScript.occupiedBy = piece.gameObject;

            Debug.Log($"{piece.name} moved from {pieceInterface.CurrentSquare} to {targetSquare}");
            pieceInterface.CurrentSquare = targetSquare;
        }
        else {
            bool captureSuccesful = gameLogicManagerScript.CapturePiece(piece.gameObject, targetSquareScript.occupiedBy);
            if(captureSuccesful) {
                originalSquareScript.occupiedBy = null;
                targetSquareScript.occupiedBy = piece.gameObject;
            }
        }
        return true;

    }
}
