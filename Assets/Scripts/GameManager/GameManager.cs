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
    Dictionary<string, CharacterState> CharacterStateDict;

    public List<Character> CharactersList; //List to put character scriptobjects in the editor, do not use in script

    public List<GameObject> CurrentCharacterList;


    bool Stop = false;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            InitializeDictionaries();
            InitializeEvents();
        }
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Stop) return;

        foreach(var kvp in CharacterStateDict)
            if(kvp.Value.state != CharacterStatus.DEAD)
                return;

        Stop = true;
        StartCoroutine(TheEnd());
    }

    void InitializeDictionaries()
    {
        CharacterStateDict = new Dictionary<string, CharacterState>();
        CurrentCharacterList = new List<GameObject>();
        foreach (Character chara in CharactersList)
        {
            CharacterState state = new CharacterState();
            state.CharacterName = chara.Name;
            state.CharacterObject = chara;
            state.state = CharacterStatus.ALIVE;
            /**
            //DEBUG
            if(state.CharacterName == "Stella")
            {
                state.state = CharacterStatus.TO_BE_EATEN;
            }
            **/
            //ENDDEBUG
            CharacterStateDict.Add(chara.Name, state);
        }
    }

    void InitializeEvents()
    {
        //Subscribe to the static events from the character classes.
        PNJ.OnQuestFinished += CharacterToBeEaten;
        PNJ.OnDied  += KillCharacter;
        SceneChangeButton.OnSceneChange += ChangeScene;
    }

    void ReloadCurrentCharacters(Scene scene)
    {
        CurrentCharacterList.Clear();
        GameObject[] goarray = GameObject.FindGameObjectsWithTag("PNJ");
        foreach(GameObject component in goarray)
        {
            Character info = component.GetComponent<PNJ>().Info;
            CurrentCharacterList.Add(component);

            if (scene.name == "Room" && CharacterStateDict[info.name].state == CharacterStatus.ALIVE || CharacterStateDict[info.name].state == CharacterStatus.DEAD)
                component.SetActive(false);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Find all the PNJ scripts with the npc tags, if any are dead, deactivate their gameobject
        ReloadCurrentCharacters(scene);
        //If we're in Room, Activate the npcs that need to die;
        if (scene.name == "Room")
            ActivateCharacterToBeKilled();
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ActivateCharacterToBeKilled() 
    {
        foreach(var kvp in CharacterStateDict)
            if(kvp.Value.state == CharacterStatus.TO_BE_EATEN)
                foreach(GameObject chara in CurrentCharacterList)
                    if(chara.GetComponent<PNJ>().Info.Name == kvp.Value.CharacterName)
                    {
                        chara.gameObject.SetActive(true);
                        return;
                    }
    }
    public CharacterStatus GetCharacterStatus(string name)
    {
        return CharacterStateDict[name].state;
    }

    public void CharacterToBeEaten(PNJ pnj)
    {
        CharacterStateDict[pnj.Info.Name].state = CharacterStatus.TO_BE_EATEN;
        ChangeScene("Room");
    }

    public void KillCharacter(PNJ pnj)
    {
        CharacterStateDict[pnj.Info.Name].state = CharacterStatus.DEAD;
    }

    IEnumerator TheEnd()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("End");
    }
}
