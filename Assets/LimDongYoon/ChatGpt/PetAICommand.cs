using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AICommand;
public class PetAICommand : MonoBehaviour
{

    PetRobot petRobot;
    private GameObject player;
    private GameObject boss;
    private WeaponScript wepon;
    

    // Start is called before the first frame update
    void Start()
    {
        petRobot = FindAnyObjectByType<PetRobot>();
        player = FindObjectOfType<PlayerController>().gameObject;
        wepon = player.GetComponent<WeaponScript>();
        boss = FindObjectOfType<BossTag>().gameObject;
        StartCoroutine( TestJson());
    }
    
    public IEnumerator TestJson()
    {
        yield return null;
        Debug.Log(JsonUtility.ToJson(SettingCurrentState()));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    monster1Location = numberOfMonsters > 0 ? GameManager.Instance.monsters[0].transform.position : Vector3.zero,
    monster2Location = numberOfMonsters > 1 ? GameManager.Instance.monsters[1].transform.position : Vector3.zero,
    monster3Location = numberOfMonsters > 2 ? GameManager.Instance.monsters[2].transform.position : Vector3.zero,

     */
    public CurrentState SettingCurrentState()
    {
        CurrentState current = new CurrentState()
        {
            gameInformation = new GameInformation() 
            {
               BossLocation = boss.transform.position,
               TotalNormalMonsters = 4,
               NumberOfNormalMonstersRemaining = GameManager.Instance.monsters.Length,
               Monster1Location = GameManager.Instance.monsters[0].transform.position,
               Monster2Location = GameManager.Instance.monsters[1].transform.position,
               Monster3Location = GameManager.Instance.monsters[2].transform.position,
               DistanceBetweenMainCharacterAndBoss = Vector3.Distance(boss.transform.position,player.transform.position),
               CurrentgBossHealth = "80"


            },
            playerStatus = new PlayerStatus() 
            {
               
            },
            TX500Status = new TX500Status() 
            { 
                
            }

        };
        return current;
    }
    public void PlayCommand(_ChatMessageResult result)
    {
        if (petRobot != null) { Debug.Log("petRobotScript is Null!"); return; } 
        int commandNum = result.commandNum;
        string command = result.command;
        if (commandNum == 0)
        {

        }
        if (commandNum == 1)
        {
            switch (command)
            {
                case "FollowPlayer":
                    petRobot.state = State.FollowPlayer;
                    break;

                case "Attack":
                    petRobot.state = State.Attack;
                    break;

                case "Defense":
                    petRobot.state = State.Defense;
                    break;

                case "Guide":
                    petRobot.state = State.Guide;
                    break;

                case "Speak":
                    petRobot.state = State.Speak;
                    break;

                case "Explore":
                    petRobot.state = State.Explore;
                    break;

                default:
                    // 처리할 옵션이 없는 경우
                    break;
            }


        }

    }

}

namespace AICommand
{
    [System.Serializable]
    public class CurrentState
    {
        public GameInformation gameInformation;
        public PlayerStatus playerStatus;
        public TX500Status TX500Status;
    }
    [System.Serializable]
    public class GameInformation
    {
        public int TotalNormalMonsters;
        public int NumberOfNormalMonstersRemaining;
        public Vector3 Monster1Location;
        public Vector3 Monster2Location;
        public Vector3 Monster3Location;
        public Vector3 BossLocation;
        public float DistanceBetweenMainCharacterAndBoss;
        public string CurrentgBossHealth;
        public bool Clear;
    }

    [System.Serializable]
    public class PlayerStatus
    {
        public string Paint;
        public string PhysicalStrength;
        public Vector3 CurrentMainCharacterLocation;
        public string CurrentMainCharacterWeapon;
        public string CurrentStateOfMainCharacter;

    }

    [System.Serializable]
    public class TX500Status
    {
        public Vector3 Location;
        public float DistanceFromPlayer;
        public State TX500Mode;
    }
}
/*게임 정보: {
  전체 일반 몬스터 수: 3,
  남은 일반 몬스터 수: 3,
  몬스터 1위치:
  몬스터 2위치:
  몬스터 3위치:
  보스 위치 Vector3(22.5,0,-46.5999985)
  주인공 과보스와의 거리: 150f,
  남은 보스 체력: 100%,
  클리어 유무: false,
}
주인공 상태:{
페인트:10%,
체력:30%,
현재 주인공 위치: Vector3(44.5,-1940,-86.5999985)
현재 주인공 무기: 검,
현재 주인공 상태: 보스와 싸우는중
}
TX-500 상태 :{
    위치:Vector3(44.5,0,-86.5999985)
    플레이어와의 거리: 2149f,
    모드:FollowPlayer,
}
*/