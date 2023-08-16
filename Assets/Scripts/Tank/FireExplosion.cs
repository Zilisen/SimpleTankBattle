using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayExplosion());
    }

    IEnumerator PlayExplosion()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
