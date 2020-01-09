using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[InitializeOnLoad]
public class ScrollViewAdapter : MonoBehaviour
{
    public RectTransform prefab;
    public RectTransform content;

    // Start is called before the first frame update
    void Start()
    {
        GetMatchViewElements();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MatchModel[] GetMatches()
    {
        try
        {
            GameObject network = this.FindObjectByTag("Network");
            var manager = network?.GetComponent<NetworkManager>();

            if (manager.matches != null)
            {
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
            else
            {
                return Array.Empty<MatchModel>();
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error, in GetMatches something went wrong: { ex.Message }");
            return Array.Empty<MatchModel>();
        }
    }

    public void GetMatchViewElements()
    {
        var matches = GetMatches();

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var match in matches)
        {
            var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            InitializeInstanceView(instance, match);
        }
    }

    public void InitializeInstanceView(GameObject viewGameObject, MatchModel model)
    {
        MatchViewModel view = new MatchViewModel(viewGameObject.transform);
        view.MatchName.text = model.MatchName;
        view.MatchSize.text = model.MatchSize.ToString();
    }

    private GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }

    public class MatchModel
    {
        public string MatchName { get; set; }
        public int MatchSize { get; set; }
    }

    public class MatchViewModel
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
