using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    public int numberOfCubes = GameData.numberOfCubes;
    public int cubeHeightMax = GameData.cubeHeightMax;
    public GameObject cube;
    //public GameObject Generator;
    //public GameObject Generator2;
    public GameObject sortButton;
    public List<BarGraph> barGraphs= new List<BarGraph>();
    private void Start()
    {
        //barGraphs.Add(new BarGraph(Generator, numberOfCubes));
        //barGraphs.Add(new BarGraph(Generator2, numberOfCubes, SortType.Bubble));
        Time.timeScale = 3;
        GenerateBlocks();
    }
        
    public void GenerateBlocks()
    {
        StopAllCoroutines();
        resetBarGraphsCubesArray(barGraphs);
        foreach(var graph in barGraphs)
        {
            for (int i = 0; i < numberOfCubes; i++)
            {
                int randomHeight = Random.Range(1, cubeHeightMax + 1);
                GameObject instance = Instantiate(cube, graph.position.transform.position, Quaternion.identity);
                instance.transform.position = new Vector3(graph.position.transform.position.x + i * instance.transform.localScale.x, graph.position.transform.position.y + (randomHeight / 2.0f), graph.position.transform.position.z);
                instance.transform.localScale = new Vector3(instance.transform.localScale.x *0.8f, randomHeight, instance.transform.localScale.z);
                instance.transform.parent = graph.position.transform;

                graph.cubesArray[i] = instance;
            }
        }
    }
    public void StartSort()
    {
        foreach(var bar in barGraphs)
        {
            if (bar.sortType == SortType.Selection)
            {
                StartCoroutine(SelectionSort(bar.cubesArray));
            }
            else if (bar.sortType == SortType.Bubble)
            {
                StartCoroutine(BubbleSort(bar.cubesArray));
            }
            else if (bar.sortType == SortType.Merge)
            {
                //Start MergeSort Coroutine
            }
            else
            {
                //Start quickSort Coroutine
            }
        }
    }
    IEnumerator BubbleSort(GameObject[] unsortedList)
    {
        Vector3 tempPos;
        GameObject temp;
        for (int x = 0; x < numberOfCubes; x++)
        {

            for (int y = 0; y < numberOfCubes - x - 1; y++)
            {
                if (unsortedList[y].transform.localScale.y > unsortedList[y + 1].transform.localScale.y)
                {
                    yield return new WaitForSeconds(1f);
                    temp = unsortedList[y];
                    unsortedList[y] = unsortedList[y + 1];
                    unsortedList[y + 1] = temp;

                    tempPos = unsortedList[y].transform.localPosition;
                    //unsortedList[y].transform.localPosition = new Vector3(unsortedList[y+1].transform.localPosition.x, tempPos.y, tempPos.z);
                    //unsortedList[y+1].transform.localPosition = new Vector3(tempPos.x, unsortedList[y+1].transform.localPosition.y, unsortedList[y+1].transform.localPosition.z);
                    LeanTween.color(unsortedList[y], Color.red, .4f);
                    LeanTween.color(unsortedList[y+1], Color.red, .4f);
                    LeanTween.color(unsortedList[y+1], unsortedList[y].transform.GetComponent<MeshRenderer>().material.color, .1f).setDelay(.9f);
                    LeanTween.color(unsortedList[y], unsortedList[y].transform.GetComponent<MeshRenderer>().material.color, .1f).setDelay(.9f);

                    LeanTween.moveLocalX(unsortedList[y], unsortedList[y + 1].transform.localPosition.x, 1f);
                    LeanTween.moveLocalZ(unsortedList[y], -3, .5f).setLoopPingPong(1);

                    LeanTween.moveLocalX(unsortedList[y + 1], unsortedList[y].transform.localPosition.x, 1f);
                    LeanTween.moveLocalZ(unsortedList[y + 1], 3, .5f).setLoopPingPong(1);
                }
            }
            LeanTween.color(unsortedList[numberOfCubes-x-1], Color.green, .4f).setDelay(1f);

        }
    }
    IEnumerator SelectionSort(GameObject[] unsortedList)
    {
        int min;
        GameObject temp;
        Vector3 tempPos;
        for(int i=0; i < unsortedList.Length; i++)
        {
            yield return new WaitForSeconds(1f);
            min = i;
            for(int j = i; j < unsortedList.Length; j++)
            {
                unsortedList[j].transform.GetComponent<MeshRenderer>().material.color = Color.blue;
                yield return new WaitForSeconds(.2f);
                unsortedList[j].transform.GetComponent<MeshRenderer>().material.color = new Color32(103, 207, 228,1);
                if (unsortedList[j].transform.localScale.y < unsortedList[min].transform.localScale.y)
                {
                    min = j;
                }
            }
            if (min != i) {
                yield return new WaitForSeconds(1f);
                temp = unsortedList[i];
                unsortedList[i] = unsortedList[min];
                unsortedList[min] = temp;

                tempPos = unsortedList[i].transform.localPosition;
                //unsortedList[i].transform.localPosition = new Vector3( unsortedList[min].transform.localPosition.x,tempPos.y, tempPos.z);
                //unsortedList[min].transform.localPosition = new Vector3(tempPos.x, unsortedList[min].transform.localPosition.y, unsortedList[min].transform.localPosition.z);
                LeanTween.color(unsortedList[i], Color.red, .4f);
                LeanTween.color(unsortedList[min], Color.red, .4f);
                LeanTween.color(unsortedList[min], unsortedList[i].transform.GetComponent<MeshRenderer>().material.color, .1f).setDelay(.9f);

                LeanTween.moveLocalX(unsortedList[i], unsortedList[min].transform.localPosition.x, 1f);
                LeanTween.moveLocalZ(unsortedList[i], -3, .5f).setLoopPingPong(1);

                LeanTween.moveLocalX(unsortedList[min], unsortedList[i].transform.localPosition.x, 1f);
                LeanTween.moveLocalZ(unsortedList[min], 3, .5f).setLoopPingPong(1);
            }
            LeanTween.color(unsortedList[i], Color.green, .1f).setDelay(.75f);
        }
    }
    public void resetBarGraphsCubesArray(List<BarGraph> list)
    {
        foreach(var graph in list)
        {
            resetArray(graph.cubesArray);
        }
    }
    private void resetArray(GameObject[] cubes)
    {
        if (cubes != null)
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                Destroy(cubes[i]);
            }
            cubes = null;
        }
    }
    public void Initsialize()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            int randomHeight = Random.Range(1, cubeHeightMax + 1);
            GameObject instance = Instantiate(cube, barGraphs[barGraphs.Count-1].position.transform.position, Quaternion.identity);
            instance.transform.position = new Vector3(barGraphs[barGraphs.Count - 1].position.transform.position.x + i * instance.transform.localScale.x, barGraphs[barGraphs.Count - 1].position.transform.position.y + (randomHeight / 2.0f), barGraphs[barGraphs.Count - 1].position.transform.position.z);
            instance.transform.localScale = new Vector3(instance.transform.localScale.x * 0.8f, randomHeight, instance.transform.localScale.z);
            instance.transform.parent = barGraphs[barGraphs.Count - 1].position.transform;

            barGraphs[barGraphs.Count - 1].cubesArray[i] = instance;
        }
    }
}
public enum SortType
{
    Selection,
    Bubble,
    Merge,
    Quick,
}
public class BarGraph
{
    int cubeHeightMax = GameData.cubeHeightMax;
    public GameObject position;
    public SortType sortType = SortType.Selection;
    public GameObject[] cubesArray;
    int numberOfCubes =GameData.numberOfCubes;
    public BarGraph(GameObject position, int numberOfCubes, SortType sortType = SortType.Selection)
    {
        this.position = position;
        this.sortType = sortType;
        this.cubesArray = new GameObject[numberOfCubes];
    }
}