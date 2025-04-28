using UnityEngine;

public class ReplayBtnClick : MonoBehaviour
{
    private TowerPhysics tp;
    public void Start()
    {
        tp = FindFirstObjectByType<TowerPhysics>();
    }
    public void resetTower()
    {
        Debug.Log("Reset tower");
        tp.CollapseTower(true);
    }
}
