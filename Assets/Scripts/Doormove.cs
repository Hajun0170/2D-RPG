using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; //���̵� ���� ���� ����
using UnityEngine.UI;


public class Doormove : MonoBehaviour
{
   
    public string nextSceneName;    // ���� �� �̸�
    public Button transitionButton; // ��ư ����
          

    
    public void StartButton()
    {
        SceneManager.LoadScene("scene3");


    }
    
    
}

