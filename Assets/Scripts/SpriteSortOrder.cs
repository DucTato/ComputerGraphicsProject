using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
    private SpriteRenderer theSR;

    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>(); 
        // The higher the object, the further it is (overlapped by other objects)
        theSR.sortingOrder = Mathf.RoundToInt( transform.position.y * -10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
