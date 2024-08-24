using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopScript : MonoBehaviour, PieceInterface
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
        return true;
    }
}
