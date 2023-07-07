using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //싱글턴 전용 프로퍼티
   public static GameManager instance
    {
        get
        {
           
            if(m_instance == null)
            {
                //씬에서 GameManager 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<GameManager>();
            }
            //싱글턴 오브젝트 반환
            return m_instance;
        }
    }

    //싱글턴이 할당될 static 변수
    private static GameManager m_instance;
    
    private int score = 0; //현재 게임 점수
    public bool isGameover { get; private set;}//게임 오버 상태

    private void Awake()
    {
        //씬에 싱글턴 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if(instance != this)
        {
            //자신을 파괴
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    
}