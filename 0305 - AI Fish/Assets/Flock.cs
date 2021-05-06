using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{   
    //Variável do FlockManager
    public FlockManager myManager;
    //Velocidade do Peixe
    float speed;
    //Variável para saber se o peixe está virando
    bool turning = false;

    void Start()
    {
        //Seta a speed para um número entre a velocidade miníma e máxima que está no FlockManager
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    void Update()
    {
        //Cria o limite para qual os peixes podem nadar sem começar a rotacionar
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        //Se eles passarem o limite
        if (!b.Contains(transform.position))
        {
	    //Começa a virar
            turning = true;
        }
        else
            turning = false;

        //Se estiver virando
        if (turning)
        {
            //Define a direção
            Vector3 direction = myManager.transform.position - transform.position;
            //Rotaciona suavemente
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(direction), 
                myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //Caso o número randomizado entre 0 e 100 seja menor do que 10
            if (Random.Range(0,100)<10)
            {
                //Redefine a velocidade
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }
            //Se for menor do que 20
            if (Random.Range(0, 100) < 20)
            {
                //Chama a função
                ApplyRules();
            }
        }
        //Move o peixe
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        //Lista de GameObjects
        GameObject[] gos;
        //Seta os componentes como os mesmos da Lista allFish
        gos = myManager.allFish;
        //Variável para o centro dos peixes (inicialmente setada em 0)
        Vector3 vcentre = Vector3.zero;
        //Variável para os peixes não se colidirem (inicialmente setada em 0)
        Vector3 vavoid = Vector3.zero;
        //Velocidade geral dos peixes
        float gSpeed = 0.01f; 
        //Distância geral dos peixes
        float nDistance; 
        //Número de peixes no grupo
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            //Se o componente go for diferente do que está esse GameObject
            if(go != this.gameObject)
            {
                //Calcula a distância entre este Objeto e o que está na lista (go)
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //Se a distância entre eles for menor do que a neightbourDistance
                if (nDistance <= myManager.neighbourDistance)
                {
                    //Seta o centro como a posição do Objeto na lista (go)
                    vcentre += go.transform.position;
		    //Incrementa a variável de tamanho do grupo
                    groupSize++;
                    //Se a distância for menor do que 1
                    if (nDistance < 1.0f)
                    {
                        //Calcula a direção para o Objeto que está na lista (go)
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    //Pega o script Flock
                    Flock anotherFlock = go.GetComponent<Flock>();
		    //Aumenta a velocidade do peixe
                    gSpeed = gSpeed + anotherFlock.speed;        
                }

            }
        }
        //Quando o grupo for maior do que 0
        if (groupSize > 0)
        {
            //Calcula o centro dos peixes com base na posição do destino e do peixe atual
            vcentre = vcentre / groupSize + (myManager.goalPos-this.transform.position);
            //Normaliza a velocidade do grupo
            speed = gSpeed / groupSize;
            //Calcula a direção
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if( direction != Vector3.zero)
            {
                //Rotaciona suavemente
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}

