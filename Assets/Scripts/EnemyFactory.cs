using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField]
    private Enemy _prefab;

    public Enemy Get()
    {
        Enemy instance = CreateGameObjectInstance(_prefab);
        instance.OriginFactory = this;
        return instance;
    }

    public void Reclaim(Enemy enemy)   // удаление врага
    {
        Destroy(enemy.gameObject);
    }
}

