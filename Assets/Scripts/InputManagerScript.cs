using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{
    public LayerMask piecesLayer;
    public LayerMask boardLayer;
    // TODO - Use the tag instead of public fields for GameLogicManager and its script.

    private GameLogicManagerScript gameLogicManagerScript;
    private GameObject selectedPiece = null;
    private Transform targetSquare = null;

    // Start is called before the first frame update
    void Start()
    {
        boardLayer = LayerMask.GetMask("Board");
        piecesLayer = LayerMask.GetMask("Pieces");
        gameLogicManagerScript = GameObject.FindGameObjectWithTag("GameLogicManagerTag").GetComponent<GameLogicManagerScript>();
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
    // Check if it's the correct player's turn
    if (selectedPiece == null && !gameLogicManagerScript.CheckTurn(hitPiece)) {
        return;
    }

    PieceInterface hitPieceInterface = hitPiece.GetComponent<PieceInterface>();
    PieceInterface selectedpieceInterface = selectedPiece ? selectedPiece.GetComponent<PieceInterface>() : null;

    
    if (selectedpieceInterface != null) {
        
        if ((hitPieceInterface.IsWhite && !selectedpieceInterface.IsWhite) || 
            (!hitPieceInterface.IsWhite && selectedpieceInterface.IsWhite)) {
           
            bool moveSuccessful = selectedpieceInterface.MovePiece(selectedPiece.transform, hitPieceInterface.CurrentSquare.transform);

            ResetHighlight(selectedPiece);
            if(!moveSuccessful) {
                return;
            }
            gameLogicManagerScript.SwitchTurn();
            selectedPiece = null;
            return;
        }
    }

    
    if (hitPiece != selectedPiece) {
        ResetHighlight(selectedPiece);
        selectedPiece = hitPiece;       
        HighlightPiece(selectedPiece);
        // Debug.Log(selectedPiece);
        // PieceBase pieceBase = selectedPiece.GetComponent<PieceBase>();
        // Debug.Log(pieceBase);
        // pieceBase
        selectedpieceInterface = selectedPiece.GetComponent<PieceInterface>();
        selectedPiece.GetComponent<PieceBase>().GenerateMoves(selectedPiece, selectedpieceInterface.CurrentSquare.gameObject);
    } else {
        ResetHighlight(selectedPiece); 
        selectedPiece = null;
    }
}


    void HandleSquareSelection(Transform hitSquare) {
        if(selectedPiece) {
            targetSquare = hitSquare;
            PieceInterface pieceInterface = selectedPiece.GetComponent<PieceInterface>();
            bool moveSuccessful = pieceInterface.MovePiece(selectedPiece.transform, targetSquare.transform);
            if(!moveSuccessful) {
                return;
            }
            ResetHighlight(selectedPiece);
            gameLogicManagerScript.SwitchTurn();
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

    void ResetHighlight(GameObject piece) {
        if(piece == null) {
            return;
        }
        Vector3 resetScale = new Vector3(0.6f, 0.6f, 1);
        piece.transform.localScale = resetScale;
    }
}
