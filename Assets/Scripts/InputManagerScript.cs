using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{
    public LayerMask piecesLayer;
    public LayerMask boardLayer;
    private GameObject selectedPiece = null;
    private Transform targetSquare = null;
    // Start is called before the first frame update
    void Start()
    {
        boardLayer = LayerMask.GetMask("Board");
        piecesLayer = LayerMask.GetMask("Pieces");
        
    }

    // Update is called once per frame
    void Update()
    {
        HandlePieceClicked();
    }

    void HandlePieceClicked() {
        if(Input.GetMouseButtonDown(0)) {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hitPiece = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, piecesLayer);
            RaycastHit2D hitSquare = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, boardLayer);

            if(hitPiece.collider != null) {
                HandlePieceSelection(hitPiece.collider.gameObject);
            }
            else if(hitSquare.collider != null) {
                HandleSquareSelection(hitSquare.collider.gameObject.transform);
            }
            
        }
    }

    void HandlePieceSelection(GameObject hitPiece) {
        if (hitPiece != selectedPiece) {
            ResetHightlight(selectedPiece);
            selectedPiece = hitPiece;
            HighlightPiece(selectedPiece);
            Transform piecePosition = selectedPiece.GetComponent<PieceInterface>().CurrentSquare;
            // Debug.Log($"{selectedPiece.name} selected on {piecePosition.name}");
        }
        else  {
            ResetHightlight(selectedPiece);
            selectedPiece = null;
        }
    }

    void HandleSquareSelection(Transform hitSquare) {
        if(selectedPiece) {
            targetSquare = hitSquare;
            // Debug.Log("Target Square is " + targetSquare.name + selectedPiece);
            PieceInterface pieceInterface = selectedPiece.GetComponent<PieceInterface>();
            pieceInterface.MovePiece(selectedPiece.transform, targetSquare.transform);
            ResetHightlight(selectedPiece);
            selectedPiece = null;
        }
        else {
            Debug.Log("No Piece Selected");
        }
    }

    

    void HighlightPiece(GameObject piece) {
        if(piece==null) {
            return;
        }
        Vector3 highlightScale = new Vector3(0.7f, 0.7f, 1);
        piece.transform.localScale = highlightScale;
    }

    void ResetHightlight(GameObject piece) {
        if(piece == null) {
            return;
        }
        Vector3 resetScale = new Vector3(0.6f, 0.6f, 1);
        piece.transform.localScale = resetScale;
    }
}
