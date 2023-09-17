using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class EnemyManager : Service
    {
        [SerializeField] private Transform SpawnTarget;
        [SerializeField] private Transform EnemyCollection;
        [SerializeField] private Enemy EnemyPrefab;

        private Coroutine spawnUpdate;

        private List<Enemy> activeEnemies = new List<Enemy>();

        private bool keepSpawning;

        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            keepSpawning = true;
            spawnUpdate = StartCoroutine(SpawnEnemyCoroutine());
        }

        private IEnumerator SpawnEnemyCoroutine()
        {
            while (keepSpawning)
            {
                yield return new WaitForSeconds(6);

                var _newEnemy = Instantiate(EnemyPrefab, SpawnTarget.position, Quaternion.identity, EnemyCollection);
                _newEnemy
                    .SetEnemyManager(this)
                    .Init();
            }
        }

        public void AddEnemy(Enemy _enemy)
        {
            activeEnemies.Add(_enemy);
        }

        public void RemoveEnemy(Enemy _enemy)
        {
            activeEnemies.Remove(_enemy);
        }
    }
}
