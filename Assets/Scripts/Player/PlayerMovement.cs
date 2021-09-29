using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRB;
    int floorMask;
    float camRayLength = 100f;
    float buffDuration;
    bool isBuffed = false;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(h, v);
        Turning();
        Animating(h,v);
    }

    public void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;

        playerRB.MovePosition(transform.position + movement);
    }

    private void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            playerRB.MoveRotation(newRotation);
        }
    }

    public void Animating(float h, float v)
    {
        bool isWalking = (h != 0f || v != 0f);
        anim.SetBool("IsWalking", isWalking);
    }


    #region tambahan

    // memberi player speed buff, dan mengakhiri buff tersebut ketika durasi nya sudah habis
    public void Buff(float duration, float multiplier)
    {
        if (isBuffed) return;
        buffDuration = duration;
        isBuffed = true;
        StartCoroutine(GetSpeedBuff(multiplier));

    }

    
    private IEnumerator GetSpeedBuff(float multiplier)
    {
        speed *= multiplier;
        yield return new WaitForSeconds(buffDuration);
        speed /= multiplier;
        isBuffed = false;
    }

    #endregion
}
