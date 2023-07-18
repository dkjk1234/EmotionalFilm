using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AICommand;
using UnityEditor.Timeline.Actions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PetAICommand : MonoBehaviour
{

    PetRobot petRobot;
    private GameObject player;
    private GameObject boss;
    private WeaponScript weponMgr;
    private Slider bossHp;

    // Start is called before the first frame update
    void Start()
    {
        petRobot = FindAnyObjectByType<PetRobot>();
        player = FindObjectOfType<PlayerController>().gameObject;
        weponMgr = player.GetComponent<WeaponScript>();
        boss = FindObjectOfType<BossTag>().gameObject;
        bossHp = GameManager.Instance.uIScript.bossHealthSlider;
        StartCoroutine( TestJson());
    }
    
    public IEnumerator TestJson()
    {
        yield return null;
        Debug.Log(SettingCurrentState());
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
    public string SettingCurrentState()
    {
        
        var petLocation = petRobot.transform.position;
        var playerLocation = player.transform.position;
        var bossLocation = boss.transform.position;
        var monsters = GameManager.Instance.monsters;
        CurrentState current = new CurrentState()
        {
            gameInformation = new GameInformation()
            {
                BossLocation = boss.transform.position,
                TotalNormalMonsters = 4,
                NumberOfNormalMonstersRemaining = monsters.Length,
                Monster1Location = monsters[0].transform.position,
                Monster2Location = monsters[1].transform.position,
                Monster3Location = monsters[2].transform.position,
                DistanceBetweenMainCharacterAndBoss = Vector3.Distance(bossLocation, playerLocation),
                CurrentgBossHealth = ((int)(bossHp.value / bossHp.maxValue)*100).ToString() + "%"


    },
            playerStatus = new PlayerStatus()
            {
                CurrentMainCharacterLocation = playerLocation,
                CurrentMainCharacterWeapon = weponMgr.weapon.ToString(),
                Health = GameManager.Instance.playerHealth.ToString(),
                PaintAmount = weponMgr.paintValue.ToString() + "%",

            },
            TX500Status = new TX500Status()
            {
                DistanceFromPlayer = Vector3.Distance(petLocation, playerLocation),
                Location = petLocation,
                TX500Mode = petRobot.state,

                
            }
            
        };
        return JsonUtility.ToJson(current);
    }
    public void PlayCommand(_ChatMessageResult result)
    {
        if (petRobot == null) { Debug.Log("petRobotScript is Null!"); return; }
        if (result.command == null) { Debug.Log("Ŀ�ǵ尡 �����ϴ�"); return; }
        else { Debug.Log("Ŀ�ǵ� " + result.command); Debug.Log("�޽��� " + result.message); Debug.Log("��ȣ" + result.commandNum); }
        
        int commandNum = result.commandNum;
        string command = result.command;
        if (commandNum == 2 || commandNum == 3)
        {
            
     
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }
        
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
                    Debug.Log("Ŀ�ǵ带 �ν����� �� �Ͽ����ϴ�.");
                    // ó���� �ɼ��� ���� ���
                    break;
            


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
        public string PaintAmount;
        public string Health;
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
/*���� ����: {
  ��ü �Ϲ� ���� ��: 3,
  ���� �Ϲ� ���� ��: 3,
  ���� 1��ġ:
  ���� 2��ġ:
  ���� 3��ġ:
  ���� ��ġ Vector3(22.5,0,-46.5999985)
  ���ΰ� ���������� �Ÿ�: 150f,
  ���� ���� ü��: 100%,
  Ŭ���� ����: false,
}
���ΰ� ����:{
����Ʈ:10%,
ü��:30%,
���� ���ΰ� ��ġ: Vector3(44.5,-1940,-86.5999985)
���� ���ΰ� ����: ��,
���� ���ΰ� ����: ������ �ο����
}
TX-500 ���� :{
    ��ġ:Vector3(44.5,0,-86.5999985)
    �÷��̾���� �Ÿ�: 2149f,
    ���:FollowPlayer,
}
*/