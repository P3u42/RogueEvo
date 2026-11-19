using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CreatureBody : MonoBehaviour
{
       
    [SerializeField]private int bodyLenght;
    [SerializeField]private LineRenderer lineBody;
    [SerializeField]private Transform targetDir;
    [SerializeField] Vector3[]segmentPoses;
    [SerializeField]private float targetDist;
    [SerializeField]private Vector3[]segmentV;
    [SerializeField] private float smoothSpeed;
    [SerializeField]private float wiggleSpeed;
    [SerializeField]private float wiggleMagnitude;
    [SerializeField]Transform wiggleDir;
    [SerializeField]Material[] bodyTextures;
    [SerializeField]Sprite[]headSprites;
    [SerializeField]private Transform limb;
    [SerializeField]private Transform[] bodyParts;

    // Start is called before the first frame update
    void Start()
    {
        lineBody =GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(segmentPoses==null) return;
        MoveSegmemts();
    }

    private void SetSegments()
    {
        lineBody.positionCount = bodyLenght;
        segmentPoses = new Vector3[bodyLenght];
        segmentV= new Vector3[bodyLenght];
    }

    private void MoveSegmemts()
    {
        wiggleDir.localRotation= Quaternion.Euler(0,0,Mathf.Sin(Time.time*wiggleSpeed)*wiggleMagnitude);
        segmentPoses[0]= targetDir.position;
        for(int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i]=Vector3.SmoothDamp(segmentPoses[i],segmentPoses[i-1]+targetDir.right*targetDist,ref segmentV[i],smoothSpeed);
            //bodyParts[i-1].transform.position=segmentPoses[i];
        }
        lineBody.SetPositions(segmentPoses);
        //limb.position=segmentPoses[segmentPoses.Length/2];


        
    }
    

    public void InitFromDNA(EnemyDNA dna)
    {
        bodyLenght = dna.bodyLengthGene;
        wiggleSpeed = dna.wiggleSpeedGene;
        smoothSpeed =dna.smoothSpeedGene;
        //targetDist = dna.targetDistGene
        SetSegments();
    }

}
