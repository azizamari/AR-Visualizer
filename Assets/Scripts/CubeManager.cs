using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public int numberOfCubes = 10;
    public int cubeHeightMax = 10;
    public GameObject[] cubesArray;
    public GameObject cube;
    public Transform Generator;
    private void Start()
    {
        GenerateBlocks();
        StartCoroutine(SelectionSort(cubesArray));
}
    public void GenerateBlocks()
    {
        StopCoroutine(SelectionSort(cubesArray));
        resetArray();
        cubesArray = new GameObject[numberOfCubes];

        for (int i = 0; i < numberOfCubes; i++)
        {
            int randomHeight = Random.Range(1, cubeHeightMax + 1);
            GameObject instance = Instantiate(cube, Generator.position, Quaternion.identity);
            instance.transform.position = new Vector3(Generator.position.x + i * instance.transform.localScale.x, Generator.position.y + (randomHeight / 2.0f), Generator.position.z);
            instance.transform.localScale = new Vector3(instance.transform.localScale.x *0.8f, randomHeight, instance.transform.localScale.z);
            instance.transform.parent = Generator;

            cubesArray[i] = instance;
        }
    }
    IEnumerator SelectionSort(GameObject[] unsortedList)
    {
        int min;
        GameObject temp;
        Vector3 tempPos;
        for(int i=0; i < unsortedList.Length; i++)
        {
            yield return new WaitForSeconds(.5f);
            min = i;
            for(int j = i; j < unsortedList.Length; j++)
            {
                if (unsortedList[j].transform.localScale.y < unsortedList[min].transform.localScale.y)
                {
                    min = j;
                }
            }
            if (min != i) {
                yield return new WaitForSeconds(.5f);
                temp = unsortedList[i];
                unsortedList[i] = unsortedList[min];
                unsortedList[min] = temp;

                tempPos = unsortedList[i].transform.localPosition;
                //unsortedList[i].transform.localPosition = new Vector3( unsortedList[min].transform.localPosition.x,tempPos.y, tempPos.z);
                //unsortedList[min].transform.localPosition = new Vector3(tempPos.x, unsortedList[min].transform.localPosition.y, unsortedList[min].transform.localPosition.z);
                LeanTween.color(unsortedList[i], Color.red, .25f);
                LeanTween.color(unsortedList[min], Color.red, .25f);
                LeanTween.color(unsortedList[min], unsortedList[i].transform.GetComponent<MeshRenderer>().material.color, .1f).setDelay(.9f);

                LeanTween.moveLocalX(unsortedList[i], unsortedList[min].transform.localPosition.x, 1f);
                LeanTween.moveLocalZ(unsortedList[i], -3, .5f).setLoopPingPong(1);

                LeanTween.moveLocalX(unsortedList[min], unsortedList[i].transform.localPosition.x, 1f);
                LeanTween.moveLocalZ(unsortedList[min], 3, .5f).setLoopPingPong(1);

            }
            LeanTween.color(unsortedList[i], Color.green, .1f).setDelay(.75f);
        }
    }
    private void resetArray()
    {
        if (cubesArray != null)
        {
            for (int i = 0; i < cubesArray.Length; i++)
            {
                Destroy(cubesArray[i]);
            }
            cubesArray = null;
        }
    }
}
