using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    public GameObject squarePrefab;
    private Dictionary<(char, int), GameObject> squareDictionary = new Dictionary<(char, int), GameObject>();
    public int gridSize = 8;
    public float squareSize = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        DrawBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DrawBoard() {
        
        for(int x = 0; x < gridSize; ++x) {
            for(int y = 0; y < gridSize; ++y) {
                GameObject square = Instantiate(squarePrefab, transform);

                square.transform.position = new Vector3(x * squareSize, y*squareSize, 0);
                // Scale the square to match squareSize
                square.transform.localScale = new Vector3(squareSize, squareSize, 1);


                char file = (char)('a' + x);
                square.name = $"{file}{y+1}";

                // Setting the file and rank;
                SquareScript squareScript = square.GetComponent<SquareScript>();

                if(squareScript!=null) {
                    squareScript.rank = y+1;
                    squareScript.file = file;
                    squareDictionary[(file, y+1)] = square;
                }

                // Setting the color of each square
                SpriteRenderer renderer = square.GetComponent<SpriteRenderer>();
                if(renderer != null) {
                    Color colorDark = new(112/255f, 102/255f, 119/255f);
                    Color colorLight = new(204/255f, 183/255f, 174/255f);

                    renderer.color = (x+y) % 2 !=0 ? colorLight : colorDark;
                }
            }
        }
        centerBoard();
    }
    void centerBoard() {
        float halfSize = gridSize*squareSize/2f;
        transform.position = new Vector3(-halfSize + (squareSize/2f), -halfSize + (squareSize/2f), 0);
    }
    public GameObject GetSquareAt(char file, int rank) {
        if(file > 'h' || file < 'a' || rank < 1 || rank > 8) {
            return null;
        }
        
        if(squareDictionary.TryGetValue((file, rank), out GameObject square)) {
            return square;
        }
        return null;
    }
}
