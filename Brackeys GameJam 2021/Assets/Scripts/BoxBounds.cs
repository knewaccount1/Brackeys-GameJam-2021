using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class BoxBounds : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Vector2 rndPoint2D;
    private Bounds bounds;

    void Start()
    {
        boxCollider =  GetComponent<BoxCollider2D>();
        bounds = boxCollider.bounds;
    }
 
    public Vector2 RandomPointInBounds()
    {
        rndPoint2D = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
        return rndPoint2D;
    }
}
