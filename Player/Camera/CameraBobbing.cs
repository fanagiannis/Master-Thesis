using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [SerializeField] private bool enable;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    private Camera cam;
    float timer;
    float BobbingSpeed=1f;
    float posy;
    void Awake()
    {
        cam=this.GetComponent<Camera>();
        posy=cam.transform.localPosition.y;
    }
    void Update()
    {
        Bob();
    }
    private void Bob()
    {
        if(enable)
        {
            timer+=Time.deltaTime*BobbingSpeed;
            cam.transform.localPosition += new Vector3(transform.localPosition.x,posy+Mathf.Sin(timer)*frequency,transform.localPosition.z);
        }
    }
}
