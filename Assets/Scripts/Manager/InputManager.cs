using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public UnityEvent GetKeySpace_Event;
    public UnityEvent GetKeyW_Event;
    public UnityEvent GetKeyA_Event;
    public UnityEvent GetKeyS_Event;
    public UnityEvent GetKeyD_Event;
    public UnityEvent GetKeyLeftShift_Event;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameIsPause())
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                GetKeySpace_Event?.Invoke();
            }
            if (Input.GetKey(KeyCode.W))
            {
                GetKeyW_Event?.Invoke();
            }
            if (Input.GetKey(KeyCode.S))
            {
                GetKeyS_Event?.Invoke();
            }
            if (Input.GetKey(KeyCode.A))
            {
                GetKeyA_Event?.Invoke();
            }
            if (Input.GetKey(KeyCode.D))
            {
                GetKeyD_Event?.Invoke();
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GetKeyLeftShift_Event?.Invoke();
            }
        }
    }
}
