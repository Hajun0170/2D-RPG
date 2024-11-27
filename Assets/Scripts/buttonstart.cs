using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; //씬이동 관련 구문 동작
using UnityEngine.UI;
using UnityEngine.Video;

public class buttonstart : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 비디오 플레이어 연결
    public string nextSceneName;    // 다음 씬 이름
    public Button transitionButton; // 버튼 연결
    public Canvas canvas;           // UI 캔버스 (영상 시작 시 숨김 처리)

    /*
    public void StartButton(VideoPlayer vp)
    {
        SceneManager.LoadScene("scene1");


    }
    */
   public void OnButtonClick()
    {
        // 버튼을 클릭하면 비디오를 활성화하고 재생
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();

        // UI 캔버스 숨기기 (선택 사항)
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        //버튼 클릭 이벤트 등록
        transitionButton.onClick.AddListener(OnButtonClick);

        //비디오 플레이어의 재생 완료 이벤트 등록
       videoPlayer.loopPointReached += OnVideoEnd;

         //비디오 플레이어는 처음에 재생 중지 상태로 유지
        videoPlayer.gameObject.SetActive(false);
    }

    
    void OnVideoEnd(VideoPlayer vp)
    {
        // 비디오 재생이 끝나면 다음 씬으로 이동
        SceneManager.LoadScene("scene1");
    }
    
    
}

