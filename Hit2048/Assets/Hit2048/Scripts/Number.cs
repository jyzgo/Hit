using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour {

    public Text _text;

    int _num = 0;

    // Use this for initialization
    public void Init(int x)
    {
        _num = x;
        _text.text = x.ToString();
    }

    public int GetNum()
    {
        return _num;
    }

    public void Fire()
    {
        isFire = true;
    }

    private void Update()
    {
        if (isFire)
        {
            transform.Translate(Vector3.up * 0.3f);
        }
    }

    bool isFire = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttached)
        {
            return;
        }
        
        RotateCircle circle = collision.GetComponent<RotateCircle>();
        if(circle != null)
        {
            isFire = false;
            Hitted();
            transform.SetParent(circle.transform);
            return;
        }

        Number num = collision.GetComponent<Number>();
        if(num != null)
        {
            isFire = false;
            Hitted();
            if (_num == num.GetNum())
            {
                num.Increase();
                Destroy(gameObject);
            }else if(num.isAttached)
            {
                transform.SetParent(num.transform);
            }
           
            return;
        }
    }

    public void Increase()
    {
        _num = _num * 2;
        _text.text = _num.ToString();
    }

    void Hitted()
    {
        isAttached = true;
        LevelMgr.Current.Hitted(_num);
    }

    public bool isAttached = false;


}
