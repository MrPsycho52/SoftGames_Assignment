using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

[System.Serializable]
public class DialogueLine
{
    public string name;
    public string text;
}

[System.Serializable]
public class Emoji
{
    public string name;
    public string url;
}

[System.Serializable]
public class Avatar
{
    public string name;
    public string url;
    public string position; // optional now
}

[System.Serializable]
public class DialogueData
{
    public List<DialogueLine> dialogue;
    public List<Emoji> emojies;
    public List<Avatar> avatars;
}

public class Dialogue_Manager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image speakerAvatar;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image reactionImage;

    [Header("Dialogue Settings")]
    public string dialogueJsonUrl;

    private DialogueData dialogueData;
    private Dictionary<string, Avatar> avatarLookup;
    private int currentLine = 0;

    void Start()
    {
        StartCoroutine(LoadDialogueFromURL());
    }

    IEnumerator LoadDialogueFromURL()
    {
        UnityWebRequest www = UnityWebRequest.Get(dialogueJsonUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download dialogue JSON: " + www.error);
            yield break;
        }

        string jsonText = www.downloadHandler.text;
        dialogueData = JsonUtility.FromJson<DialogueData>(FixJson(jsonText));

        avatarLookup = new Dictionary<string, Avatar>();
        foreach (var a in dialogueData.avatars)
            avatarLookup[a.name] = a;

        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        while (currentLine < dialogueData.dialogue.Count)
        {
            var line = dialogueData.dialogue[currentLine];
            string cleanText = line.text;

            // Parse reaction
            string reaction = "";
            var match = Regex.Match(cleanText, @"{(.*?)}");
            if (match.Success)
            {
                reaction = match.Groups[1].Value;
                cleanText = Regex.Replace(cleanText, @"{.*?}", "").Trim();
            }

            nameText.text = line.name + " : ";
            dialogueText.text = "\"" + cleanText + "\"";

            // Show single avatar
            if (avatarLookup.TryGetValue(line.name, out Avatar speaker))
            {
                StartCoroutine(LoadImage(speaker.url, speakerAvatar));
            }

            // Show emoji reaction
            if (!string.IsNullOrEmpty(reaction))
            {
                var emoji = dialogueData.emojies.Find(e => e.name == reaction);
                if (emoji != null)
                {
                    reactionImage.gameObject.SetActive(true);
                    StartCoroutine(LoadImage(emoji.url, reactionImage));
                }
            }
            else
            {
                reactionImage.gameObject.SetActive(false);
            }

            // Wait for input
            yield return new WaitUntil(() => !Input.GetMouseButton(0));
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            currentLine++;
        }

        dialogueText.text = "The End!";
        nameText.text = "";
        reactionImage.gameObject.SetActive(false);
    }

    IEnumerator LoadImage(string url, Image target)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D tex = DownloadHandlerTexture.GetContent(www);
            target.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            target.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Failed to load image from {url}: {www.error}");
        }
    }

    string FixJson(string value)
    {
        if (value.TrimStart().StartsWith("{"))
            return value;
        return "{\"dialogue\":" + value + "}";
    }
}
