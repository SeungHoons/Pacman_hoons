using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singletone<GameManager>
{
	private GameObject player;
	public GameObject[] ghosts;
	public List<GameObject> allPellet;
	private static int score;
	public static int highScore;
	private static int playerLives = 5;
	public int playerEatenPelletNum = 0;
	public int totalPellet = 0;

	public static int stage = 1;

	public int ghostEatenScore = 200;
	private bool nowStartEaten = false;
	private bool nowStartDeath = false;
	Camera mainCamera;

	[SerializeField]
	private TextMeshProUGUI scoreText;
	[SerializeField]
	private TextMeshProUGUI liveText;
	[SerializeField]
	private TextMeshProUGUI highScoreText;
	[SerializeField]
	private TextMeshProUGUI eatenGhostScoreText;
	[SerializeField]
	private TextMeshProUGUI stageText;

	[SerializeField]
	private TextMeshProUGUI readyAndOverText;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		ghosts = GameObject.FindGameObjectsWithTag("Ghost");
		highScore = PlayerPrefs.GetInt("HighScore", 0);
		highScoreText.text = highScore.ToString();

		mainCamera = Camera.main;
		score = 0;

		StartCoroutine(StartGame());
		UpdateUI();
	}

	IEnumerator StartGame()
	{
		foreach (GameObject _ghost in ghosts)
		{
			_ghost.GetComponent<GhostBase>().isCanMove = false;
		}
		player.GetComponent<Animator>().enabled = false;
		player.GetComponent<Player>().isCanMove = false;

		UpdateUI();
		yield return new WaitForSeconds(2.25f);
		readyAndOverText.enabled = false;

		StartCoroutine(StartGameAfter(1));

	}
	IEnumerator StartGameAfter(float _delay)
	{
		yield return new WaitForSeconds(_delay);

		foreach (GameObject _ghost in ghosts)
		{
			_ghost.GetComponent<GhostBase>().isCanMove = true;
		}
		player.GetComponent<Animator>().enabled = true;
		player.GetComponent<Player>().isCanMove = true;
	}

	public void addScore(int _score)
	{
		score += _score;
	}

	public void EatenBigPellet()
	{
		foreach (GameObject _ghost in ghosts)
		{
			_ghost.GetComponent<GhostBase>().StartFrightenedMode();
		}
	}

	public void Restart()
	{
		player.GetComponent<Player>().ReStartGame();
		foreach (GameObject _ghost in ghosts)
		{
			_ghost.GetComponent<GhostBase>().Restart();
		}
		readyAndOverText.enabled = false;
		nowStartDeath = false;
	}

	private void CheckEatenPellet()
	{
		if (playerEatenPelletNum == totalPellet)
		{
			PlayerWin();
		}
	}

	private void PlayerWin()
	{
		stage++;
		if (stage > 2)
		{
			readyAndOverText.text = "PLAYER WIN";
			StartCoroutine(GameOverPlayerWinner(2));
		}
        else
            StartCoroutine(ProgressPlayerWin(2));
	}

	IEnumerator ProgressEndScene(float _delay)
	{
		yield return new WaitForSeconds(_delay);
		SceneManager.LoadScene(0);
	}

	IEnumerator GameOverPlayerWinner(float _delay)
    {
        player.GetComponent<Player>().isCanMove = false;
        player.GetComponent<Animator>().enabled = false;
		readyAndOverText.enabled = true;

        foreach (GameObject _ghost in ghosts)
        {
            _ghost.GetComponent<GhostBase>().isCanMove = false;
            _ghost.GetComponent<Animator>().enabled = false;
        }

        yield return new WaitForSeconds(_delay);
		StartCoroutine(ProgressEndScene(3));
    }

    IEnumerator ProgressPlayerWin(float _delay)
    {
        player.GetComponent<Player>().isCanMove = false;
        player.GetComponent<Animator>().enabled = false;

        foreach(GameObject _ghost in ghosts)
        {
            _ghost.GetComponent<GhostBase>().isCanMove = false;
            _ghost.GetComponent<Animator>().enabled = false;
        }

        yield return new WaitForSeconds(_delay);
        StartCoroutine(BlinkTileMap(2));
    }

    IEnumerator BlinkTileMap(float _delay)
    {
        player.GetComponent<SpriteRenderer>().enabled = false;

        foreach (GameObject _ghost in ghosts)
        {
            _ghost.GetComponent<SpriteRenderer>().enabled = false;
        }

        TileManager.Inst.GetTilemap().GetComponent<Animator>().SetBool("Blink", true);
        yield return new WaitForSeconds(_delay);
        TileManager.Inst.GetTilemap().GetComponent<Animator>().SetBool("Blink", false);
        StartNextLevel();
    }

    void StartNextLevel()
    {
        StopAllCoroutines();
        playerEatenPelletNum = 0;
        StartCoroutine(ProgressStartNextStage(1));
    }

    IEnumerator ProgressStartNextStage(float _delay)
    {
        readyAndOverText.enabled = true;
        UpdateUI();

        RedrawMap();

        yield return new WaitForSeconds(_delay);

        StartCoroutine(ProgressRestartShowObjects(1));
    }

    void RedrawMap()
    {
        foreach(GameObject _pellet in allPellet)
        {
            _pellet.SetActive(true);
        }
    }

    public void StartDeath()
    {
        if (!nowStartDeath)
        {
            nowStartDeath = true;
            StopAllCoroutines();

         

            foreach (GameObject _ghost in ghosts)
            {
                _ghost.transform.GetComponent<GhostBase>().isCanMove = false;
            }

            player.GetComponent<Player>().isCanMove = false;
            player.GetComponent<Animator>().enabled = false;

            StartCoroutine(ProgressDeathAfter(2));
        }
    }

    IEnumerator ProgressDeathAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject _ghost in ghosts)
        {
            _ghost.transform.GetComponent<SpriteRenderer>().enabled = false;
        }

        StartCoroutine(ProgressDeathAnimation(2f));
    }

    IEnumerator ProgressDeathAnimation(float delay)
    {
        player.transform.rotation = Quaternion.Euler(0, 0, 0);

        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<Animator>().SetBool("Idle", false);
        player.GetComponent<Animator>().SetBool("Die", true);

        yield return new WaitForSeconds(delay);

        StartCoroutine(ProgressRestart(2f));
    }

    IEnumerator ProgressRestart(float delay)
    {
        playerLives -= 1;


        ghostEatenScore = 200;

        player.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(delay);

        if (playerLives <= 0)
        {
            readyAndOverText.text = "GAME OVER";
            readyAndOverText.color = Color.red;
            readyAndOverText.enabled = true;
            player.GetComponent<SpriteRenderer>().enabled = false;

            StartCoroutine(ProgrssGameOver(2));
        }
        else
        {
            readyAndOverText.enabled = true;
            player.GetComponent<SpriteRenderer>().enabled = false;
            UpdateUI();
            yield return new WaitForSeconds(delay);
            StartCoroutine(ProgressRestartShowObjects(2));
        }
    }

    IEnumerator ProgrssGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
    }

    IEnumerator ProgressRestartShowObjects(float delay)
    {
        foreach (GameObject _ghost in ghosts)
        {
            _ghost.GetComponent<Animator>().enabled = false;
            _ghost.GetComponent<SpriteRenderer>().enabled = true;
            _ghost.GetComponent<GhostBase>().TranslateStartPosition();
        }

        player.GetComponent<Player>().TranslateStartPosition();
        player.GetComponent<Animator>().SetBool("Die", false);
        player.GetComponent<Player>().UpdateAnimationState();
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Animator>().enabled = false;

        yield return new WaitForSeconds(delay);

        Restart();
    }
    public void StartEaten(GhostBase _eatenGhost)
    {
        if (!nowStartEaten)
        {
            nowStartEaten = true;

            foreach (GameObject _ghost in ghosts)
            {
                _ghost.transform.GetComponent<GhostBase>().isCanMove = false;
            }

            player.GetComponent<Player>().isCanMove = false;

            player.GetComponent<SpriteRenderer>().enabled = false;

            _eatenGhost.transform.GetComponent<SpriteRenderer>().enabled = false;


            Vector2 pos = _eatenGhost.transform.position;
            Vector2 viewPortPoint = mainCamera.WorldToViewportPoint(pos);

            eatenGhostScoreText.GetComponent<RectTransform>().anchorMin = viewPortPoint;
            eatenGhostScoreText.GetComponent<RectTransform>().anchorMax = viewPortPoint;

            eatenGhostScoreText.text = ghostEatenScore.ToString();

            eatenGhostScoreText.GetComponent<TextMeshProUGUI>().enabled = true;

            StartCoroutine(ProgressEatenAfter(0.75f, _eatenGhost));
            UpdateUI();
        }
    }

    IEnumerator ProgressEatenAfter(float _delay, GhostBase _eatenGhost)
    {
        yield return new WaitForSeconds(_delay);


        eatenGhostScoreText.GetComponent<TextMeshProUGUI>().enabled = false;

        player.GetComponent<SpriteRenderer>().enabled = true;
        _eatenGhost.GetComponent<SpriteRenderer>().enabled = true;

        foreach (GameObject _ghost in ghosts)
        {
            _ghost.transform.GetComponent<GhostBase>().isCanMove = true;
        }
        player.GetComponent<Player>().isCanMove = true;
        nowStartEaten = false;
    }

    public void UpdateUI()
    {
        int currentStage;
        scoreText.SetText(score.ToString());

        liveText.text = playerLives.ToString();
        stageText.text = stage.ToString();
        currentStage = stage;

        if(score > highScore)
        {
            highScore = score;
            highScoreText.text = highScore.ToString();
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        CheckEatenPellet();

        //PlayerLivesUpdate()
    }

    private void PlayerLivesUpdate()
    {
        //인스턴스화 시킬수 있으면
        if (playerLives == 5)
        {

        }
    }
}
