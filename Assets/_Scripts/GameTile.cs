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
    
    private SpriteRenderer markedRenderer;
    private bool traversed = false;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        this.bgRenderer = spriteRenderers[0];        
        this.outlineRenderer = spriteRenderers[1];
        this.fillRenderer = spriteRenderers[2];
        this.markedRenderer = spriteRenderers[3];
    }

    public virtual void TraverseTile()
    {
        this.traversed = true;
        this.markedRenderer.enabled = true;
    }

    public void ResetTraverseState()
    {
        this.traversed = false;
        this.markedRenderer.enabled = false;
    }
}
