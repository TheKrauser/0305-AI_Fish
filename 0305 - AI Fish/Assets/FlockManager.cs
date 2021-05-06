using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    //Prefab do Peixe
    public GameObject fishPrefab;
    //Número de Peixes
    public int numFish= 20;
    //Lista de GameObject de todos os peixes
    public GameObject[] allFish; 
    //Área na qual eles vão spawnar
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    //Destino
    public Vector3 goalPos;

    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]
    //Velocidade mínima
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    //Velocidade máxima
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    //Distância entre os peixes do lado
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    //Velocidade de rotação
    public float rotationSpeed;

    void Start()
    {
    	//Atribui o número de peixes na Lista
        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; i++)
        {
	    //Randomizando a posição para spawnar
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), 
                                                                Random.Range(-swinLimits.y, swinLimits.y), 
                                                                Random.Range(-swinLimits.z, swinLimits.z));
	    //Instanciando na Lista e na posição randomizada
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
	    //Atribui o script Flock
            allFish[i].GetComponent<Flock>().myManager = this;
        }
	//Destino
        goalPos = this.transform.position;
    }

    void Update()
    {
	//Seta o destino referente a posição dele e os limites da váriavel swinLimits
        goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                                Random.Range(-swinLimits.z, swinLimits.z));
    }
}
