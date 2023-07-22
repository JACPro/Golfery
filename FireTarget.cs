using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireTarget : MonoBehaviour
{
    private bool _isAiming = false;
    [SerializeField] private Transform _centerOfTarget;
    [SerializeField] private float _fireSpeed = 5f;
    private Vector2 _aimStartPos;
    private Rigidbody2D _rb;

    [Header("Line Renderer")]
    [SerializeField][Tooltip("Material used by the line renderer")] private Material _lrMaterial;
    [SerializeField][Range(10, 100)] private int _numLinePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float _timeBetweenLinePoints = 0.1f;
    private LineRenderer _lineRenderer;
    private SpriteRenderer _spriteRenderer;

    //Audio
    [SerializeField] private AudioClip _drawBowSFX;
    [SerializeField] private AudioClip _fireShotSFX;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        InputManager.Instance.OnDisableEvent += ReleaseFireButton;
    }

    private void OnEnable() 
    {
        InputManager.Instance.Controls.Player.Fire.started += FireButtonPressed;
        InputManager.Instance.Controls.Player.Fire.canceled += FireButtonReleased;
        InputManager.Instance.Controls.Player.Cancel.started += CancelButtonPressed;
    }

    private void OnDisable() 
    {
        InputManager.Instance.Controls.Player.Fire.started -= FireButtonPressed;
        InputManager.Instance.Controls.Player.Fire.canceled -= FireButtonReleased;
        InputManager.Instance.Controls.Player.Cancel.started -= CancelButtonPressed;
    }

    private void FireButtonPressed(InputAction.CallbackContext context)
    {
        AudioManager.Instance.PlaySound(_drawBowSFX);
        _aimStartPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _isAiming = true;

        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, _centerOfTarget.position);
    }

    //used when input manager is disabled
    private void ReleaseFireButton()
    {   
        InputAction.CallbackContext context = new InputAction.CallbackContext();
        CancelButtonPressed(context);
    }

    private void FireButtonReleased(InputAction.CallbackContext context)
    {
        if (!_isAiming) return;
        AudioManager.Instance.PlaySound(_fireShotSFX);
        ScoreManager.Instance.RegisterShotFired();
        CinemachineShake.Instance.ShakeCamera(0.2f);

        _isAiming = false;
        _lineRenderer.enabled = false;

        //set target velocty
        Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - _aimStartPos;
        _rb.simulated = true;
        _rb.velocity = -direction * _fireSpeed;

        //make target fully opaque
        Color spriteColor = _spriteRenderer.color;
        spriteColor.a = 1;
        _spriteRenderer.color = spriteColor;

        TargetSpawner.Instance.SpawnTarget(); //spawn next target
        this.enabled = false;
    }

    private void CancelButtonPressed(InputAction.CallbackContext context)
    {
        if (!_isAiming) return;

        _lineRenderer.enabled = false;
        _isAiming = false;
    }

    private void Update() 
    {
        if (!_isAiming) return;

        Vector2 currAimPos = (Vector2)_centerOfTarget.position + ((Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - _aimStartPos);
        DrawTrajectory();
    }

    Vector2 pos1, pos2;
    //return length of line
    private void DrawTrajectory()
    {
        _lineRenderer.positionCount = Mathf.CeilToInt(_numLinePoints / _timeBetweenLinePoints) + 1;

        //set target velocty
        Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - _aimStartPos;
        Vector2 velocity = -direction * _fireSpeed;
        int i = 0;
        // _lineRenderer.SetPosition(i, _centerOfTarget.position);
        for (float time = 0; time < _numLinePoints; time += _timeBetweenLinePoints)
        {
            i++;
             
            Vector2 point = (Vector2)_centerOfTarget.position + time * velocity;
            point.y = _centerOfTarget.position.y + velocity.y * time + (Physics2D.gravity.y / 2f * time * time);
            _lineRenderer.SetPosition(i, point);

            Vector2 lastPosition = _lineRenderer.GetPosition(i - 1);
            pos1 = lastPosition;
            pos2 = point;
            RaycastHit2D hit = (Physics2D.Raycast(lastPosition, 
                (point - lastPosition).normalized, 
                (point - lastPosition).magnitude
                ));
            if (hit.collider != null)
            {
                _lineRenderer.SetPosition(i, hit.point);
                _lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
}
