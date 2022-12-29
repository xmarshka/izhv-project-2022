using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyEnemyStatus : MonoBehaviour
{
    private CharacterController characterController;
    private RoomManager roomManager;
    private Vector3 blowbackVector;

    [SerializeField] bool inBossRoom = false;

    [SerializeField] int health = 50;
    [SerializeField] int speed = 10;
    [SerializeField] float speedFadeIn = 3.0f;
    [SerializeField] int damage = 5;
    [SerializeField] float range = 1.5f;
    [SerializeField] int worth = 5;

    [SerializeField] float averageTimeToFire;

    [SerializeField] private float blowbackSpeed = 0.3f;
    [SerializeField] private float blowbackTime = 0.21f;
    [SerializeField] private float blowbackVelocityDecay = 0.02f;
    private float blowbackVelocity;

    [SerializeField] GameObject projectile;

    private GameObject player;

    private float currentSpeed;
    private bool moving;
    private bool readyToFire;
    private IEnumerator coroutine;

    public void Death()
    {
        Debug.Log(name + " died!");
        player.GetComponent<PlayerStatus>().AddCurrency(worth);
        if (!inBossRoom)
        {
            roomManager.EnemyDied();
        }
        Destroy(this.gameObject);
    }

    public void ApplyDamage(float damage)
    {
        health -= (int)damage;
        if (health <= 0)
        {
            Death();
            return;
        }

        moving = false;
        currentSpeed = speed / 2;

        StopCoroutine(coroutine);
        coroutine = WaitForSecondsToMove(blowbackTime);

        blowbackVector = (gameObject.transform.position - player.transform.position).normalized;
        StartCoroutine(Blowback());
        StartCoroutine(coroutine);
    }

    void Start()
    {
        currentSpeed = 0.0f;
        moving = true;
        StartCoroutine(WaitForSecondsToFire(averageTimeToFire));
        coroutine = WaitForSecondsToMove(blowbackTime);
        blowbackVelocity = blowbackSpeed;

        player = GameObject.Find("Player");
        characterController = gameObject.GetComponent<CharacterController>();
        if (!inBossRoom)
        {
            roomManager = GameObject.Find("Room Manager").GetComponent<RoomManager>();
        }
    }

    void Update()
    {
        if (moving)
        {
            transform.LookAt(player.transform.position);
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);

            if (readyToFire)
            {
                GameObject fired = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity);
                fired.GetComponent<Projectile>().target = player.transform.position;

                StartCoroutine(WaitForSecondsToFire(averageTimeToFire));
            }
        }
    }

    IEnumerator WaitForSecondsToFire(float time)
    {
        readyToFire = false;
        // yield on a new YieldInstruction that waits for time seconds.
        yield return new WaitForSeconds(time);
        readyToFire = true;
    }

    IEnumerator WaitForSecondsToMove(float time)
    {
        moving = false;
        // yield on a new YieldInstruction that waits for time seconds.
        yield return new WaitForSeconds(time);
        moving = true;
    }

    IEnumerator Blowback()
    {
        while(!moving)
        {
            characterController.Move(blowbackVelocity * Time.deltaTime * blowbackVector);
            blowbackVelocity -= blowbackVelocityDecay * Time.deltaTime;
            if (blowbackVelocity <= 0.2f)
            {
                blowbackVelocity = 0.2f;
            }
            yield return null;
        }

        blowbackVelocity = blowbackSpeed;
    }
}
