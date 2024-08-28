using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnScript : PieceBase
{
    
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }
    public override List<string> GenerateMoves(GameObject piece, GameObject currentSquare) {
        List<string> legalMoves = new();
        (int, int)[] directions = {(1, 1), (1, -1), (-1, 1), (-1, -1)};
        SquareScript squareScript = currentSquare.GetComponent<SquareScript>();
        char file = squareScript.file;
        int rank = squareScript.rank;

        foreach ((int fileDir, int rankDir) in directions) {
            while(true) {
                file = (char)(file+fileDir);
                rank += rankDir;
                GameObject newSquare = boardScript.GetSquareAt(file, rank);
                if(newSquare==null) {
                    break;
                }
                SquareScript newSquareScript = newSquare.GetComponent<SquareScript>();
                GameObject occupiedBy = newSquareScript.occupiedBy;
                // if a square is occupied by a piece
                if(occupiedBy!=null) {
                    bool pieceIsWhite = piece.GetComponent<PieceBase>().IsWhite;
                    bool occupiedByWhite = occupiedBy.GetComponent<PieceBase>().IsWhite;
                    // if the piece on the square is of same color
                    if((pieceIsWhite && occupiedByWhite) || (!pieceIsWhite && !occupiedByWhite) ) {
                        break;
                    }
                    // different color - can be captured
                    legalMoves.Add(newSquare.name);
                    break;
                }
                // empty square
                legalMoves.Add(newSquare.name);
            }
            // reset them to  current square positions
            file = squareScript.file;
            rank = squareScript.rank;
        }
        foreach (string square in legalMoves)
        {
            Debug.Log(square);
        }
        return legalMoves;

    }
    
}
