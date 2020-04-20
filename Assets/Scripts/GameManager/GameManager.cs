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

    public List<GameObject> CurrentCharacterList;


    //SCENES
    public string currentScene;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            InitializeDictionaries();
            InitializeEvents();

        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void InitializeDictionaries()
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
        ReloadCurrentCharacters();

        //If we're in Room, Activate the npcs that need to die;
        if (name == "Room")
        {
            KillCharacter();
        }
    }

    public void KillCharacter() 
    {
        foreach(var kvp in CharacterStateDict)
        {
            if(kvp.Value.state == CharacterStatus.TO_BE_EATEN)
            {
                foreach(GameObject chara in CurrentCharacterList)
                {
                    if(chara.GetComponent<PNJ>().Info.Name == kvp.Value.CharacterName)
                    {
                        chara.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private void ReloadCurrentCharacters()
    {
        CurrentCharacterList.Clear();
        GameObject[] goarray = GameObject.FindGameObjectsWithTag("PNJ");
        Debug.Log(goarray.Length);
        foreach(GameObject component in goarray)
        {
            
            Character info = component.GetComponent<PNJ>().Info;
            Debug.Log(info.Name);
            CurrentCharacterList.Add(component);
            if(CharacterStateDict[info.name].state == CharacterStatus.DEAD)
            {
                component.SetActive(false);
            }
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

}
