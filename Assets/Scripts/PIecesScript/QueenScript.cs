using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenScript : PieceBase
{
    
    
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
                file += (char)fileDir;
                rank += rankDir;
                GameObject newSquare = boardScript.GetSquareAt(file, rank);
                if(newSquare==null) {
                    break;
                }
                SquareScript newSquareScript = newSquare.GetComponent<SquareScript>();
                if(newSquareScript.occupiedBy != null) {
                    legalMoves.Add(newSquare.name);
                    break;
                }
                legalMoves.Add(newSquare.name);
            }
        }

        return legalMoves;

    }
}
