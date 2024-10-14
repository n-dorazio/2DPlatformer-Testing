using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound; //Sound when picking up a checkpoint
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        //Check if checkpoint available

        if(currentCheckpoint == null)
        {
            //show game over screen
            uiManager.GameOver();
            return;
        }
        transform.position = currentCheckpoint.position; //move player to checkpoint position
        playerHealth.Respawn(); //restore player health and reset animation

        //Move camera to checkpoint room (Checkpoint must be  placed as a child of the room to work)
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }


    //Activate checkpoints

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; //Deactivate checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("appear"); //Play animation when checkpoint is reached
        }
    }
}


