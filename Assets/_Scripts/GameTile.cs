using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer bgRenderer;
    [HideInInspector]
    public SpriteRenderer outlineRenderer;
    [HideInInspector]
    public SpriteRenderer fillRenderer;    

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        this.bgRenderer = spriteRenderers[0];        
        this.outlineRenderer = spriteRenderers[1];
        this.fillRenderer = spriteRenderers[2];
    }    
}
