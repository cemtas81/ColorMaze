using UnityEngine;
using TMPro;
using Dan.Main;
using System.Collections;

namespace LeaderboardCreatorDemo
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _entryTextObjects;
        [SerializeField] private TMP_InputField _usernameInputField;
        [SerializeField] private CanvasGroup _leaderboardLoadingPanel;
        // Make changes to this section according to how you're storing the player's score:
        // ------------------------------------------------------------
        //[SerializeField] private ExampleGame _exampleGame;

        public int Score;
        // ------------------------------------------------------------

        private void Start()
        {
            Score = PlayerPrefs.GetInt("MainScore");
            LoadEntries();
            StartCoroutine(LoadingTextCoroutine(_leaderboardLoadingPanel.GetComponentInChildren<TextMeshProUGUI>()));
        }

        private IEnumerator LoadingTextCoroutine(TMP_Text text)
        {
            var loadingText = "Loading";
            for (int i = 0; i < 3; i++)
            {
                loadingText += ".";
                text.text = loadingText;
                yield return new WaitForSeconds(0.25f);
            }

            StartCoroutine(LoadingTextCoroutine(text));
        }

        private void LoadEntries()
        {
            // Q: How do I reference my own leaderboard?
            // A: Leaderboards.<NameOfTheLeaderboard>

            Leaderboards.MazeCem.GetEntries(entries =>
            {
                foreach (var t in _entryTextObjects)
                    t.text = "";
                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                    _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";

                // Toggle the loading panel off only after the entries are successfully loaded
                ToggleLoadingPanel(false);
            });
            ToggleLoadingPanel(true);
        }

        private void ToggleLoadingPanel(bool isOn)
        {
            _leaderboardLoadingPanel.alpha = isOn ? 1f : 0f;
            _leaderboardLoadingPanel.interactable = isOn;
            _leaderboardLoadingPanel.blocksRaycasts = isOn;
        }

        public void UploadEntry()
        {
            Leaderboards.MazeCem.UploadNewEntry(_usernameInputField.text, Score, isSuccessful =>
            {
                if (isSuccessful)
                    LoadEntries();
            });
        }
    }
}
