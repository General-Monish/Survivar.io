using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehaviour : ProjectileBehaviour
{
    knife kniffe;
    // Start is called before the first frame update
   protected override void Start()
    {
        base.Start();
        kniffe = FindObjectOfType<knife>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * kniffe.speed * Time.deltaTime;
    }
}
