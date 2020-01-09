using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScrollViewAdapter : MonoBehaviour
{
    public RectTransform prefab;
    public RectTransform content;

    private void Start()
    {
        StartCoroutine(GetMatchViewElements(true));
    }

    public void RefreshServersMatches()
    {
        StartCoroutine(GetMatchViewElements());
    }

    private MatchModel[] GetMatches()
    {
        try
        {
            GameObject network = this.FindObjectByTag("Network");
            var manager = network?.GetComponent<NetworkManager>();

            var matches = new MatchModel[manager.matches.Count];
            for (int i = 0; i < matches.Length; i++)
            {
               matches[i] = new MatchModel()
               {
                   MatchName = manager.matches[i].name,
                   MatchSize = manager.matches[i].currentSize
               };
            }

            return matches;
        }
        catch (Exception ex)
        {
            Debug.Log($"Error, something went wrong: { ex.Message }");
            return Array.Empty<MatchModel>();
        }
    }

    public IEnumerator GetMatchViewElements(bool isStart = false)
    {
        if (isStart)
        {
            yield return new WaitForSeconds(0.5f);
        }

        var matches = GetMatches();

        if (content == null || matches == null || prefab == null)
        {
            Debug.Log($"Error, something went wrong");
            yield break;
        }

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var match in matches)
        {
            var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
            instance?.transform.SetParent(content, false);
            InitializeInstanceView(instance, match);
        }
    }

    private void InitializeInstanceView(GameObject viewGameObject, MatchModel model)
    {
        if (viewGameObject == null)
        {
            return;
        }

        MatchViewModel view = new MatchViewModel(viewGameObject.transform);
        view.MatchName.text = model.MatchName;
        view.MatchSize.text = model.MatchSize.ToString();
    }

    private GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }

    private class MatchModel
    {
        public string MatchName { get; set; }
        public int MatchSize { get; set; }
    }

    private class MatchViewModel
    {
        public Text MatchName { get; set; }
        public Text MatchSize { get; set; }

        public MatchViewModel(Transform rootView)
        {
            MatchName = rootView.Find("MatchName").GetComponent<Text>();
            MatchSize = rootView.Find("MatchSize").GetComponent<Text>();
        }
    }
}
