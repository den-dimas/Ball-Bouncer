using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public IObjectPool<Enemy> objectPool;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && objectPool != null)
            objectPool.Release(this);
    }
}
