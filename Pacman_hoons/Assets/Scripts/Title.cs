using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Title : MonoBehaviour
{
    public Animator blinkyAnim;
    public Animator inkyAnim;
    public Animator pinkyAnim;
    public Animator clydeAnim;

    public TextMeshProUGUI touchScreenText;
    private int addValue;

    [SerializeField]
    private float animationTimer;
    private float animationChangeTime = 2f;

    Vector2 animDirection;

    void Start()
    {
        animDirection = Vector2.left;
        addValue = -1;
    }

    // Update is called once per frame
    void Update()
    {
        animationTimer += Time.deltaTime;
        if( animationTimer > animationChangeTime)
        {
            ChangeAnimDirection();
            SetAnimDirection();
            animationTimer = 0;
        }

        BlinkText();

        if (Input.GetKeyUp(KeyCode.Return) || Input.touchCount>0)
        {
            SceneManager.LoadScene(1);
        }
    }

    private void BlinkText()
    {
        touchScreenText.alpha += (Time.deltaTime * addValue);
        if (touchScreenText.alpha < 0.3)
            addValue = 1;
        else if (touchScreenText.alpha >= 1)
            addValue = -1;
    }
    private void ChangeAnimDirection()
    {
        if (animDirection == Vector2.left)
        {
            animDirection = Vector2.up;
        }
        else if (animDirection == Vector2.up)
        {
            animDirection = Vector2.right;
        }
        else if (animDirection == Vector2.right)
        {
            animDirection = Vector2.down;
        }
        else if (animDirection == Vector2.down)
        {
            animDirection = Vector2.left;
        }
    }

    private void SetAnimDirection()
    {
        blinkyAnim.SetInteger("DirY", (int)animDirection.y);
        blinkyAnim.SetInteger("DirX", (int)animDirection.x);
        pinkyAnim.SetInteger("DirY", (int)animDirection.y);
        pinkyAnim.SetInteger("DirX", (int)animDirection.x);
        inkyAnim.SetInteger("DirY", (int)animDirection.y);
        inkyAnim.SetInteger("DirX", (int)animDirection.x);
        clydeAnim.SetInteger("DirY", (int)animDirection.y);
        clydeAnim.SetInteger("DirX", (int)animDirection.x);
    }
}
