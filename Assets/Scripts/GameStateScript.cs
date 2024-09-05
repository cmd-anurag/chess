using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum PieceType {
    WhitePawns,
    BlackPawns,
    WhiteRooks,
    BlackRooks,
    WhiteKing,
    BlackKing,
    WhiteQueen,
    BlackQueen,
    WhiteBishop,
    BlackBishop,
    WhiteKnight,
    BlackKnight
}

public class GameStateScript : MonoBehaviour
{
// individual piece bitboards
private ulong whitePawns;
private ulong blackPawns;
private ulong whiteRooks;
private ulong blackRooks;
private ulong whiteKing;
private ulong blackKing;
private ulong whiteQueen;
private ulong blackQueen;
private ulong whiteBishop;
private ulong blackBishop;
private ulong whiteKnight;
private ulong blackKnight;

public ulong friendlyPieces;
public ulong enemyPieces;
public ulong occupiedBitboard;

// Attack bitboards for each piece type
    private ulong[] rookAttacks;
    private ulong[] bishopAttacks;
    private ulong[] queenAttacks;
    private ulong[] kingAttacks;
    private ulong[] knightAttacks;

    void InitializeAttackBitboards() {
        rookAttacks = new ulong[64];
        bishopAttacks = new ulong[64];
        queenAttacks = new ulong[64];
        kingAttacks = new ulong[64];
        knightAttacks = new ulong[64];
        ulong square;

        for (int i = 0; i < 64; i++) {
            square = 1UL << i;
            rookAttacks[i] = GenerateRookAttacks(square);
            bishopAttacks[i] = GenerateBishopAttacks(square);
            queenAttacks[i] = GenerateQueenAttacks(i);
            kingAttacks[i] = GenerateKingAttacks(square);
            knightAttacks[i] = GenerateKnightAttacks(square);
        }
    }

    ulong GenerateKingAttacks(ulong square) {
        ulong attacks = 0;
        ulong rank1mask = 0x00000000000000FFUL;
        ulong rank8mask = 0xFF00000000000000UL;
        ulong afile = 0x0101010101010101UL;
        ulong hfile = 0x8080808080808080UL;

        // left shift 8 (up)
        attacks |= (square & ~rank8mask) << 8;

        // left shift 1 (right)
        attacks |= (square & ~hfile) << 1;

        // left shift 9 (top right)
        attacks |= (square & ~(rank8mask | hfile)) << 9;
        
        // left shift 7 (top left)
        attacks |= (square & ~(rank8mask | afile)) << 7;

        // right shift 1 (left)
        attacks |= (square & ~afile) >> 1;

        // right shift 8 (down)
        attacks |= (square & ~rank1mask) >> 8;

        // right shift 7 (bottom right)
        attacks |= (square & ~(rank1mask | hfile)) >> 7;

        // right shift 9 (bottom left)
        attacks |= (square & ~(rank1mask | afile)) >> 9;

        return attacks;
    }

    ulong GenerateKnightAttacks(ulong square) {
        ulong attacks = 0;
        // border ranks
        ulong rank1mask = 0x00000000000000FFUL;
        ulong rank2mask = 0x000000000000FF00UL;
        ulong rank7mask = 0x00FF000000000000UL;
        ulong rank8mask = 0xFF00000000000000UL;

        // border files
        ulong afile = 0x0101010101010101UL;
        ulong bfile = 0x0202020202020202UL;
        ulong hfile = 0x8080808080808080UL;
        ulong gfile = 0x4040404040404040UL;

        // left shift 17 (2 up 1 right)
        attacks |= (square & ~(rank7mask | rank8mask | hfile)) << 17;

        // left shift 15 (2 up 1 left)
        attacks |= (square & ~(rank7mask | rank8mask | afile)) << 15;

        // left shift 10 (1 up 2 right)
        attacks |= (square & ~(rank8mask | gfile | hfile)) << 10;

        // left shift 6 (1 up 2 left)
        attacks |= (square & ~(rank8mask | afile | bfile)) << 6;

        // right shift 17 (2 down 1 left)
        attacks |= (square & ~(rank1mask | rank2mask | afile)) >> 17;

        // right shift 15 (2 down 1 right)
        attacks |= (square & ~(rank1mask | rank2mask | hfile)) >> 15;

        // right shift 10 (1 down 2 left)
        attacks |= (square & ~(rank1mask | afile | bfile)) >> 10;

        // right shift 6 (1 down 2 right)
        attacks |= (square & ~(rank1mask | gfile | hfile)) >> 6;


        return attacks;
    }

    ulong GenerateQueenAttacks(int squareIndex) {
        return rookAttacks[squareIndex] | bishopAttacks[squareIndex];
    }

    ulong GenerateRookAttacks(ulong square) {
        // 8 is up (left shift)
        // -8 is down (right shift)
        // -1 is left (left shift)
        // 1 is right (right shift)

        ulong attacks = 0;
        int[] directions = {8, -8, 1, -1};
        ulong original = square;
        foreach (int direction in directions) {
            square = original;
            while(square != 0) {
                
                if(direction == 8) {
                    if((square & 0xFF00000000000000UL) != 0) {
                        attacks |= square;
                        break;
                    }
                    square <<= direction;
                    attacks |= square;
                }
                else if(direction == -8) {
                    if((square & 0x00000000000000FFUL) != 0) {
                        attacks |= square;
                        break;
                    }
                    square >>= -direction;
                    attacks |= square;
                }
                else if(direction == -1) {
                    if((square & 0x8080808080808080UL) != 0) {
                        attacks |= square;
                        break;
                    }
                    square <<= -direction;
                    attacks |= square;
                }
                else if(direction == 1) {
                    if((square & 0x0101010101010101UL) != 0) {
                        attacks |= square;
                        break;
                    }
                    square >>= direction;
                    attacks |= square;
                }
            }
            // i used this to reset the home square to 0.
            attacks &= ~original;
        }
        return attacks;
    }
    ulong GenerateBishopAttacks(ulong square) {
        // 9 is top right (left shift)
        // 7 is top left (left shift)
        // -9 is bottom left (right shift)
        // -7 is bottom right (right shift)

        ulong attacks = 0;
        int[] directions = {7, 9, -7, -9};
        ulong original = square;

        foreach (int direction in directions)
        {   
            square = original;
            while(square != 0) {
                
                if(direction == 9) {
                    if(((square & 0xFF00000000000000UL) != 0) || ((square & 0x8080808080808080UL) != 0)) {
                        attacks |= square;
                        break;
                    }
                    square <<= direction;
                    attacks |= square;
                }
                else if(direction == 7) {
                    if(((square & 0xFF00000000000000UL) != 0) || ((square & 0x0101010101010101UL) != 0)) {
                        attacks |= square;
                        break;
                    }
                    square <<= direction;
                    attacks |= square;
                }
                else if(direction == -9) {
                    if(((square & 0x00000000000000FF) != 0) || ((square & 0x0101010101010101UL) != 0)) {
                        attacks |= square;
                        break;
                    }
                    square >>= -direction;
                    attacks |= square;
                }
                else if(direction == -7) {
                    if(((square & 0x00000000000000FF) != 0) || ((square & 0x8080808080808080UL) != 0)) {
                        attacks |= square;
                        break;
                    }
                    square >>= -direction;
                    attacks |= square;
                }
            }
        }
        attacks &= ~original;
        return attacks;
    }

    public ulong GetPieceBitBoard(PieceType pieceType) {
        return pieceType switch
        {
            PieceType.WhitePawns => whitePawns,
            PieceType.BlackPawns => blackPawns,
            PieceType.WhiteRooks => whiteRooks,
            PieceType.BlackRooks => blackRooks,
            PieceType.WhiteKing => whiteKing,
            PieceType.BlackKing => blackKing,
            PieceType.WhiteQueen => whiteQueen,
            PieceType.BlackQueen => blackQueen,
            PieceType.WhiteBishop => whiteBishop,
            PieceType.BlackBishop => blackBishop,
            PieceType.WhiteKnight => whiteKnight,
            PieceType.BlackKnight => blackKnight,
            _ => throw new System.ArgumentException("invalid piece type"),
        };
    }

public void SetPieceBitboard(PieceType pieceType, ulong value) {
    switch (pieceType)
        {
            case PieceType.WhitePawns:
                whitePawns = value;
                break;
            case PieceType.BlackPawns:
                blackPawns = value;
                break;
            case PieceType.WhiteKnight:
                whiteKnight = value;
                break;
            case PieceType.BlackKnight:
                blackKnight = value;
                break;
            case PieceType.WhiteBishop:
                whiteBishop = value;
                break;
            case PieceType.BlackBishop:
                blackBishop = value;
                break;
            case PieceType.WhiteKing:
                whiteKing = value;
                break;
            case PieceType.BlackKing:
                blackKing = value;
                break;
            case PieceType.WhiteRooks:
                whiteRooks = value;
                break;
            case PieceType.BlackRooks:
                blackRooks = value;
                break;
            case PieceType.WhiteQueen:
                whiteQueen = value;
                break;
            case PieceType.BlackQueen:
                blackQueen = value;
                break;
            default:
                throw new System.ArgumentException("invalid piece type");
        }
}

public void DisplayBitboard(ulong value) {
    string binarystring = Convert.ToString((long)value, 2).PadLeft(64, '0');
   
    // Debug.Log(binarystring);
    System.Text.StringBuilder stringBuilder = new();
    System.Text.StringBuilder rowBuilder = new();
    for(int i = 0; i < 64; ++i) {
        rowBuilder.Insert(0, binarystring[i] + " ");
        if((i + 1) % 8 == 0) {
            stringBuilder.Append(rowBuilder.ToString());
            stringBuilder.AppendLine();
            rowBuilder.Clear();
        }
    }
    Debug.Log(stringBuilder.ToString());
}





void InitializeBitboards() {
    
    whitePawns = 0x000000000000FF00UL;
    blackPawns = 0x00FF000000000000UL;
    whiteRooks = 0x0000000000000081UL;
    blackRooks = 0x8100000000000000UL;
    whiteKing = 0x0000000000000010UL;
    blackKing = 0x1000000000000000UL;
    whiteQueen = 0x0000000000000008UL;
    blackQueen = 0x0800000000000000UL;
    whiteBishop = 0x0000000000000024UL;
    blackBishop = 0x2400000000000000UL;
    whiteKnight = 0x0000000000000042UL;
    blackKnight = 0x4200000000000000UL;

    friendlyPieces = whitePawns | whiteRooks | whiteKing | whiteQueen | whiteBishop | whiteKnight;
    enemyPieces = blackPawns | blackRooks | blackKing | blackQueen | blackBishop | blackKnight;
    occupiedBitboard = friendlyPieces | enemyPieces;
}


    void Awake() {
        InitializeBitboards();
        InitializeAttackBitboards();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
