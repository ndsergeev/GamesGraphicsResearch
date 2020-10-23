using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject itemToSpawn;
    private GameObject _item; 
    private void Start()
    {
        GameManager.Instance.pickups.Add(this);
        if (!itemToSpawn) return;
        
        _item = Instantiate(itemToSpawn, transform);
        _item.transform.position += Vector3.up;
    }

    public void Hide()
    {
        if (!_item) return;
        _item.SetActive(false);
    }

    public void Expose()
    {
        if (!_item) return;
        _item.SetActive(true);
    }
}
