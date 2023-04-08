using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerMove_Event();

public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField]
    public float m_moveSpeed = 6f;
    [Header("Rotational speed")]
    [SerializeField]
    private float m_rotateSpeed = 90f;
    [Header("Camera X lower limit")]
    [SerializeField]
    private float m_CameraClampMax_X = 90f;
    [Header("Camera X limit")]
    [SerializeField]
    private float m_CameraClampMin_X = -20f;
    [Header("Movement speed Acceleration")]
    [SerializeField]
    private float m_moveSpeedUp = 2;
    [Header("Minimum distance from camera")]
    [SerializeField]
    private float m_CameraDistanceMin = -2;
    [Header("Maximum camera distance")]
    [SerializeField]
    private float m_CameraDistanceMax = -8;
    [Header("Camera distance")]
    [SerializeField]
    private float m_CameraDistance = -8;


    private float m_tempSpeedUp = 1;
    private Vector3 m_moveVec = Vector3.zero;
    private Transform m_cameraPivot = null;
    private Vector3 lastMousePosition = Vector3.zero;
    public event PlayerMove_Event playerMoveStartRuning_Event;
    public event PlayerMove_Event playerMoveEndRuning_Event;
    private Player mPlayer;
    private bool DownLeftShift = false;

    private void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        mPlayer = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameIsPause())
        {
            mPlayer.mPlayerState = PlayerMoveState.Idle;

            Vector3 horizontalMove = Vector3.zero;
            m_tempSpeedUp = 1;
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 v = Camera.main.transform.forward;
                v.y = 0;
                v.Normalize();
                horizontalMove += v;
            }
            if (Input.GetKey(KeyCode.S))
            {
                Vector3 v = -Camera.main.transform.forward;
                v.y = 0;
                v.Normalize();
                horizontalMove += v;
            }
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 v = -Camera.main.transform.right;
                v.y = 0;
                v.Normalize();
                horizontalMove += v;
            }
            if (Input.GetKey(KeyCode.D))
            {
                Vector3 v = Camera.main.transform.right;
                v.y = 0;
                v.Normalize();
                horizontalMove += v;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (mPlayer.Runing() || DownLeftShift)
                {
                    DownLeftShift = true;
                    m_tempSpeedUp = m_moveSpeedUp;
                    playerMoveStartRuning_Event?.Invoke();
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                DownLeftShift = false;
                playerMoveEndRuning_Event?.Invoke();
            }
            if (horizontalMove.magnitude > 0)
            {
                RotateToDirection(new Vector2(horizontalMove.x, horizontalMove.z));
                if (m_tempSpeedUp <= 1)
                {
                    mPlayer.mPlayerState = PlayerMoveState.Run;
                }
                else
                {
                    mPlayer.mPlayerState = PlayerMoveState.Runing;
                }
            }

            CharacterController cc = GetComponent<CharacterController>();
            if (cc.isGrounded)
            {
                m_moveVec = new Vector3(0, -0.1f, 0);
              
            }
            else
            {
                m_moveVec += Physics.gravity * Time.fixedDeltaTime;
                //mPlayer.mPlayerState = PlayerMoveState.None;
            }

            m_moveVec = horizontalMove + new Vector3(0, m_moveVec.y, 0);
            cc.Move(new Vector3(m_moveVec.x * m_moveSpeed, m_moveVec.y, m_moveVec.z * m_moveSpeed) * Time.deltaTime * m_tempSpeedUp);
        }
        else
        {
            DownLeftShift = false;
            mPlayer.mPlayerState = PlayerMoveState.Idle;
        }
    }
    private void LateUpdate()
    {
        if (GameManager.Instance.GameIsPause())
        {
            Vector3 dir = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;


            if (m_cameraPivot != null)
            {
                UpdateCamera(dir.x, -dir.y);
                Transform cameraTrans = m_cameraPivot.GetChild(0);
                m_CameraDistance = Mathf.Clamp(Input.mouseScrollDelta.y * 0.3f + cameraTrans.localPosition.z, m_CameraDistanceMax, m_CameraDistanceMin);
                cameraTrans.localPosition = new Vector3(cameraTrans.localPosition.x, cameraTrans.localPosition.y, m_CameraDistance);

            }


        }

    }
    public void IsShiftOver(bool end)
    {
        DownLeftShift = end;
    }
    private bool RotateToDirection(Vector2 direction, bool atOnce = false, float deltaTime = 0.033f)
    {
        if (direction.sqrMagnitude <= 0.00001f)
        {
            return true;
        }
        Vector3 characterForword = transform.forward;
        Vector2 cf = new Vector2(characterForword.x, characterForword.z);
        float degree = Vector2.Angle(cf, direction);
        direction = new Vector2(direction.y, -direction.x);
        if (Vector2.Dot(cf, direction) > 0) { degree = -degree; }
        float deltaDegree = m_rotateSpeed * deltaTime;
        if (atOnce) { deltaDegree = Mathf.Abs(degree); }
        if (Mathf.Abs(degree) > deltaDegree)
        {
            if (degree < 0)
            {
                deltaDegree = -deltaDegree;
            }
            Vector3 euler = transform.localEulerAngles;
            euler.y += deltaDegree;
            transform.localEulerAngles = euler;
            return false;
        }
        else
        {
            Vector3 euler = transform.localEulerAngles;
            euler.y += degree;
            transform.localEulerAngles = euler;
            return true;
        }
    }
    private void UpdateCamera(float deltaX, float deltaY)
    {
        m_cameraPivot.localPosition = this.transform.Find("UpAnchor").position;
        if (deltaX == 0 && deltaY == 0) return;
        if (Input.GetMouseButton(0))
        {
            Vector3 rotate = m_cameraPivot.localEulerAngles + new Vector3(deltaY, deltaX, 0);
            if (rotate.x >= 180f) rotate.x -= 360f;
            if (rotate.x > m_CameraClampMin_X && rotate.x < m_CameraClampMax_X)
            {
                m_cameraPivot.localEulerAngles = rotate;
            }
        }
    }
}