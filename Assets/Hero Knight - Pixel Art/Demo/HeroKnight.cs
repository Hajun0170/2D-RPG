using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;

using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HeroKnight : MonoBehaviour
{


    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;
    public Transform pos;
    public Vector2 boxSize;
    Vector2 targetPos;
   
    // 충돌 무시 대상
    private Collider2D playerCollider;
    [SerializeField] private LayerMask enemyLayerMask;
    private List<Collider2D> enemiesInRange = new List<Collider2D>();

    public int maxHealth = 10;  // 최대 체력
    private int currentHealth;     // 현재 체력
    public Slider healthBar;         // 체력바 슬라이더
    public int Hp = 10;

    public LayerMask enemyLayer; // 적 Layer 설정
    /// //////////////////////

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    private int originalLayer;

 
    ///////////////

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();

        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void IgnoreEnemyCollision()
    {
        foreach (var enemy in enemiesInRange)
        {
            Physics2D.IgnoreCollision(playerCollider, enemy, true);
        }
        Debug.Log("Ignoring collisions with enemies.");
    }

    public void RestoreEnemyCollision()
    {
        foreach (var enemy in enemiesInRange)
        {
            Physics2D.IgnoreCollision(playerCollider, enemy, false);
        }
        Debug.Log("Restored collisions with enemies.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other);
        }
    }

    

   
    public void TakeDamage(int damage)
    {
        
        if (Hp == 0)
        {

            SceneManager.LoadScene("scene4");

        }

    
       m_animator.SetTrigger("Hurt");
     
        Hp = Hp - damage;
        currentHealth = currentHealth - damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력은 0에서 maxHealth로 제한

        UpdateHealthBar();


    }

  

    // 체력바 업데이트 함수
    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth;  // 체력바 값을 현재 체력에 맞게 설정
    }

    // 무적 상태 활성화
    private void EnableInvincibility()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");

        // 충돌 무시 설정: 적 Layer와 충돌 무시
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 1.0f, enemyLayerMask);
        foreach (var enemy in enemies)
        {
            Physics2D.IgnoreCollision(playerCollider, enemy, true);
        }
    }

    // 무적 상태 종료
    public void DisableInvincibility()
    {
        gameObject.layer = originalLayer;

        // 충돌 복원
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 1.0f, enemyLayerMask);
        foreach (var enemy in enemies)
        {
            Physics2D.IgnoreCollision(playerCollider, enemy, false);
        }

        m_rolling = false; // 구르기 상태 해제
    }

    // 애니메이션 이벤트를 통해 호출
    public void OnRollComplete()
    {
        DisableInvincibility();
    }

    // Update is called once per frame
    void Update()
    {



        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }

        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }



        // Move
        if (!m_rolling)
            m_body2d.linearVelocity = new Vector2(inputX * m_speed, m_body2d.linearVelocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.linearVelocity.y);

        // -- Handle Animations --
        //Wall Slide
        //m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        // m_animator.SetBool("WallSlide", m_isWallSliding);

        ///////대쉬///////
        bool isShiftPressed = Input.GetKey(KeyCode.LeftShift);

        if (isShiftPressed && Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // 대시 동작
            PerformShiftAction(inputX);
        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // 기본 이동
            PerformNormalMovement(inputX);
        }
        else
        {
            // 정지 상태
            PerformIdle();
        }

        void PerformShiftAction(float inputX)
        {
            float dashSpeed = m_speed * 3.0f; // 대시 속도
            m_body2d.linearVelocity = new Vector2(inputX * dashSpeed, m_body2d.linearVelocity.y);

            // 대시 애니메이션 트리거
            // m_animator.SetTrigger("Dash");
        }

        void PerformNormalMovement(float inputX)
        {
            m_body2d.linearVelocity = new Vector2(inputX * m_speed, m_body2d.linearVelocity.y);
            m_animator.SetInteger("AnimState", 1);
        }

        void PerformIdle()
        {
            m_body2d.linearVelocity = new Vector2(0, m_body2d.linearVelocity.y);
            m_animator.SetInteger("AnimState", 0);
        }
        /////////////////////
        /*
        //Death
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }      

         /*   
        //Hurt
        else if (Input.GetKeyDown("q") && !m_rolling)
            m_animator.SetTrigger("Hurt");
       */
        string currentSceneName = SceneManager.GetActiveScene().name;
        //Attack
        if (Input.GetKeyDown("z") && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.1f;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {

                if (collider.tag == "Enemy" && currentSceneName == "scene1")
                {
                    collider.GetComponent<Monai>().TakeDamege(1);

                  
                }

                else if (collider.tag == "Enemy" && currentSceneName == "Scene2")
                {
                    collider.GetComponent<Bossai>().TakeDamege(1);
                }

            }
        }
        /*
        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);
        */

        // Roll
        else if (Input.GetKeyDown("x") && !m_rolling)
        {
            originalLayer = gameObject.layer;
            m_rolling = true;
            m_animator.SetTrigger("Roll");

            // 구르기 상태 시작
            EnableInvincibility();
            // 구르기 이동 처리
            m_body2d.linearVelocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.linearVelocity.y);

            // 일정 시간 후 또는 애니메이션 종료 시 복원
            Invoke(nameof(DisableInvincibility), 0.5f); // 0.5초 후 무적 해제 (애니메이션 길이에 맞게 조정)
            // > 몬스터 공격에 딜레이를 추가해보자.
        }






        //Jump
        /*
         else if (Input.GetKeyDown("c") && m_grounded && !m_rolling)
         {
             m_animator.SetTrigger("Jump");
             m_grounded = false;
             m_animator.SetBool("Grounded", m_grounded);
             m_body2d.linearVelocity = new Vector2(m_body2d.linearVelocity.x, m_jumpForce);
             m_groundSensor.Disable(0.2f);
         }
        */

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in slide animation.
    /*
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
    */



}
