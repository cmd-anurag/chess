using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : PieceBase
{
    
    // Start is called before the first frame update
    void Start()
    {
        gameLogicManagerScript = GameObject.FindGameObjectWithTag("GameLogicManagerTag").GetComponent<GameLogicManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
