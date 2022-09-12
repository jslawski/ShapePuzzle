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

    public bool traversed = false;

    private Animator tileAnimator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        this.bgRenderer = spriteRenderers[0];        
        this.outlineRenderer = spriteRenderers[1];
        this.fillRenderer = spriteRenderers[2];
        this.markedRenderer = spriteRenderers[3];

        this.tileAnimator = GetComponent<Animator>();

        StartCoroutine(this.LoadTile());
    }

    private IEnumerator LoadTile()
    {
        float loadDelay = Random.Range(0.0f, 0.5f);

        yield return new WaitForSeconds(loadDelay);

        this.tileAnimator.SetTrigger("LoadTileTrigger");
    }

    public virtual void TraverseTile(PlayerUnit player)
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
