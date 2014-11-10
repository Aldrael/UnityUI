using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterServerInterface : MonoBehaviour
{

    public float refreshHostListFrequency = 1.5f;

    ComboBox hostList;
    List<HostData> hosts = new List<HostData>();
    bool refreshHostList = true;
    bool hostsUpdated = false;

    public delegate void ServerSelected(HostData selectedServer);
    public event ServerSelected OnServerSelected;

    public HostData selectedHost;

    void Awake()
    {
        //Request the list as soon we're able to, so it's ready to display
        MasterServerUtils.RequestHostList();
    }

    // Use this for initialization
    void Start()
    {
        //Create the list and start the list refreshing
        hostList = ScriptableObject.CreateInstance<ComboBox>();
        OnServerSelected += JoiningServer;
        hostList.SetAllowMultiple(false);
        StartUpdatingHosts();
    }

    void JoiningServer(HostData selectedServer)
    {
        Debug.Log("Joining server: " + selectedServer.gameName);
    }

    void StartUpdatingHosts()
    {
        refreshHostList = true;
        StartCoroutine(UpdateHostsList());
    }

    void StopUpdatingHosts()
    {
        refreshHostList = false;
    }

    IEnumerator UpdateHostsList()
    {
        while (refreshHostList)
        {
            MasterServerUtils.RequestHostList();
            hosts = MasterServerUtils.ListHost();
            hostsUpdated = true;

            yield return new WaitForSeconds(refreshHostListFrequency);
        }
    }

    void OnListItemSelected(ComboBox.ComboItem selectedItem)
    {
        selectedHost = (HostData)selectedItem.value;
    }

    void OnListItemUnselected(ComboBox.ComboItem unselectedItem)
    {
        if (selectedHost == (HostData)unselectedItem.value)
        {
            selectedHost = null;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Check for an updated list, and refresh the items in the list when we have one.
        if (hostsUpdated)
        {
            hostsUpdated = false;
            hostList.Clear();
            foreach (HostData hostdata in hosts)
            {
                ComboBox.ComboItem item = hostList.AddItem(hostdata.gameName, hostdata, OnListItemSelected, OnListItemUnselected);
                //Maintain our selected host, even through a wipe and refresh
                //Would want to implement a more robust comparison between selectedHost and hostdata
                if (selectedHost != null && hostdata.gameName == selectedHost.gameName && hostdata.ip[0].Equals(selectedHost.ip[0]))
                {
                    item.Mark();
                    selectedHost = hostdata;
                }
            }
        }
    }

    void OnGUI()
    {
        //If we have hosts, list them, otherwise, display a "not found" message.
        if (hosts.Count > 0)
        {
            hostList.Draw(GUI.skin.box, new Rect(0, Screen.height / 2, 220, 300));
            if (GUI.Button(new Rect(0, Screen.height / 2 + 100, 220, 35), "Join selected"))
            {
                if (selectedHost != null)
                    OnServerSelected(selectedHost);
            }
        }
        else
        {
            GUI.Label(new Rect(Screen.width / 20, Screen.height / 2, 220, 30), "No hosts found.");
        }
    }

    //All monobehaviours will have this called when we can't connect to the master server
    // duplicating this function in multiple monobehaviours is fine, it will just duplicate
    // any response to the error (printing messages, trying a new master server, etc.).
    void OnFailedToConnectToMasterServer(NetworkConnectionError info)
    {
        MasterServerUtils.OnFailedToConnectToMasterServer(info);
    }
}
