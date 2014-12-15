﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    private int m_playerId;
    public int PlayerId
    {
        get { return m_playerId; }
        set { m_playerId = value; }
    }
    private Transform m_transform;
    public Transform Transform
    {
        get { return m_transform; }
        set { m_transform = value; }
    }

    private int m_health;
    public int Health
    {
        get { return m_health; }
        set
        {
            m_health = Mathf.Clamp(value, 0, 100);
        }
    }

    private List<PlayerAction> m_playerAction;
    public List<PlayerAction> PlayerAction
    {
        get { return m_playerAction; }
    }

    public PlayerData(int playerId, Transform transform = null, int health = 100, List<PlayerAction> playerAction = null)
    {
        m_playerId = playerId;
        m_transform = transform;
        m_health = health;

        if (playerAction == null)
            m_playerAction = new List<PlayerAction>();
        else
            m_playerAction = playerAction;
    }
}
