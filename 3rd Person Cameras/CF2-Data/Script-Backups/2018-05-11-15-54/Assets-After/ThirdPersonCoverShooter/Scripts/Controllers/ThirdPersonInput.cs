﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using ControlFreak2;


namespace CoverShooter
{
    /// <summary>
    /// Takes player input and translates that to commands to CharacterMotor.
    /// </summary>
    [RequireComponent(typeof(CharacterMotor))]
    public class ThirdPersonInput : MonoBehaviour
    {
        /// <summary>
        /// Camera mooved by this input component.
        /// </summary>
        public ThirdPersonCamera Camera
        {
            get
            {
                if (CameraOverride != null)
                    return CameraOverride;
                else
                {
                    if (CameraManager.Main != _cachedCameraOwner)
                    {
                        _cachedCameraOwner = CameraManager.Main;

                        if (_cachedCameraOwner == null)
                            _cachedCamera = null;
                        else
                            _cachedCamera = _cachedCameraOwner.GetComponent<ThirdPersonCamera>();
                    }

                    return _cachedCamera;
                }
            }
        }

        /// <summary>
        /// Camera to rotate around the player. If set to none it is taken from the main camera.
        /// </summary>
        [Tooltip("Camera to rotate around the player. If set to none it is taken from the main camera.")]
        public ThirdPersonCamera CameraOverride;

        /// <summary>
        /// Multiplier for horizontal camera rotation.
        /// </summary>
        [Tooltip("Multiplier for horizontal camera rotation.")]
        [Range(0, 10)]
        public float HorizontalRotateSpeed = 0.9f;

        /// <summary>
        /// Multiplier for vertical camera rotation.
        /// </summary>
        [Tooltip("Multiplier for vertical camera rotation.")]
        [Range(0, 10)]
        public float VerticalRotateSpeed = 1.0f;

        /// <summary>
        /// Is camera responding to mouse movement when the mouse cursor is unlocked.
        /// </summary>
        [Tooltip("Is camera responding to mouse movement when the mouse cursor is unlocked.")]
        public bool RotateWhenUnlocked = false;

        /// <summary>
        /// Maximum time in seconds to wait for a second tap to active rolling.
        /// </summary>
        [Tooltip("Maximum time in seconds to wait for a second tap to active rolling.")]
        public float DoubleTapDelay = 0.3f;

        private CharacterMotor _motor;
        private ThirdPersonController _controller;

        private Camera _cachedCameraOwner;
        private ThirdPersonCamera _cachedCamera;

        private float _timeW;
        private float _timeA;
        private float _timeS;
        private float _timeD;

        private void Awake()
        {
            _controller = GetComponent<ThirdPersonController>();
            _motor = GetComponent<CharacterMotor>();
        } 

        private void Update()
        {
            UpdateCamera();
            UpdateTarget();
            UpdateMovement();
            UpdateWeapons();
            UpdateReload();
            UpdateRolling();
            UpdateFireAndZoom();
            UpdateGrenade();
            UpdateCrouching();
            UpdateClimbing();
            UpdateCover();
            UpdateJumping();
        }

        protected virtual void UpdateMovement()
        {
            var movement = new CharacterMovement();

            var direction = ControlFreak2.CF2Input.GetAxis("Horizontal") * Vector3.right +
                            ControlFreak2.CF2Input.GetAxis("Vertical") * Vector3.forward;

            var lookAngle = Util.AngleOfVector(_controller.LookTargetInput - _motor.transform.position);

            if (direction.magnitude > float.Epsilon)
                movement.Direction = Quaternion.Euler(0, lookAngle, 0) * direction.normalized;

            if (_controller.ZoomInput)
                movement.Magnitude = 0.5f;
            else
            {
                if (_motor.Gun != null)
                {
                    if (ControlFreak2.CF2Input.GetButton("Run") && !_motor.IsCrouching)
                        movement.Magnitude = 2.0f;
                    else
                        movement.Magnitude = 1.0f;
                }
                else
                {
                    if (ControlFreak2.CF2Input.GetButton("Run"))
                        movement.Magnitude = 1.0f;
                    else
                        movement.Magnitude = 0.5f;
                }
            }

            _controller.MovementInput = movement;
        }

        protected virtual void UpdateClimbing()
        {
            if (ControlFreak2.CF2Input.GetButtonDown("Climb"))
                if (ControlFreak2.CF2Input.GetAxis("Vertical") > 0.1f)
                    if (_motor.IsInCover && _motor.CanClimbOrVault)
                        _motor.InputClimbOrVault();
        }

        protected virtual void UpdateCover()
        {
            if (ControlFreak2.CF2Input.GetButtonDown("TakeCover"))
                if (!_controller.AutoTakeCover && !_motor.IsInCover && _motor.PotentialCover != null)
                    _motor.InputTakeCover();
        }

        protected virtual void UpdateJumping()
        {
            if (ControlFreak2.CF2Input.GetButtonDown("Jump"))
                _motor.InputJump();
        }

        protected virtual void UpdateCrouching()
        {
            if (!_motor.IsSprinting && ControlFreak2.CF2Input.GetButton("Crouch"))
                _motor.InputCrouch();
        }

        protected virtual void UpdateGrenade()
        {
            if (_motor.HasGrenadeInHand)
            {
                if (ControlFreak2.CF2Input.GetButtonDown("Fire1"))
                    _controller.InputThrowGrenade();

                if (ControlFreak2.CF2Input.GetButtonDown("Fire2"))
                    _motor.InputCancelGrenade();

                if (!_motor.IsReadyToThrowGrenade)
                    if (ControlFreak2.CF2Input.GetButton("Grenade"))
                        _motor.InputTakeGrenade();
            }
            else
            {
                if (ControlFreak2.CF2Input.GetButton("Grenade"))
                    _motor.InputTakeGrenade();
            }
        }

        protected virtual void UpdateFireAndZoom()
        {
            if (ControlFreak2.CF2Input.GetButtonDown("Fire1")) _controller.FireInput = true;
            if (ControlFreak2.CF2Input.GetButtonDown("Fire2")) _controller.ZoomInput = true;
            if (ControlFreak2.CF2Input.GetButtonUp("Fire1")) _controller.FireInput = false;
            if (ControlFreak2.CF2Input.GetButtonUp("Fire2")) _controller.ZoomInput = false;
        }

        protected virtual void UpdateRolling()
        {
            if (_timeW > 0) _timeW -= Time.deltaTime;
            if (_timeA > 0) _timeA -= Time.deltaTime;
            if (_timeS > 0) _timeS -= Time.deltaTime;
            if (_timeD > 0) _timeD -= Time.deltaTime;

            if (ControlFreak2.CF2Input.GetButtonDown("RollForward"))
            {
                if (_timeW > float.Epsilon)
                {
                    if (_motor.IsInCover && _motor.CanClimbOrVault)
                        _motor.InputClimbOrVault();
                    else                    
                        _motor.InputRoll(_motor.LookAngle);
                }
                else
                    _timeW = DoubleTapDelay;
            }

            if (ControlFreak2.CF2Input.GetButtonDown("RollLeft"))
            {
                if (_timeA > float.Epsilon)
                    _motor.InputRoll(_motor.LookAngle - 90);
                else
                    _timeA = DoubleTapDelay;
            }

            if (ControlFreak2.CF2Input.GetButtonDown("RollBackward"))
            {
                if (_timeS > float.Epsilon)
                    _motor.InputRoll(_motor.LookAngle + 180);
                else
                    _timeS = DoubleTapDelay;
            }

            if (ControlFreak2.CF2Input.GetButtonDown("RollRight"))
            {
                if (_timeD > float.Epsilon)
                    _motor.InputRoll(_motor.LookAngle + 90);
                else
                    _timeD = DoubleTapDelay;
            }
        }

        protected virtual void UpdateWeapons()
        {
            if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha1)) { _motor.InputCancelGrenade(); _motor.InputWeapon(0); }
            if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha2)) { _motor.InputCancelGrenade(); _motor.InputWeapon(1); }
            if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha3)) { _motor.InputCancelGrenade(); _motor.InputWeapon(2); }
            if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha4)) { _motor.InputCancelGrenade(); _motor.InputWeapon(3); }
            if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha5)) { _motor.InputCancelGrenade(); _motor.InputWeapon(4); }
            if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha6)) { _motor.InputCancelGrenade(); _motor.InputWeapon(5); }
            if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha7)) { _motor.InputCancelGrenade(); _motor.InputWeapon(6); }
            if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha8)) { _motor.InputCancelGrenade(); _motor.InputWeapon(7); }
            if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha9)) { _motor.InputCancelGrenade(); _motor.InputWeapon(8); }
            if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha0)) { _motor.InputCancelGrenade(); _motor.InputWeapon(9); }

            if (ControlFreak2.CF2Input.mouseScrollDelta.y < 0)
            {
                if (_motor.CurrentWeapon == 0)
                    _motor.InputWeapon(_motor.Weapons.Length);
                else
                    _motor.InputWeapon(_motor.CurrentWeapon - 1);
            }
            else if (ControlFreak2.CF2Input.mouseScrollDelta.y > 0)
            {
                if (_motor.CurrentWeapon == _motor.Weapons.Length)
                    _motor.InputWeapon(0);
                else
                    _motor.InputWeapon(_motor.CurrentWeapon + 1);
            }
        }

        protected virtual void UpdateReload()
        {
            if (!_motor.HasGrenadeInHand)
                if (ControlFreak2.CF2Input.GetButton("Reload"))
                    _motor.InputReload();
        }

        protected virtual void UpdateTarget()
        {
            if (_controller == null)
                return;

            var camera = Camera;
            if (camera == null) return;

            var lookPosition = camera.transform.position + camera.transform.forward * 1000;

            _controller.LookTargetInput = lookPosition;
            _controller.GrenadeHorizontalAngleInput = Util.AngleOfVector(camera.transform.forward);
            _controller.GrenadeVerticalAngleInput = Mathf.Asin(camera.transform.forward.y) * 180f / Mathf.PI;

            var closestHit = Util.GetClosestHit(camera.transform.position, lookPosition, Vector3.Distance(camera.transform.position, _motor.Top), gameObject);

            if (_motor.TurnSettings.IsAimingPrecisely)
                closestHit += transform.forward;

            _controller.FireTargetInput = closestHit;
        }

        protected virtual void UpdateCamera()
        {
            var camera = Camera;
            if (camera == null) return;

            var scale = 1.0f;
            if (ControlFreak2.CFCursor.lockState == CursorLockMode.Locked || RotateWhenUnlocked)
            {


                if (_controller.IsZooming && _motor != null && _motor.Gun != null)
                    scale = 1.0f - _motor.Gun.Zoom / camera.StateFOV;
            }

            scale *= 2f;

            //Debug.Log(ControlFreak2.CF2Input.GetAxis("Mouse X") + ControlFreak2.CF2Input.GetAxis("Mouse Y"));
            //camera.Horizontal += ControlFreak2.CF2Input.GetAxis("Mouse X") * scale * 4f;
            camera.Horizontal  = Mathf.LerpAngle(camera.Horizontal, camera.Horizontal + ControlFreak2.CF2Input.GetAxis("Mouse X") * HorizontalRotateSpeed * scale, 1f);

            // camera.Vertical -= ControlFreak2.CF2Input.GetAxis("Mouse Y") * scale * 4f;

            camera.Vertical = Mathf.LerpAngle(camera.Vertical, camera.Vertical - ControlFreak2.CF2Input.GetAxis("Mouse Y") * VerticalRotateSpeed * scale, 1f);
                camera.UpdatePosition();
            //}
        }
    }
}