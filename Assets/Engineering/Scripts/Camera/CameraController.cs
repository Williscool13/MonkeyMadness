using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;

    [SerializeField] List<Transform> players = new List<Transform>();

    [SerializeField] Vector2 borderPadding;
    [DetailedInfoBox("This is a proportion of the current distance between the players that the camera will add to the size of the camera.",
        "1 = 100% of the distance between the players will be added to the camera size. 0.5 = 50% of the distance between the players will be added to the camera size.")]
    [SerializeField] Vector2 screenOFfset;
    [SerializeField] float cameraGrowSpeed;
    [SerializeField] float cameraMoveSpeed;


    void Update()
    {
        if (players.Count == 0) return;

        if (players.Count == 1) {

            cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, 5, cameraGrowSpeed * Time.deltaTime);
            Vector3 targetPos = players[0].position;
            targetPos.z = -10f;
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, cameraMoveSpeed * Time.deltaTime);


            return;
        }

        CamInfo ci = GetMaxDistance();
        if (ci.first == null || ci.second == null) {
            Debug.Log("No players found");
            return;
        }

        Vector3 targetPosition = (ci.first.position + ci.second.position) / 2;
        targetPosition.z = -10f;
        targetPosition.x += screenOFfset.x;
        targetPosition.y += screenOFfset.y;

        //float targetSize = GetCameraSize();
        float targetSize = ci.distance;


        cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, targetSize, cameraGrowSpeed * Time.deltaTime);
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, cameraMoveSpeed * Time.deltaTime);
    }


    CamInfo GetMaxDistance() {
        float highestDistancePlayers = -1;
        Transform p1 = null;
        Transform p2 = null;
        for (int i = 0; i < players.Count; i++) {
            for (int j = 0; j < players.Count; j++) {
                if (i == j) continue;
                float distanceBetweenPlayers = Vector3.Distance(players[i].position, players[j].position);
                if (distanceBetweenPlayers > highestDistancePlayers){
                    highestDistancePlayers = distanceBetweenPlayers;
                    p1 = players[i];
                    p2 = players[j];
                }
            }
        }

        CamInfo ci = new CamInfo();

        if (p1 == null || p2 == null) {
            ci.first = null;
            ci.second = null;
            ci.distance = 0;
            return ci;
        } 

        float xDist = Mathf.Abs(p1.position.x - p2.position.x);
        float yDist = Mathf.Abs(p1.position.y - p2.position.y);

        float xDiff = xDist / 2;
        float yDiff = yDist / 2;

        float xSize = xDiff + borderPadding.x + screenOFfset.x;
        float ySize = yDiff + borderPadding.y + screenOFfset.y;

        ci.first = p1;
        ci.second = p2;
        ci.distance = Mathf.Max(xSize, ySize);

        return ci;
    }

    public void RegisterPlayer(Transform t) {
        players.Add(t);
    }

    public void UnregisterPlayer(Transform t) {
        for (int i = 0; i < players.Count; i++) {
            if (players[i] == t) {
                players.RemoveAt(i);
            }
        }
    }

    struct CamInfo {
        public Transform first;
        public Transform second;
        public float distance;
    }

   
}
