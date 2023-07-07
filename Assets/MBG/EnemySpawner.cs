/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //적 프리팹
    public Enemy enemyPrefab;
    public EnemyShaman enemyShamanPrefab;
    public EnemyGhost enemyGhostPrefab;

    //적 스폰포인트
    public Transform[] enemySpawnPoints;

    //생성한 적을 담을 리스트
    private List<Enemy> enemies = new List<Enemy>();
    private List<EnemyShaman> enemyShamans = new List<EnemyShaman>();
    private List<EnemyGhost> enemyGhosts = new List<EnemyGhost>();

    private int allEnemyCount; //총 적 개수
    private int wave; //현재 웨이브

    //적 초기 개수
    public int defaultEnemyCount = 2;
    public int defaultEnemyShamanCount = 2;
    public int defaultEnemyGhostCount = 1;

    //적 생성 개수
    public int enemyCount;
    public int enemyShamanCount;
    public int enemyGhostCount;

    private void Start()
    {
        enemyCount = defaultEnemyCount;
        enemyShamanCount = defaultEnemyShamanCount;
        enemyGhostCount = defaultEnemyGhostCount;
    }

    // Update is called once per frame
    void Update()
    {

     allEnemyCount = enemies.Count + enemyShamans.Count + enemyGhosts.Count;

    }

    //적 생성 위치 마다 적을 생성 적 능력치(근접형AI)
    void CreateEnemy(Transform spawnPoint)
    {
        //스폰 포인트 개수 만큼 실행
        for (int i = 0; i < enemyCount; i++)
        {
            //스폰 위치 랜덤 설정
            Vector3 randomSpawnPoint = spawnPoint.position + Random.insideUnitSphere * 2f;
            randomSpawnPoint.y = spawnPoint.position.y; //높이 초기화

            //적 프리팹으로 적 생성
            Enemy enemy = Instantiate(enemyPrefab, randomSpawnPoint, spawnPoint.rotation);

            //생성한 적의 능력치와 추적 대상 설정
            enemy.Setup(health, damage, speed); //적 클래스로 접근해야 Setup 접근 가능

            //생성된 적을 리스트에 추가
            enemies.Add(enemy);

            //적의 onDeath 이벤트에 익명 메서드 등록
            //사망한 적을 리스트에 제거
            enemy.onDeath += () => enemies.Remove(enemy);
            //사망한 적을 5초 뒤에 파괴
            enemy.onDeath += () => Destroy(enemy.gameObject, 5f);
            //적 사망 시 점수 상승
            enemy.onDeath += () => GameManager.instance.AddScore(enemyScore);
        }
    }

    //원거리 적 생성
    void CreateShaman(Transform spawnPoint)
    {
        //스폰 포인트 개수 만큼 실행
        for (int i = 0; i < enemyShamanCount; i++)
        {
            //스폰 위치 랜덤 설정
            Vector3 randomSpawnPoint = spawnPoint.position + Random.insideUnitSphere * 2f;
            randomSpawnPoint.y = spawnPoint.position.y; //높이 초기화


            //적 프리팹으로 적 생성
            EnemyShaman shaman = Instantiate(enemyShamanPrefab, randomSpawnPoint, spawnPoint.rotation);

            //생성된 적을 리스트에 추가
            enemyShamans.Add(shaman);

            //적의 onDeath 이벤트에 익명 메서드 등록
            //사망한 적을 리스트에 제거
            shaman.onDeath += () => enemyShamans.Remove(shaman);
            //사망한 적을 5초 뒤에 파괴
            shaman.onDeath += () => Destroy(shaman.gameObject, 5f);
            //적 사망 시 점수 상승
            shaman.onDeath += () => GameManager.instance.AddScore(shamanScore);
        }
    }
}*/