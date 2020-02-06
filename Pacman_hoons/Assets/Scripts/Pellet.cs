using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
	[SerializeField]
	int score;

    [SerializeField]
    bool isPowerPellet;


	private void OnTriggerEnter2D(Collider2D _col)
	{
		if(_col.tag == "Player")
		{
			GameManager.Inst.addScore(score);
            if(isPowerPellet)
            {
                GameManager.Inst.EatenBigPellet();
            }
			this.gameObject.SetActive(false);
            GameManager.Inst.playerEatenPelletNum++;
            GameManager.Inst.UpdateUI();
		}
	}
}
