using UnityEngine;
using UnityEditor;
using TMPro;

public class GameUISetupHelper : MonoBehaviour
{
    [MenuItem("GoalForge/Setup GameUI PowerUp Texts")]
    public static void SetupPowerUpTexts()
    {
        GameUI gameUI = FindObjectOfType<GameUI>();
        if (gameUI == null)
        {
            Debug.LogError("❌ GameUI not found in scene!");
            return;
        }

        Canvas canvas = gameUI.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("❌ Canvas not found!");
            return;
        }

        CreatePowerUpText(canvas.transform, "Player1PowerUpText", new Vector2(-300f, 200f), gameUI, 1);
        CreatePowerUpText(canvas.transform, "Player2PowerUpText", new Vector2(300f, 200f), gameUI, 2);

        Debug.Log("✅ PowerUp texts created and linked to GameUI!");
    }

    static void CreatePowerUpText(Transform parent, string name, Vector2 position, GameUI gameUI, int playerNumber)
    {
        Transform existing = parent.Find(name);
        GameObject textObj;
        
        if (existing != null)
        {
            textObj = existing.gameObject;
        }
        else
        {
            textObj = new GameObject(name);
            textObj.transform.SetParent(parent, false);
        }

        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        if (rectTransform == null)
            rectTransform = textObj.AddComponent<RectTransform>();

        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(300f, 50f);

        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        if (text == null)
            text = textObj.AddComponent<TextMeshProUGUI>();

        text.text = "";
        text.fontSize = 24;
        text.color = Color.yellow;
        text.alignment = TextAlignmentOptions.Center;
        text.fontStyle = FontStyles.Bold;

        textObj.SetActive(false);

        SerializedObject so = new SerializedObject(gameUI);
        string propertyName = playerNumber == 1 ? "player1PowerUpText" : "player2PowerUpText";
        so.FindProperty(propertyName).objectReferenceValue = text;
        so.ApplyModifiedProperties();

        Debug.Log($"✅ Created and linked {name}");
    }
}

