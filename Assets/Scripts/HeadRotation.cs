using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotation : MonoBehaviour
{

    [SerializeField]private float rotationSpeed = 5f;
    [SerializeField]private Vector2 direction;
    [SerializeField]private float moveSpeed;
    [SerializeField]private float changeDirectionTime =2f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void InitFromDNA(EnemyDNA dna)
    {
        changeDirectionTime =  dna.chageDirectionTimeGene;
    }

    // Update is called once per frame
    void Update()
    {
        timer+= Time.deltaTime;
        if (timer >= changeDirectionTime){
            PickRandomDirection();
            timer =0f;

        }
        RotateHead();
    }

    private void RotateHead(){
            //direction = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
            float angle = Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation,rotation,rotationSpeed*Time.deltaTime);
            Vector2 cursorPos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //transform.position =Vector2.MoveTowards(transform.position,cursorPos,moveSpeed*Time.deltaTime);
    }

    private void PickRandomDirection()
    {
        direction =Random.insideUnitCircle.normalized;
    }
}
