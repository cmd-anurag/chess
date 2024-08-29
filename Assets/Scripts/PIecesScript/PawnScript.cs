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
    public override bool MovePiece(Transform piece, Transform targetSquare)
    {
        bool moveSuccessful = base.MovePiece(piece, targetSquare);
        if(moveSuccessful) {
            firstMove = false;
        }
        return moveSuccessful;
    }

    public override List<string> GenerateMoves(GameObject piece, GameObject currentSquare) {
        gameLogicManagerScript.ClearAllHighlights();
        List<string> legalMoves = new() {};
        char file = currentSquare.name[0];
        int rank = currentSquare.GetComponent<SquareScript>().rank;
        

        bool pieceIsWhite = piece.GetComponent<PieceBase>().IsWhite;
        GameObject newSquare, captureSquare1, captureSquare2;

        if(pieceIsWhite) {
            newSquare = boardScript.GetSquareAt(file, rank+1);
            captureSquare1 = boardScript.GetSquareAt((char)(file+1), rank+1);
            captureSquare2 = boardScript.GetSquareAt((char)(file-1), rank+1);
        }
        else {
            newSquare = boardScript.GetSquareAt(file, rank-1);
            captureSquare1 = boardScript.GetSquareAt((char)(file+1), rank-1);
            captureSquare2 = boardScript.GetSquareAt((char)(file-1), rank-1);
        }
        if(newSquare==null) {
            return legalMoves;
        }
        SquareScript newSquareScript = newSquare.GetComponent<SquareScript>();
        GameObject occupiedBy = newSquareScript.occupiedBy;

        if(occupiedBy==null) {
            legalMoves.Add(newSquare.name);
            gameLogicManagerScript.HighLightSquare(newSquare);
        }
        // capturable squares
        occupiedBy = captureSquare1 ? captureSquare1.GetComponent<SquareScript>().occupiedBy : null;
        if(occupiedBy!=null) {
            bool occupiedByWhite = occupiedBy.GetComponent<PieceBase>().IsWhite;
            if((pieceIsWhite && !occupiedByWhite) || (!pieceIsWhite && occupiedByWhite)) {
                gameLogicManagerScript.HighlightCapture(captureSquare1);
                legalMoves.Add(captureSquare1.name);
            }
        }

        occupiedBy = captureSquare2? captureSquare2.GetComponent<SquareScript>().occupiedBy : null;
        if(occupiedBy != null) {
            bool occupiedByWhite = occupiedBy.GetComponent<PieceBase>().IsWhite;
            if((pieceIsWhite && !occupiedByWhite) || (!pieceIsWhite && occupiedByWhite)) {
                gameLogicManagerScript.HighlightCapture(captureSquare2);
                legalMoves.Add(captureSquare2.name);
            }
        }

        if(firstMove) {
            newSquare = pieceIsWhite ? boardScript.GetSquareAt(file, rank+2) : boardScript.GetSquareAt(file, rank-2);
            if(newSquare.GetComponent<SquareScript>().occupiedBy == null) {
                gameLogicManagerScript.HighLightSquare(newSquare);
                legalMoves.Add(newSquare.name);
            }
        }

        return legalMoves;

    }
    
}
