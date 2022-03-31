using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이전 총알생성기에서 사용했던 방식인
// 매번 필요시 마다 사용했던 'Instantiate'(생성) 방식이 아닌
// 오브젝트 풀링 방식을 사용할꺼에요.
// 오브젝트 풀링(Object Pooling)?
// 게임 초기에 필요한 만큼 오브젝트를 미리 만들어
// '풀(Pool):웅덩이'에 쌓아두는 방식입니다.
// 왜 해당 방식이 필요하나!!!
// Instantiate() 메서드처럼 오브젝트를 실시간으로 생성하거나,
// Destory() 메서드처럼 오브젝트를 실시간으로 파괴하는
// 처리는 성능을 많이 요구 합니다.
// 또한 메모리를 정리하는 GC(가비지 컬렉션)유발하기 쉽습니다.
// 게임 도중에 오브젝트를 너무 자주 생성하거나 파괴하면 
// 게임 끊김(프리즈) 현상이 발생이 됩니다.

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatfotmSpawner : MonoBehaviour
{
    // 생성할 발판의 원본 프리팹
    public GameObject platformPrefab;
    // 생성할 발판 수
    public int count = 3;

    // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMin = 1.25f;
    // 다음 배치까지의 시간 간격 최대값
    public float timeBetSpawnMax = 2.25f;
    // 다음 배치까지의 시간 간격
    private float timeBetSpawn;

    // 배치할 위치의 최소 y값
    public float yMin = -3.5f;
    // 배치할 위치의 최대 y값
    public float yMax = 1.5f;
    // 배치할 위치의 x값
    private float xPos = 20f;

    // 미리 생성한 발판들을 보관할 배열
    private GameObject[] platforms;
    // 사용할 현재 순번의 발판
    private int currentIndex = 0;

    // 초반에 생성한 발판을 화면 밖에 숨겨둘 위치
    private Vector2 poolPosition = new Vector2(0, -25);
    // 마지막 배치 시점
    private float lastSpawnTime;

    void Start() 
    {
        // 변수를 초기화하고 사용할 발판을 미리 생성

        // count만큼의 공간을 가지는 새로운 발판 배열 생성
        platforms = new GameObject[count];

        // count만큼 루프하면서 발판 생성
        for(int i = 0; i < count; i++)
        {
            // platformPrefab을 원본으로 새 발판을
            // poolPosition 위치에 복제 생성
            // 생성된 발판을 platforms 배열에 할당
            platforms[i] = Instantiate(platformPrefab, 
                poolPosition, Quaternion.identity);
        }

        // 마지막 배치 시점 초기화
        lastSpawnTime = 0f;
        // 다은번 배치까지의 시간 간격을 초기화
        timeBetSpawn = 0f;
    }

    void Update()
    {
        // 순서를 돌아가며 주기적 발판을 배치

        // 게임오버 상태에서는 동작하지 않음.
        if (GameManager.instance.isGameover) return;

        // 마지막 배치 시점에서 timeBetSpawn 이상 시간이 흘렀다면,
        if(Time.time >= lastSpawnTime + timeBetSpawn)
        {
            // 기록된 마지막 배치 시점을 현재 시점으로 갱신
            lastSpawnTime = Time.time;

            // 다음 배치까지의 시간 간격을 timeBetSpawnMin,
            // timeBetSpawnMax 사이에서 랜덤 가져오기
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            // 배치할 위치의 높이를 yMin과 yMax 사이에서 랜덤 가져오기
            float yPos = Random.Range(yMin, yMax);

            // 사용할 현재 순번의 발판 게임 오브젝트를 비활성화하고
            // 바로 즉시 다시 활성화 이 때, 발판의 Platform 컴포넌트의
            // OnEnable 메서드가 실행됨
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            // 현재 순번의 발판을 화면 오른쪽에 재배치
            platforms[currentIndex].transform.position 
                = new Vector2(xPos, yPos);
            // 순번 넘기기
            currentIndex++;

            // 마지막 순번에 도달했다면...
            if(currentIndex >= count)
            {
                currentIndex = 0;
            }
        }

    }
}
