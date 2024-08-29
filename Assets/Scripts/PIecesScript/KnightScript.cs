using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : PieceBase
{

    // Update is called once per frame
    void Update()
    {
        
    }
    public override List<string> GenerateMoves(GameObject piece, GameObject currentSquare) {
        gameLogicManagerScript.ClearAllHighlights();
        List<string> legalMoves = new();
        (int, int)[] directions = {(2, 1), (2, -1), (-2, 1), (-2, -1), (1, 2), (1, -2), (-1, 2), (-1, -2)};
        SquareScript squareScript = currentSquare.GetComponent<SquareScript>();
        bool pieceIsWhite = piece.GetComponent<PieceBase>().IsWhite;
        char file;
        int rank;
        foreach ((int fileDir, int rankDir) in directions) {
            file = squareScript.file;
            rank = squareScript.rank;
            file += (char)fileDir;
            rank += rankDir;
            GameObject newSquare = boardScript.GetSquareAt(file, rank);
            if(newSquare==null) {
                continue;
            }
            SquareScript newSquareScript = newSquare.GetComponent<SquareScript>();
            if(newSquareScript.occupiedBy != null) {
                 bool occupiedByWhite = newSquareScript.occupiedBy.GetComponent<PieceBase>().IsWhite;
                if((pieceIsWhite && occupiedByWhite) || (!pieceIsWhite && !occupiedByWhite)) {
                    continue;
                }
                gameLogicManagerScript.HighlightCapture(newSquare);
                legalMoves.Add(newSquare.name);
                continue;
            }
            gameLogicManagerScript.HighLightSquare(newSquare);
            legalMoves.Add(newSquare.name);
            file = squareScript.file;
            rank = squareScript.rank;
        }

        return legalMoves;

    }
    
}
