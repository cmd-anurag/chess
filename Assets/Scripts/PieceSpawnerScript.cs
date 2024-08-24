using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PieceSpawnerScript : MonoBehaviour
{
    public GameObject whitePawnPrefab;
    public GameObject whiteKingPrefab;
    public GameObject whiteQueenPrefab;
    public GameObject whiteRookPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject whiteKnightPrefab;
    public GameObject blackPawnPrefab;
    public GameObject blackKingPrefab;
    public GameObject blackQueenPrefab;
    public GameObject blackRookPrefab;
    public GameObject blackBishopPrefab;
    public GameObject blackKnightPrefab;
    
    public GameObject Board;

    
    void Start()
    {
        SpawnPieces();
    }

    
    void Update()
    {
        
    }

    void SpawnPieces() {
        string squareName;

        // white pawns
        GameObject whitePawn;
        for(int i = 0; i < 8; ++i) {
            whitePawn = Instantiate(whitePawnPrefab);
            squareName = (char)('a' + i) + "2";
            PawnScript pawnScript = whitePawn.GetComponent<PawnScript>();
            
            Transform square = Board.transform.Find(squareName);
            if(square != null) {
                whitePawn.transform.position = square.position;
                whitePawn.transform.SetParent(Board.transform, true);
                pawnScript.CurrentSquare = square;
                pawnScript.isWhite = true;
            }
        }

        // black pawns;
        GameObject blackPawn;
        for(int i = 0; i < 8; ++i) {
            blackPawn = Instantiate(blackPawnPrefab);
            squareName = (char)('a' + i) + "7";
            PawnScript pawnScript = blackPawn.GetComponent<PawnScript>();
            Transform square = Board.transform.Find(squareName);
            if(square!=null) {
                blackPawn.transform.position = square.position;
                blackPawn.transform.SetParent(Board.transform, true);
                pawnScript.CurrentSquare = square;
                pawnScript.isWhite = false;
            }
        }

        // white rooks
        SpawnRook(whiteRookPrefab, "a1", true);
        SpawnRook(whiteRookPrefab, "h1", true);

        // black rooks
        SpawnRook(blackRookPrefab, "a8", false);
        SpawnRook(blackRookPrefab, "h8", false);
        
        // white queen
        SpawnQueen(whiteQueenPrefab, "d1", true);
        // black queen
        SpawnQueen(blackQueenPrefab, "d8", false);

        // white king
        SpawnKing(whiteKingPrefab, "e1", true);
        // black king
        SpawnKing(blackKingPrefab, "e8", false);

        // white bishops
        SpawnBishop(whiteBishopPrefab, "c1", true);
        SpawnBishop(whiteBishopPrefab, "f1", true);
        // black bishops
        SpawnBishop(blackBishopPrefab, "c8", false);
        SpawnBishop(blackBishopPrefab, "f8", false);


       // white knights
        SpawnKnight(whiteKnightPrefab, "b1", true);
        SpawnKnight(whiteKnightPrefab, "g1", true);

        // black knights
        SpawnKnight(blackKnightPrefab, "b8", false);
        SpawnKnight(blackKnightPrefab, "g8", false);
    }
    void SpawnRook(GameObject rookPrefab, string squareName, bool isWhite) {
        Transform rookSquare = Board.transform.Find(squareName);
        if (rookSquare != null) {
            GameObject rook = Instantiate(rookPrefab);
            RookScript rookScript = rook.GetComponent<RookScript>();
            rookScript.isWhite = isWhite;
            rookScript.CurrentSquare = rookSquare;
            rook.transform.position = rookSquare.position;
            rook.transform.SetParent(Board.transform, true);
        }
    }
    void SpawnQueen(GameObject queenPrefab, string squareName, bool isWhite) {
        Transform queenSquare = Board.transform.Find(squareName);
        if (queenSquare != null) {
            GameObject queen = Instantiate(queenPrefab);
            QueenScript queenScript = queen.GetComponent<QueenScript>();
            queenScript.isWhite = isWhite;
            queenScript.CurrentSquare = queenSquare;
            queen.transform.position = queenSquare.position;
            queen.transform.SetParent(Board.transform, true);
        }
    }
    void SpawnKing(GameObject kingPrefab, string squareName, bool isWhite) {
        Transform kingSquare = Board.transform.Find(squareName);
        if (kingSquare != null) {
            GameObject king = Instantiate(kingPrefab);
            KingScript kingScript = king.GetComponent<KingScript>();
            kingScript.isWhite = isWhite;
            kingScript.CurrentSquare = kingSquare;
            king.transform.position = kingSquare.position;
            king.transform.SetParent(Board.transform, true);
        }
    }
    void SpawnBishop(GameObject bishopPrefab, string squareName, bool isWhite) {
        Transform bishopSquare = Board.transform.Find(squareName);
        if (bishopSquare != null) {
            GameObject bishop = Instantiate(bishopPrefab);
            BishopScript bishopScript = bishop.GetComponent<BishopScript>();
            bishopScript.isWhite = isWhite;
            bishopScript.CurrentSquare = bishopSquare;
            bishop.transform.position = bishopSquare.position;
            bishop.transform.SetParent(Board.transform, true);
        }
    }
    void SpawnKnight(GameObject knightPrefab, string squareName, bool isWhite) {
        Transform knightSquare = Board.transform.Find(squareName);
        if (knightSquare != null) {
            GameObject knight = Instantiate(knightPrefab);
            KnightScript knightScript = knight.GetComponent<KnightScript>();
            knightScript.isWhite = isWhite;
            knightScript.CurrentSquare = knightSquare;
            knight.transform.position = knightSquare.position;
            knight.transform.SetParent(Board.transform, true);
        }
}

}
