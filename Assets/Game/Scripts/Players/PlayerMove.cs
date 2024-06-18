using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 5f;

    [SerializeField]
    FixedJoystick joystick;
    CharacterController controller;
    
    Animator animator;
    PhotonView pv;

    Vector2 inputDir;
    float gravityValue;

    public enum MoveState
    {
        Idle,
        Move,
        Jump,
        Fall,
        Climb //��ٸ� Ÿ��?
    }
    public MoveState moveState;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        animator = GetComponentInChildren<Animator>();
        pv = GetComponent<PhotonView>();

        moveState = MoveState.Idle;
    }

    private void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }

        Move();
        Gravity();

        if (controller.velocity.y < 0 && moveState != MoveState.Climb)
        {
            moveState = MoveState.Fall;
        }

        switch (moveState)
        {
            case MoveState.Idle:
                if (inputDir != Vector2.zero)
                    moveState = MoveState.Move;
                break;

            case MoveState.Move:
                if (inputDir == Vector2.zero)
                    moveState = MoveState.Idle;
                break;

            case MoveState.Jump:
                if (gravityValue < 0.1)
                    moveState = MoveState.Fall;
                break;

            case MoveState.Fall:
                if (controller.isGrounded)
                    moveState = MoveState.Idle;
                break;

            case MoveState.Climb:

                break;

            default:
                break;
        }

        controller.Move(Vector3.up * gravityValue * Time.deltaTime);
        //animator.SetInteger("State", (int)state);
    }

    public void Init(float moveSpeed, float jumpPower)
    {
        this.moveSpeed = moveSpeed;
        this.jumpPower = jumpPower;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (input != null)
            inputDir = input;
    }

    private void Move()
    {
        Vector3 moveDir;
        float speed = moveSpeed;

        //����or���� �߿��� ������ �̵�
        if (moveState == MoveState.Jump || moveState == MoveState.Fall || moveState == MoveState.Climb)
        {
            speed *= 0.5f;
        }

        if (moveState == MoveState.Climb)
        {
            moveDir = new Vector3(inputDir.x, inputDir.y, 0);
            controller.Move(transform.TransformDirection(moveDir) * speed * Time.deltaTime);
        }
        else
        {
            moveDir = new Vector3(inputDir.x, 0, inputDir.y);
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        if (moveDir != Vector3.zero && moveState != MoveState.Climb) //�������� ���� ���� ������ ����
            transform.forward = moveDir; //�����̴� �������� ĳ���� ������?

        //animator.SetFloat("RunSpeed", moveDir.magnitude);
    }

    private void Gravity() //�ڽ�ĳ��Ʈ�� �� ��Ʈ�Ӹ� �ɷ������� isGrounded üũ ���ֱ�
    {
        if (moveState == MoveState.Climb)
            gravityValue = -0.5f;
        else
            gravityValue += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded)
        {
            gravityValue = Mathf.Max(0f, gravityValue);
        }

        controller.Move(Vector3.up * gravityValue * Time.deltaTime);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) //���ȴ��� üũ
            return;

        if (!controller.isGrounded)
            return;

        if (moveState != MoveState.Jump)
            moveState = MoveState.Jump;

        gravityValue = jumpPower;

        //animator.SetTrigger("OnJump");
    }

    public void OnJump()
    {
        if (!controller.isGrounded)
            return;

        if (moveState != MoveState.Jump)
            moveState = MoveState.Jump;

        gravityValue = jumpPower;

        //animator.SetTrigger("OnJump");
    }
}
