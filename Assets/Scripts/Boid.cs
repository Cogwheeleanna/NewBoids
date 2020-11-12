using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Rigidbody rigid;

    // использовать метод для инициализации

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        //выбрать случайную позицию
        pos = Random.insideUnitSphere * Spawner.S.spawnRadius;

        //выбрать случайную скорость
        Vector3 vel = Random.onUnitSphere * Spawner.S.velocity;
        rigid.velocity = vel;

        LookAhead();

        //окрасить птицу в случайный цвет, но не слишком темный
        Color randColor = Color.black;
        while (randColor.r + randColor.g + randColor.b < 1.0f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }

        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            r.material.color = randColor;
        }
        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TintColor", randColor);
    }

    void LookAhead()
        //ориентировать птицу клювом в направлении полета
    {
        transform.LookAt(pos + rigid.velocity);
    }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private void FixedUpdate()
    {
        Vector3 vel = rigid.velocity;
        Spawner spn = Spawner.S;
        //движение в одну сторону аттрактор
        Vector3 delta = Attractor.POS - pos;
        //проветить куда двигаться
        bool attracted = (delta.magnitude > spn.attractPushDist);
        Vector3 velAttract = delta.normalized * spn.velocity;

        //применить все скорости
        float fdt = Time.fixedDeltaTime;

        if (attracted)
        {
            vel = Vector3.Lerp(vel, velAttract, spn.attractPull * fdt);
        }

        else
        { 
        vel = Vector3.Lerp(vel, -velAttract, spn.attractPush * fdt);
        }

        //установить vel в соотв с велосити в объекте одиночке
        vel = vel.normalized * spn.velocity;
        //присвоить скорость риджидбоди
        rigid.velocity = vel;
        //повернуть клювом в направлении движ
        LookAhead();
    }
}
