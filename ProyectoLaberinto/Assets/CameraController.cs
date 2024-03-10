using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform personaje;
    public Transform camara_t;
    // Start is called before the first frame update
    void Start()
    {
        camara_t = GetComponent<Transform>();
        Debug.Log("La posición del personaje es:" + personaje.position);
        Debug.Log("La posición de la camara es:" + transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(personaje.position.x, personaje.position.y, transform.position.z);
    }
}
