using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private bool m_debugging;
    [Header("Movement")]
    [SerializeField] private float m_moveSpeed;
    private Vector3 m_moveDir;

    [Header("Shooting")]
    [SerializeField] private float m_damage;
    [SerializeField] private float m_fireRate;
    private Vector3 m_aimDir;
    private float m_aimMagnitude { get { return m_aimDir.magnitude; } }
    [SerializeField, Range(0f, 1f)] private float m_aimDeadZone;
    //base projectile ref
    private float m_fireCoolDown;
    public bool CanFire { get { if (m_fireCoolDown <= 0) return true; else return false; } }

    /// <summary>
    /// Changes a stat
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool ChangeState(int index, float value)
    {
        switch (index)
        {
            case 0:
                m_moveSpeed += value;
                break;
            case 1:
                m_damage += value;
                break;
            case 2:
                m_fireRate += value;
                break;
        }
        return true;
    }
    public void Update()
    {
        //If we can't fire...
        if (!CanFire)
        {
            //..reduce the cool down
            m_fireCoolDown -= Time.deltaTime;
        }
        //Apply direction to the player object
        transform.position += m_moveDir * m_moveSpeed * Time.deltaTime;
        Color tmp_color = Color.blue;
        if(m_aimMagnitude >= m_aimDeadZone)
        {
            if (m_debugging) Debug.Log($"{gameObject} is firing");
            if (m_debugging) tmp_color = Color.red;
        }
        if (m_debugging) Debug.DrawRay(transform.position, m_aimDir * m_aimMagnitude * 5, tmp_color);
    }
    public void OnMove(InputValue value)
    {
        //Get the direction from the given input
        m_moveDir = value.Get<Vector2>();
        m_moveDir = new Vector3(m_moveDir.x, 0, m_moveDir.y);
        if (m_debugging) Debug.Log($"Moving {gameObject} by {m_moveDir}.");
    }
    public void OnFire(InputValue value)
    {
        //If we can fire...
        if (CanFire)
        {
            //Get the direction of the right stick
            m_aimDir = value.Get<Vector2>();
            //Apply it to the x & z axi
            m_aimDir = new Vector3(m_aimDir.x, 0, m_aimDir.y);
            if (m_debugging) Debug.Log($"{gameObject} is aiming towards {m_aimDir}.");
            //Set the cool down
            m_fireCoolDown = m_fireRate;
            //spawn projectile
        }
    }
}
