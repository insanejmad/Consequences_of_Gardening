using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CharacterStatus
{
    ALIVE,
    DEAD,
    TO_BE_EATEN,
}

public class CharacterState
{
    public Character CharacterObject;
    public string CharacterName;
    public CharacterStatus state;
}
public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;


    //CHARACTERS
    private Dictionary<string, CharacterState> CharacterStateDict;

    public List<Character> CharactersList; //List to put character scriptobjects in the editor, do not use in script

    public List<Character> CurrentCharacterList;


    //SCENES
    public string currentScene;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeDictionaries();
        InitializeEvents();
    }

    private void InitializeDictionaries()
    {
        CharacterStateDict = new Dictionary<string, CharacterState>();

        foreach (Character chara in CharactersList)
        {
            CharacterState state = new CharacterState();
            state.CharacterName = chara.Name;
            state.CharacterObject = chara;
            state.state = CharacterStatus.ALIVE;
            CharacterStateDict.Add(chara.Name, state);
        }


    }

    private void InitializeEvents()
    {
        //Subscribe to the static events from the character classes.
        PNJ.OnQuestFinished += CharacterToBeEaten;
        PNJ.OnDied  += KillCharacter;
        SceneChangeButton.OnSceneChange += ChangeScene;
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
        currentScene = name;
        //Find all the PNJ scripts with the npc tags, if any are dead, deactivate their gameobject
        //ReloadCurrentCharacters();
    }

    private void ReloadCurrentCharacters()
    {
        CurrentCharacterList.Clear();
        PNJ[] goarray = GameObject.FindObjectsOfType<PNJ>();
        foreach(PNJ component in goarray)
        {
            Character info = component.GetComponent<PNJ>().Info;
            CurrentCharacterList.Add(info);
            if(CharacterStateDict[info.name].state == CharacterStatus.DEAD)
            {
                component.gameObject.SetActive(false);
            }
        }
    }

    public void CharacterToBeEaten(PNJ pnj)
    {
        CharacterStateDict[pnj.Info.Name].state = CharacterStatus.TO_BE_EATEN;
        //ChangeScene("Bedroom");
    }

    public void KillCharacter(PNJ pnj)
    {
        CharacterStateDict[pnj.Info.Name].state = CharacterStatus.DEAD;
    }

}
