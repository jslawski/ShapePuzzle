using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image fill;
    [SerializeField]
    private TextMeshProUGUI inputPrompt;

    public void SetupVictoryScreenTile(GameTile finalTile)
    {
        this.background.material = finalTile.bgRenderer.material;
        this.fill.sprite = finalTile.fillRenderer.sprite;
        this.fill.color = finalTile.fillRenderer.material.color;

        if (SceneManager.GetActiveScene().buildIndex < 2)
        {
            this.inputPrompt.text = "PRESS SPACE TO START NEXT LEVEL";
        }
        else
        {
            this.inputPrompt.text = "THANK YOU FOR PLAYING! PRESS ESC TO QUIT";
        }
    }

    
}
