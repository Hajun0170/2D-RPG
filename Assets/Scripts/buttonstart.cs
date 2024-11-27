using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; //���̵� ���� ���� ����
using UnityEngine.UI;
using UnityEngine.Video;

public class buttonstart : MonoBehaviour
{
    public VideoPlayer videoPlayer; // ���� �÷��̾� ����
    public string nextSceneName;    // ���� �� �̸�
    public Button transitionButton; // ��ư ����
    public Canvas canvas;           // UI ĵ���� (���� ���� �� ���� ó��)

    /*
    public void StartButton(VideoPlayer vp)
    {
        SceneManager.LoadScene("scene1");


    }
    */
   public void OnButtonClick()
    {
        // ��ư�� Ŭ���ϸ� ������ Ȱ��ȭ�ϰ� ���
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();

        // UI ĵ���� ����� (���� ����)
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        //��ư Ŭ�� �̺�Ʈ ���
        transitionButton.onClick.AddListener(OnButtonClick);

        //���� �÷��̾��� ��� �Ϸ� �̺�Ʈ ���
       videoPlayer.loopPointReached += OnVideoEnd;

         //���� �÷��̾�� ó���� ��� ���� ���·� ����
        videoPlayer.gameObject.SetActive(false);
    }

    
    void OnVideoEnd(VideoPlayer vp)
    {
        // ���� ����� ������ ���� ������ �̵�
        SceneManager.LoadScene("scene1");
    }
    
    
}

