using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameLogic : MonoBehaviour
{
    GameObject character;

    int playerID;

    Vector2 characterPositionInPercent;
    Vector2 characterVelocityInPercent;
    const float CharacterSpeed = 0.25f;
    float DiagonalCharacterSpeed;

    Sprite circleTexture;

    Dictionary<int,Player> _players = new Dictionary<int,Player>();

    void Start()
    {
        DiagonalCharacterSpeed = Mathf.Sqrt(CharacterSpeed * CharacterSpeed + CharacterSpeed * CharacterSpeed) /2f;
        NetworkedClientProcessing.SetGameLogic(this);

        circleTexture = Resources.Load<Sprite>("Circle");
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            characterVelocityInPercent = Vector2.zero;

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                characterVelocityInPercent.x = DiagonalCharacterSpeed;
                characterVelocityInPercent.y = DiagonalCharacterSpeed;
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                characterVelocityInPercent.x = -DiagonalCharacterSpeed;
                characterVelocityInPercent.y = DiagonalCharacterSpeed;
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                characterVelocityInPercent.x = DiagonalCharacterSpeed;
                characterVelocityInPercent.y = -DiagonalCharacterSpeed;
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                characterVelocityInPercent.x = -DiagonalCharacterSpeed;
                characterVelocityInPercent.y = -DiagonalCharacterSpeed;
            }
            else if (Input.GetKey(KeyCode.D))
                characterVelocityInPercent.x = CharacterSpeed;
            else if (Input.GetKey(KeyCode.A))
                characterVelocityInPercent.x = -CharacterSpeed;
            else if (Input.GetKey(KeyCode.W))
                characterVelocityInPercent.y = CharacterSpeed;
            else if (Input.GetKey(KeyCode.S))
                characterVelocityInPercent.y = -CharacterSpeed;


            characterVelocityInPercent *=  Time.deltaTime;

            
        }
        /* characterPositionInPercent += (characterVelocityInPercent * Time.deltaTime);*/

        NetworkedClientProcessing.SendNewPositionToServer(characterVelocityInPercent);
    }

    public void ApplyMovementFromServer(Vector2 positionChange, int characterID)
    {
        Player player = _players[characterID];
        player.posInPercent = positionChange;

        ApplyPercentagePositionToScreen(player.avatar,player.posInPercent);

        Debug.Log(positionChange + " ppp ");
        Debug.Log(player.posInPercent);

        _players[characterID] = player;

    }

    void ApplyPercentagePositionToScreen(GameObject avatar, Vector2 characterPositionInPercent)
    {
        Debug.Log("iii " + characterPositionInPercent);
        Vector2 screenPos = new Vector2(characterPositionInPercent.x * (float)Screen.width, characterPositionInPercent.y * (float)Screen.height);
        Vector3 characterPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
        characterPos.z = 0;

        avatar.transform.position = characterPos;
    }

    public void AddMyPlayerIntoGame(int MyID)
    {
        playerID = MyID;

        AddCharacterIntoGame(MyID, Vector2.zero);
        _players[MyID].avatar.GetComponent<SpriteRenderer>().color = Color.green;


    }

    public void AddCharacterIntoGame(int ID, Vector2 position)
    {
        Player other = new Player();
        other.posInPercent = position;
        other.avatar = CreateNewPlayer(Color.red);

        ApplyPercentagePositionToScreen(other.avatar,position);

        _players.Add(ID, other);


    }

    GameObject CreateNewPlayer(Color color)
    {
        character = new GameObject("Character");

        character.AddComponent<SpriteRenderer>();
        character.GetComponent<SpriteRenderer>().sprite = circleTexture;
        character.GetComponent<SpriteRenderer>().color = color;

        return character;
    }

}

