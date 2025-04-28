using TMPro;
using UnityEngine;

public class TowerPhysics : MonoBehaviour
{
    //[SerializeField] private Vector3 spawnOffset = new Vector3(0, 1, 0);

    [Header("Tower Sway & Instability")]
    [SerializeField] private float swayForce = 5f;

    [Header("Canvases")]
    [SerializeField] private Canvas gamePlayCanvas;
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private TextMeshProUGUI scoreTextFinal;

    [Header("Audio")]
    [SerializeField] private AudioSource gameOverSource;
    [SerializeField] private AudioSource bgMusicSource;

    [SerializeField] private AudioClip gameOverClip;



    public bool collapsed;

    private GameObject baseBlock;
    private Score score;

    private void Awake()
    {
        gameOverCanvas.gameObject.SetActive(false);
        gamePlayCanvas.gameObject.SetActive(true);
        
    }
    private void Start()
    {

        collapsed = false;
        baseBlock = GameObject.FindWithTag("Base");
        score = FindFirstObjectByType<Score>();
    }

    void Update()
    {
        if (collapsed) return;
        SwayTower();
    }

    private void SwayTower()
    {
        if (collapsed) 
            return;

        float towerHeight = baseBlock.transform.childCount + 1; // including base block
        float newSwayForce = swayForce * towerHeight;

        float swayX = Mathf.Sin(Time.time * 0.5f);
        float swayZ = Mathf.Cos(Time.time * 0.5f);

        Vector3 sway = new Vector3(swayX, 0, swayZ);
        GetComponent<Rigidbody>().AddTorque(newSwayForce * Time.deltaTime * sway, ForceMode.Force);
    }

    public void CollapseTower(bool replayBtn)
    {
        collapsed = true;

        if (!replayBtn) // if gameover by losing
        {
            DestroyBlocks();
            Invoke(nameof(GameOver), 1f);
        }
        else // if tower reset by replay button
        {
            DestroyBlocks();
            Invoke(nameof(ResetCollapsed), 1f); // wait for PlaceBlock reset functionalities
            score.UpdateScore(0);
        }
    }

    private void DestroyBlocks()
    {

        foreach (Transform block in baseBlock.transform)
        {
            if (block.CompareTag("Block") || block.CompareTag("Top"))
            {
                Destroy(block.gameObject);
            }
        }
    }

    private void GameOver()
    {
        if (bgMusicSource.isPlaying)
            bgMusicSource.Pause();
        gameOverSource.PlayOneShot(gameOverClip);

        gamePlayCanvas.gameObject.SetActive(false);
        scoreTextFinal.text = score.getScore().ToString();
        gameOverCanvas.gameObject.SetActive(true);

        score.UpdateScore(0);
        Invoke(nameof(ResetCollapsed), 3f); // reset tower after some time after Game Over

    }

    private void ResetCollapsed()
    {
        gameOverCanvas.gameObject.SetActive(false);
        gamePlayCanvas.gameObject.SetActive(true);

        if (!bgMusicSource.isPlaying)
        {
            bgMusicSource.time = 0f;
            bgMusicSource.Play(); 
        }
        collapsed = false;
    }

}
