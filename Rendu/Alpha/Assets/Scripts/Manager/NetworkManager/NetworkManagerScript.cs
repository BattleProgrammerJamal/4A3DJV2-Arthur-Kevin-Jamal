﻿/**
 * @Author : Arthur TORRENTE
 * @Date : 07/12/2014
 * @Desc : Gestion du réseaux
 * @LastModifier : Arthur TORRENTE
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManagerScript : MonoBehaviour
{
    public static NetworkManagerScript m_instance;

    [SerializeField]
    private GameManagerScript m_gameManager;

    [SerializeField]
    private NetworkView m_networkView;

    [SerializeField]
    private bool m_buildServer;
    public bool BuildServer
    {
        get { return m_buildServer; }
        set { m_buildServer = value; }
    }

    [SerializeField]
    private string m_ip;
    public string Ip
    {
        get { return m_ip; }
        set { m_ip = value; }
    }

    [SerializeField]
    private int m_port;
    public int Port
    {
        get { return m_port; }
        set { m_port = value; }
    }

    [SerializeField]
    private int m_maxPlayer;
    public int MaxPlayer
    {
        get { return m_maxPlayer; }
        set { m_maxPlayer = value; }
    }

    /* Fait le matching playerID et NetworkPlayer */
    private List<NetworkPlayer> m_players;
    public List<NetworkPlayer> Players
    {
        get { return m_players; }
    }

    [SerializeField]
    private bool m_setup;
    public bool Setup
    {
        get { return m_setup; }
        set { m_setup = value; }
    }


    void Awake()
    {
        if(m_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            m_instance = this;

            m_players = new List<NetworkPlayer>();
        }
        else if(m_instance != this)
        {
            mergeInstance();
            Destroy(gameObject);
        }
    }

    void mergeInstance()
    {
        m_instance.Setup = m_setup;
    }

	void Start ()
    {
        if(m_setup)
        {
            if (m_buildServer)
                setupServer();
            else
                setupClient();
        }
	}
	
    void setupServer()
    {
        Network.InitializeSecurity();
        NetworkConnectionError err = Network.InitializeServer(m_maxPlayer, m_port, !Network.HavePublicAddress());

        if (err != NetworkConnectionError.NoError)
            Debug.LogError(err.ToString());
        else
            Debug.LogError("Server start");
    }

    void setupClient()
    {
        NetworkConnectionError err = Network.Connect(m_ip, m_port);

        if (err != NetworkConnectionError.NoError)
            Debug.LogError(err.ToString());
        else
            Debug.LogError("Client connect");
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        if(m_setup)
        {
            if (m_players.Count < m_maxPlayer)
            {
                Debug.LogError("Player add" + player.ToString());
                m_networkView.RPC("addPlayer", RPCMode.AllBuffered, player);

                /*if (m_players.Count == m_maxPlayer - 1)
                {
                    Network.RemoveRPCsInGroup(0);
                    Network.RemoveRPCsInGroup(1);
                    m_networkView.RPC("RPCLoadLevel", RPCMode.AllBuffered, "Planification");
                }*/
            }
        }
        else
        {
            Debug.LogError("New Connection not Allow");
        }
    }

    [RPC]
    void addPlayer(NetworkPlayer newPlayer)
    {
        m_players.Add(newPlayer);
        m_gameManager.newPlayer(m_players.Count - 1);
    }

    [RPC]
    void RPCLoadLevel(string name)
    {
        Network.SetSendingEnabled(0, false);
        Network.isMessageQueueRunning = false;
    }

    public int getPlayerId(NetworkPlayer player)
    {
        for (int i = 0; i < m_players.Count; ++i)
            if (m_players[i] == player)
                return i;

        return -1;
    }
}
