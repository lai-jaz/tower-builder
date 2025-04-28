using UnityEngine;

public class PlaceBlock : MonoBehaviour
{
    [Header("Tower Blocks")]
    [SerializeField] private GameObject blockPrefab; // tower blocks
    private GameObject baseBlock; // base with rigidbody for sway effects, tilting
    private GameObject floatingBlock; // tower block that is floating above

    private Camera mainCamera;
    [Header("Main Camera Options")]
    [SerializeField] private float cameraMoveUp = 1.75f;
    [SerializeField] private float cameraMoveSpeed = 2f;
    private Vector3 cameraNewPosition;
    private Vector3 cameraOldPosition;

    [Header("Materials array")]
    [SerializeField] private Material[] blockMaterials;

    private TowerPhysics tp;
    private Score score;

    private void Start()
    {
        baseBlock = GameObject.FindWithTag("Base");
        mainCamera = Camera.main;
        cameraOldPosition = mainCamera.transform.position;
        cameraNewPosition = cameraOldPosition;

        tp = FindFirstObjectByType<TowerPhysics>();
        score = FindFirstObjectByType<Score>();

        SpawnFloatingBlock();
    }
    private void Update()
    {
        if (tp.collapsed) // reset camera, top of tower and floating block position
        {
            cameraNewPosition = cameraOldPosition;
            Vector3 spawnPos = GetTopOfTower() + new Vector3(0, 0.75f, 0);
            floatingBlock.transform.position = spawnPos;
            return;
        }

        if (Input.touchCount > 0 ) // touch to drop the block
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                DropFloatingBlock();
                cameraNewPosition += new Vector3(0f, cameraMoveUp, 0f);
                score.UpdateScore(baseBlock.transform.childCount*10);
            }
        }
        if (floatingBlock != null) // move floating block
        {
            MoveFloatingBlock();
        }
    }
    private void LateUpdate()
    {
        if (tp.collapsed)
            return;

        Camera.main.transform.position = Vector3.Lerp(
        Camera.main.transform.position,
        cameraNewPosition,
        Time.deltaTime * cameraMoveSpeed
        );
    }

    //---------------------- floating block functions ----------------------------------
    public void SpawnFloatingBlock()
    {
        Vector3 spawnPos = GetTopOfTower() + new Vector3(0, 0.75f, 0);
        floatingBlock = createRandomTowerBLock(spawnPos);

        CheckCollapsed collisionScript = floatingBlock.GetComponent<CheckCollapsed>();
        collisionScript.tp = FindFirstObjectByType<TowerPhysics>();

        Rigidbody rb = floatingBlock.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true; // disable physics while floating
        }
    }

    private void MoveFloatingBlock()
    {
        float floatSpeed = 1.5f;
        float floatRange = 1f;
        Vector3 swayPosition = floatingBlock.transform.position;

        swayPosition.x = Mathf.Sin(Time.time * floatSpeed) * floatRange;

        floatingBlock.transform.position = swayPosition;
    }

    private void DropFloatingBlock()
    {
        GameObject previousTop = GameObject.FindWithTag("Top");
        if (previousTop != null)
            previousTop.tag = "Block";

        Rigidbody rb = floatingBlock.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false; // enable physics to make the block drop

        floatingBlock.tag = "Top";
        floatingBlock.transform.parent = baseBlock.transform;
        floatingBlock = null;

        Invoke(nameof(SpawnFloatingBlock), 0.2f);
    }


    private Vector3 GetTopOfTower()
    {
        GameObject topBlock = GameObject.FindWithTag("Top");

        if (topBlock == null)
        {
            topBlock = GameObject.FindWithTag("Base");
        }
        float topY = topBlock.transform.position.y + topBlock.transform.localScale.y / 2f;
        return new Vector3(0f, topY, -5.5f);

    }

    // ---------------------------- create random block -----------------------------------
    private GameObject createRandomTowerBLock(Vector3 spawnPos)
    {
        GameObject block = Instantiate(blockPrefab, spawnPos, Quaternion.identity);

        // random size
        float scaleX = Random.Range(1.2f, 2.5f);
        float scaleY = Random.Range(0.9f, 1.2f);
        float scaleZ = Random.Range(1.2f, 2.0f);
        block.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        // random color from saved materials
        if (blockMaterials.Length > 0)
        {
            Material randomMat = blockMaterials[Random.Range(0, blockMaterials.Length)];
            block.GetComponent<Renderer>().material = randomMat;
        }

        return block;
    }
}
