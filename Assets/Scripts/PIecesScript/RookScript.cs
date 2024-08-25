using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookScript : MonoBehaviour, PieceInterface
{
    public Transform CurrentSquare {get; set;}
    public bool isWhite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool MovePiece(Transform piece, Transform targetSquare) {
        piece.transform.position = targetSquare.position;
        PieceInterface pieceInterface = piece.GetComponent<PieceInterface>();
        Debug.Log($"{piece.name} moved from {pieceInterface.CurrentSquare} to {targetSquare}");
        pieceInterface.CurrentSquare = targetSquare;
        return true;
    }
}
