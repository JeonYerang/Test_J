using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum State
{
    Idle,
    Move,
    Jump,
    Fall,
    Climb //��ٸ� Ÿ��?
}

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

    public State state;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        animator = GetComponentInChildren<Animator>();
        pv = GetComponent<PhotonView>();

        state = State.Idle;
    }

    private void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }

        Move();
        Gravity();

        if (controller.velocity.y < 0 && state != State.Climb)
        {
            state = State.Fall;
        }

        switch (state)
        {
            case State.Idle:
                if (inputDir != Vector2.zero)
                    state = State.Move;
                break;

            case State.Move:
                if (inputDir == Vector2.zero)
                    state = State.Idle;
                break;

            case State.Jump:
                if (gravityValue < 0.1)
                    state = State.Fall;
                break;

            case State.Fall:
                if (controller.isGrounded)
                    state = State.Idle;
                break;

            case State.Climb:

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

    public void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }

    private void Move()
    {
        Vector3 moveDir;
        float speed = moveSpeed;

        //����or���� �߿��� ������ �̵�
        if (state == State.Jump || state == State.Fall || state == State.Climb)
        {
            speed *= 0.5f;
        }

        if (state == State.Climb)
        {
            moveDir = new Vector3(inputDir.x, inputDir.y, 0);
            controller.Move(transform.TransformDirection(moveDir) * speed * Time.deltaTime);
        }
        else
        {
            moveDir = new Vector3(inputDir.x, 0, inputDir.y);
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        if (moveDir != Vector3.zero && state != State.Climb) //�������� ���� ���� ������ ����
            transform.forward = moveDir; //�����̴� �������� ĳ���� ������?

        //animator.SetFloat("RunSpeed", moveDir.magnitude);
    }

    private void Gravity() //�ڽ�ĳ��Ʈ�� �� ��Ʈ�Ӹ� �ɷ������� isGrounded üũ ���ֱ�
    {
        if (state == State.Climb)
            gravityValue = -0.5f;
        else
            gravityValue += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded)
        {
            gravityValue = Mathf.Max(0f, gravityValue);
        }

        controller.Move(Vector3.up * gravityValue * Time.deltaTime);
    }

    public void OnJump()
    {
        if (!controller.isGrounded)
            return;
        
        if (state != State.Jump)
            state = State.Jump;

        gravityValue = jumpPower;

        //animator.SetTrigger("OnJump");
    }
}
