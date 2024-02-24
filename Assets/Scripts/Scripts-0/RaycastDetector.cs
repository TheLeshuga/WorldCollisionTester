using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    public GameObject objectToDetect; // El GameObject que deseamos detectar con el rayo

    void Update()
    {
        // Dispara un rayo hacia adelante desde la posición y dirección del objeto
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            // Comprueba si la colisión fue con el GameObject que deseamos detectar
            if (hit.collider.gameObject == objectToDetect)
            {
                Debug.Log("Colisión detectada con: " + objectToDetect.name);
                // Aquí puedes agregar cualquier lógica adicional que desees realizar cuando se detecta la colisión
            }
        }

        // Visualiza el rayo en el Editor
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);
    }
}

