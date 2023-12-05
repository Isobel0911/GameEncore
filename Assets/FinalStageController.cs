using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalStageController : MonoBehaviour {
    AudioSource ac = null;
    GameObject[] texts;
    private bool hasStarted = false;
    public bool isCredit = false;

    void Awake() {
        ac = GameObject.Find("Main Camera")?.GetComponent<AudioSource>();
        texts = new GameObject[6];
        for (int i = 0; i < texts.Length; i++) {
            texts[i] = GameObject.Find($"GroupText{i}");
            texts[i]?.SetActive(false);
        }
    }

    void Update() {
        if (!hasStarted) {
            hasStarted = true;
            StartCoroutine(WaitStart());
        }
    }

    IEnumerator WaitStart() {
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(ShowTextsSequence());
    }

    IEnumerator ShowTextsSequence() {
        for (int i = 0; i < texts.Length; i++) {
            if (texts[i] != null) {
                texts[i].SetActive(true);
                yield return new WaitForSeconds(5);
                texts[i].SetActive(false);
                yield return new WaitForSeconds(2);
            }
        }

        StartCoroutine(FadeOutAudio());
    }

    IEnumerator FadeOutAudio() {
        if (ac != null) {
            float startVolume = ac.volume;
            while (ac.volume > 0) {
                ac.volume -= startVolume * Time.deltaTime / 3;
                yield return null;
            }
        }

        if (!isCredit) SceneManager.LoadScene("Menu");
    }
}
