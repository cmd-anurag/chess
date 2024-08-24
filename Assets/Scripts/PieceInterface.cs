using UnityEngine;

public interface PieceInterface {
    Transform CurrentSquare {get; set;}
    bool MovePiece(Transform piece, Transform targetSquare);
}