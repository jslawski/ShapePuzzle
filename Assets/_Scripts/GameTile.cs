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

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        this.bgRenderer = spriteRenderers[0];        
        this.outlineRenderer = spriteRenderers[1];
        this.fillRenderer = spriteRenderers[2];
        this.markedRenderer = spriteRenderers[3];
    }

    public virtual void TraverseTile(PlayerUnit player)
    {
        this.markedRenderer.enabled = true;
    }

    public void ResetTraverseState()
    {
        this.markedRenderer.enabled = false;
    }
}
