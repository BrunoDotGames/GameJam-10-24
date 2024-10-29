using UnityEngine;

public class Item : MonoBehaviour
{
    public InventoryItemData data;

    public void OnPickupItem()
    {
        Destroy(gameObject);
    }

    public void EnemySpawn(ItemType type)
    {
        Debug.Log($"Spawning Enemy");
        if(type == ItemType.Necklace)
        {
           Debug.Log($"Spawning Necklace Enemy");
           Instantiate(data.enemySpawner, data.spawnLocation, Quaternion.identity);
        }
        else if (type == ItemType.Earings)
        {
            Instantiate(data.enemySpawner, data.spawnLocation, Quaternion.identity);
        }
        else if (type == ItemType.Ring)
        {
           Instantiate(data.enemySpawner, data.spawnLocation, Quaternion.identity);
        }
    }
}
