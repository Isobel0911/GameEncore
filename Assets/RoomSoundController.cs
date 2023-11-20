using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class RoomSoundController : MonoBehaviour {
    private GameObject mainCharacter;
    private List<GameObject> NPCs;
    private BoxCollider[] detectors;
    private BoxCollider[] subDetectors;
    public GameObject[] doors;
    private bool[][] npc;
    private bool[] allMain;
    private int detectorCount = 0;
    private string name;
    private static bool switchState = false;
    private static bool[] counter;
    private bool localState = false;
    private static bool[] states;
    public int idx;

    private int ExtractNumber(string name) {
        Match match = Regex.Match(name, @"\d+");
        if (match.Success) {
            return int.Parse(match.Value);
        }
        return -1; // Return -1 or some other value to indicate that no number was found
    }

    void Start() {
        name = gameObject.name;
        List<BoxCollider> detectorList = new List<BoxCollider>();
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform) {
            BoxCollider collider = child.GetComponent<BoxCollider>();
            if (child.name.StartsWith("Detect ")) {
                detectorList.Add(collider);
            } else {
                children.Add(child);
            }
            RoomSubDetector detectorScript = child.gameObject.AddComponent<RoomSubDetector>();
            detectorScript.SetController(this);
            detectorScript.idx = detectorCount;
            detectorCount++;
        }
        allMain = new bool[detectorCount];

        // Sort the children based on the numeric part of their names
        List<Transform> sortedChildren = children
            .Where(child => child.name.StartsWith("SubDetect "))
            .OrderBy(child => ExtractNumber(child.name))
            .ToList();

        List<BoxCollider> subDetectorList = new List<BoxCollider>();

        foreach (Transform child in sortedChildren) {
            subDetectorList.Add(child.GetComponent<BoxCollider>());
        }

        detectors = detectorList.ToArray();
        subDetectors = subDetectorList.ToArray();

        mainCharacter = GameObject.Find("PlayerArmature");
        NPCs = new List<GameObject>(GameObject.FindGameObjectsWithTag("NPC"));
        npc = new bool[NPCs.Count][]; // Initialize the outer array
        states = new bool[NPCs.Count];
        for (int i = 0; i < NPCs.Count; i++) {
            npc[i] = new bool[detectorCount]; // Initialize each inner array
        }
        if (counter == null) counter = new bool[7];
    }

    void Update() {
        for (int i = 0; i < doors.Length; i++) {
            subDetectors[i].enabled = (Mathf.Abs(doors[i].transform.eulerAngles.y) > 30f);
        }
        if (switchState == localState) {
            switchState = !switchState;
            for (int i = 0; i < NPCs.Count; i++) {
                states[i] = false;
            }
        }
        localState = !localState;
        if (mainCheck()) {
            for (int i = 0; i < NPCs.Count; i++) {
                for (int j = 0; j < detectorCount; j++) {
                    states[i] = states[i] || npc[i][j];
                }
            }
        }
        counter[idx] = true;
        if (checkCounter()) {
            resetCounter();
            for (int i = 0; i < NPCs.Count; i++) {
                AudioSource audioSource = NPCs[i].GetComponent<AudioSource>();
                if (states[i] != audioSource.enabled) {
                    audioSource.enabled = states[i];
                }
            }
        }
    }

    public void ableMain(int i) { allMain[i] = true; }
    public void disableMain(int i) { allMain[i] = false; }
    public void ableNPC(string name, int idx) {
        for (int i = 0; i < NPCs.Count; i++) {
            if (NPCs[i].name == name) {
                npc[i][idx] = true;
                return;
            }
        }
    }
    public void disableNPC(string name, int idx) {
        for (int i = 0; i < NPCs.Count; i++) {
            if (NPCs[i].name == name) {
                npc[i][idx] = false;
                return;
            }
        }
    }
    bool mainCheck() {
        for (int j = 0; j < detectorCount; j++)
            if(allMain[j]) return true;
        return false;
    }
    public string getName() {return name;}
    bool checkCounter() {
        for (int i = 0; i < counter.Length; i++) {
            if (!counter[i]) return false;
        }
        return true;
    }
    void resetCounter() {
        for (int i = 0; i < counter.Length; i++) {
            counter[i] = false;
        }
    }
}
