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
    private Dictionary<string, Scene> SceneDict;
    public List<Scene> SceneList;
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

        SceneDict = new Dictionary<string, Scene>();

        foreach (Scene scene in SceneList)
        {
            SceneDict.Add(scene.name, scene);
        }
    }

    private void InitializeEvents()
    {
        //Subscribe to the static events from the character classes.
        PNJ.OnQuestFinished += CharacterToBeEaten;
        PNJ.OnDied  += KillCharacter;

    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
        currentScene = name;


        //If we're in bedroom and npcs need to be eaten, spawn them


        //Find all the PNJ scripts with the npc tags, if any are dead, deactivate their gameobject
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
