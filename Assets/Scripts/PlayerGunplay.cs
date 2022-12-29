using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunplay : MonoBehaviour
{
    private PlayerStatus playerStatus;

    [SerializeField] GameObject weapon;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] AudioClip enemyHit;

    private GameObject gameCamera;

    public Vector3 cameraRayIntersect;
    private Plane plane = new Plane(Vector3.up, 0);

    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;

    void Shoot(Vector3 target)
    {
        int layerMask = (1 << 3) | (1 << 7); // Layer 3 = Enemy, Layer 7 = Boss

        RaycastHit hit;

        Debug.DrawRay(transform.position, (target - transform.position), Color.red, 1);
        if (Physics.Raycast(transform.position, (target - transform.position), out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("Did Hit");
            if (hit.transform.gameObject.GetComponent<EnemyStatus>())
            {
                hit.transform.gameObject.GetComponent<EnemyStatus>().ApplyDamage(playerStatus.damage);
            }
            else if (hit.transform.gameObject.GetComponent<FlyEnemyStatus>())
            {
                hit.transform.gameObject.GetComponent<FlyEnemyStatus>().ApplyDamage(playerStatus.damage);
            }
            else if (hit.transform.gameObject.GetComponent<Boss>())
            {
                hit.transform.gameObject.GetComponent<Boss>().ApplyDamage(playerStatus.damage);
            }
            weapon.GetComponent<AudioSource>().PlayOneShot(enemyHit);
        }
    }

    void Start()
    {
        playerStatus = gameObject.GetComponent<PlayerStatus>();
        gameCamera = GameObject.Find("Main Camera");
        weapon = gameObject.transform.Find("Weapon").gameObject;
    }

    void Update()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            cameraRayIntersect = ray.GetPoint(distance);
            if (Input.GetMouseButtonDown(0))
            {
                Shoot(cameraRayIntersect + Vector3.up);
                gameCamera.transform.position += Vector3.right * 0.5f + Vector3.forward * 0.5f;
                weapon.GetComponent<AudioSource>().PlayOneShot(weapon.GetComponent<AudioSource>().clip);
            }            
        }
        transform.LookAt(cameraRayIntersect + Vector3.up * 0.5f);
        transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void LateUpdate()
    {
        // update position
        Vector3 targetPosition = transform.position + cameraOffset;
        gameCamera.transform.position = Vector3.SmoothDamp(gameCamera.transform.position, targetPosition, ref velocity, smoothTime);

        // update rotation
        gameCamera.transform.LookAt(transform.position);
    }
}
