using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnScript : PieceBase
{
    [SerializeField]private bool firstMove = true;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO - Override MovePiece for pawns
    public override List<string> GenerateMoves(GameObject piece, GameObject currentSquare) {
        List<string> legalMoves = new();
        List<string> captureMoves = new();
        return legalMoves;

    }
    
}
