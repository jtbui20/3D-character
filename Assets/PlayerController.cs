using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float InputX;
    public float InputZ;

    public float BaseSpeed = 1;
    public float SpeedMulti = 2f;
    public bool isSprint = false;

    public GameObject CameraTarget;
    public float TopClamp;
    public float BottomClamp;
    public float CameraAngleOverride = 0.0f;
    public bool CameraLock = false;
    public float CameraAccelerator = 500f;

    private float _Yaw;
    private float _Pitch;

    private Animator _anim;
    private GameObject _camera;

    // Start is called before the first frame update

    private void Awake()
    {
        if (_camera == null) _camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();

        RotatePosition();
        ApplySpeed();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    void ProcessInput()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        isSprint = Input.GetButton("Sprint");
    }

    private void CameraRotation()
    {
        _Yaw += Input.GetAxis("Mouse X") * CameraAccelerator * Time.deltaTime;
        _Pitch += Input.GetAxis("Mouse Y") * CameraAccelerator * Time.deltaTime;

        _Yaw = ClampAngle(_Yaw, float.MinValue, float.MaxValue);
        _Pitch = ClampAngle(_Pitch, BottomClamp, TopClamp);

        CameraTarget.transform.rotation = Quaternion.Euler(_Pitch + CameraAngleOverride, _Yaw, 0.0f);
    }

    private static float ClampAngle(float value, float min, float max)
    {
        if (value < -360f) value += 360f;
        if (value > 360f) value -= 360f;
        return Mathf.Clamp(value, min, max);
    }

    void RotatePosition()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        var forward = _camera.transform.forward;
        var right = _camera.transform.right;
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movevDirection = forward * InputZ + right * InputX;
        if (movevDirection.magnitude != 0) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movevDirection), 0.1f);
    }

    void ApplySpeed()
    {
        BaseSpeed = new Vector2(InputX, InputZ).magnitude;
        _anim.SetFloat("Direction", InputX * 2);
        _anim.SetFloat("Speed", BaseSpeed * ((isSprint) ? SpeedMulti : 1));
    }
}
