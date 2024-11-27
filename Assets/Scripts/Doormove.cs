using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; //씬이동 관련 구문 동작
using UnityEngine.UI;


public class Doormove : MonoBehaviour
{
   
    public string nextSceneName;    // 다음 씬 이름
    public Button transitionButton; // 버튼 연결
          

    
    public void StartButton()
    {
        SceneManager.LoadScene("scene3");


    }
    
    
}

