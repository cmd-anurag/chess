using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingScript : PieceBase
{
    

    // Update is called once per frame
    void Update()
    {
        
    }
    public override List<string> GenerateMoves(GameObject piece, GameObject currentSquare) {
        List<string> legalMoves = new();
        (int, int)[] directions = {(1, 1), (1, -1), (-1, 1), (-1, -1), (1, 0), (-1, 0), (0, 1), (0, -1)};
        SquareScript squareScript = currentSquare.GetComponent<SquareScript>();
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
