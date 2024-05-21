# World Collision Tester User Manual

After you have installed the necessary packages you will need to add the package to your project in Unity.

1. At the top of the editor window, find and click on the "Window" tab.
2. In the drop-down menu, scroll down and look for the "Package Manager" option.
3. Make sure you're in the "Unity Registry" tab.
4. Search for "ML-Agents" in the search box.
5. Find the package "com.unity.ml-agents" and click on it.
6. Click the "Install" button to start the installation process.

Once installed, ML-Agents will be ready to use in your project. You can now download or duplicate the Scripts folder from this repository 
to get the World Collision Tester funcionality.

## Get ready your agent

In order for your character (agent) to learn the right behaviour and not have problems with other scripts, other scripts that have to do 
with movement and any other scripts that interfere with the search for errors should be disabled. Only scripts that control e.g. the 
gravity of the character or which areas the character can access should be active. 

Now, you should have a set of scripts for training with the ML-Agents library, see how to set up the agent in the section on [Making 
a New Learning Environment](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Learning-Environment-Create-New.md). 
It is very important that the script you create for the agent mimics the movement of your character. In addition, in order to make 
use of the OverlapDetectorWithReward script, it will be necessary to add the following functions and declaration in the script 
created for the agent:

```csharp
private OverlapDetectorWithReward overlapDetector;

    private void Start()
    {
        //Rest of the code
        overlapDetector = GetComponent<OverlapDetectorWithReward>();
        if (overlapDetector == null)
        {
            Debug.LogError("OverlapDetectorWithReward component not found.");
        }

        overlapDetector.OnCollisionDetected += HandleCollisionDetected;
    }

    private void HandleCollisionDetected(float rewardValue)
    {
        AddRewardFromDetector(rewardValue);
    }

    public void AddRewardFromDetector(float rewardValue)
    {
        SetReward(rewardValue);
        EndEpisode();
    }
```

It is recommended that the OnActionReceived function should also check whether Max Step has been reached and penalise it. 
Although, this may vary depending on the number of agents used and the number of maximum steps that are set.

```csharp
public override void OnActionReceived(ActionBuffers actions) {
  // Rest of the code
  if (StepCount >= MaxStep)
  {
      SetReward(-0.25f);
  }
}
```

Additionally, the script OverlapDetectorWithReward must be added. This script has three variables:

- ```Layers to detect:``` This variable collects the list of the layer names of the objects to be checked for collisions. Layers related to floors, walls, etc... should be listed here.
- ```Overlap type:``` It is a combination of capsule, cube and sphere. The user will choose the shape that best suits the agent. Using the gizmos, it will be possible to see how the shape fits the agent.
- ```Detection radius:``` A float of the size of the previously selected shape.

With this script we will detect when our agent is passing through an object that it should not. That's why it's important to indicate all the layers related to the scenario, choose the shape that best suits the agent and a size that is smaller than the agent.

<div style="display: flex;">
    <img src="https://github.com/TheLeshuga/WorldCollisionTester/assets/72620125/3868a76a-c469-4f59-85e8-fde379ee4f05" alt="capsule" width="320" style="margin-right: 10px;">
    <img src="https://github.com/TheLeshuga/WorldCollisionTester/assets/72620125/e807e438-5af2-4098-9559-b6922dcbb9ac" alt="sphere" width="230" style="margin-right: 10px;">
    <img src="https://github.com/TheLeshuga/WorldCollisionTester/assets/72620125/c3b783b5-a83a-46db-910c-c50c48830238" alt="box" width="260">
</div>

In these images, the 3 different forms can be seen in the same agent. In this example the best option will be the capsule shape (left image) and the radius is 0.3, as it is smaller than the agent and is a good range to say that the agent passes through an object it should not.


## Setting up the grid map

In an empty object in the scene, the GridMap script, GridMapRegulated or both shall be included. These scripts that extends from BaseGridMap are two types of grid maps where the first one will directly fit the given scenario object and the second one gives the freedom to adjust it to more irregular scenarios.

These scripts will be used to create a list of positions where the agent can appear, creating a randomisation of zones to be explored during training or testing. This list will be used in the script created for the agent.

Both scripts share the following variables:

- ```Lower left corner positions:``` This will be the list of objects that represent the lower left corners of the grid. A cube or other object should be placed at the indicated position on the stage where you want the grids.
- ```Cell size list:``` This is the list of the size of the grid cells.
- ```Grid cube Y size:``` This is the list of the height of the cubes used to detect obstacles or the ground. This option is added to adapt the grid to floors with different heights. If you don't want the height to vary, set the same size as you set in cell size.
- ```Obstacle layer:``` It is the obstacle layer, it will detect that all objects that have this layer are not possible positions to spawn the agent.
- ```Ground layer:``` It is the ground layer, it will detect that those objects that have this layer are possible positions to spawn the agent.
- ```Debug Mode:``` It will display the grid, colouring the cells red if it detected that it has the obstacle layer and green if it has the ground layer.

<div style="display: flex;">
    <img src="https://github.com/TheLeshuga/WorldCollisionTester/assets/72620125/8050546f-4839-48ae-b745-95deb1d424da" alt="leftcorner" width="450" style="margin-right: 10px;">
    <img src="https://github.com/TheLeshuga/WorldCollisionTester/assets/72620125/31f067d0-ddb5-4ce4-940b-21d886e45d8e" alt="debugfloor" width="450">
</div>

In the image scenario the GridMap is being used. In the imagen of the left, you see the cube used for the bottom left corner (note how the Z axis faces upwards and the X axis faces to the right). In the image of the right, you see the same scenario with debug mode.

The GridMap script will also have the variable Floor objects, a list of objects representing the floor of the scene. It will create the grid from these floors. The GridMapRegulated script instead, instead of accepting objects, will have two lists: Floor widths and Floor heights. Where the size of the grids must be entered. When entering the configuration of a grid, put them all in the same order and do not forget that all list variables must have the same number of objects.

![regulateddebug](https://github.com/TheLeshuga/WorldCollisionTester/assets/72620125/0c46d67d-bd62-4f2d-b872-03f104aaf466)

In the scene of the image, use is made of the GridMapRegulated, composed of 6 grids to avoid elevated areas. As we have seen, for both types, several grids can be added and both types can be combined in the scene to best adjust the possible spawning positions of the agent. Also, it is necessary to include the "heatBox" tag in the project for the correct functioning of the grid maps.

Now to get the list of positions for the agent include this statement in your script made for the agent:

```csharp
private GridMapRegulated gridMap;    // Or GridMap
private List<Vector3> possiblePositions = new List<Vector3>();

private void Start()
    {
        gridMap = FindObjectOfType<GridMapRegulated>();
        if (gridMap != null)
        {
            possiblePositions = gridMap.ReceivePositions();
        }
        else
        {
            Debug.LogError("GridMap component not found.");
        }

        // Rest of the code 
    }
```

A recommended way to use the list of possible positions in OnEpisodeBegin would be:

```csharp
public override void OnEpisodeBegin() {
    if (possiblePositions.Count == 0) {
        Debug.LogError("possiblePositions list is empty.");
    } else {        
        int randomIndex = Random.Range(0, possiblePositions.Count);
    
        transform.localPosition = possiblePositions[randomIndex];
        // Rest of the code
    }
}
```

In this way we avoid that the execution is broken by having an empty list, moreover, in the first instance it will be normal to have an empty list. Therefore, it will be normal to receive the empty list error once per agent. 

You can use both types of grid in the scene by combining the lists of possible positions provided by each script in this way:

```csharp
private GridMap gridMap;
private GridMapRegulated gridMapRegu;
private List<Vector3> possiblePositions = new List<Vector3>();

private void Start()
{
    gridMap = FindObjectOfType<GridMap>();
    if (gridMap != null)
    {
        possiblePositions.AddRange(gridMap.ReceivePositions());
    }
    else
    {
        Debug.LogError("GridMap component not found.");
    }

    gridMapRegu = FindObjectOfType<GridMapRegulated>();
    if (gridMapRegu != null)
    {
        possiblePositions.AddRange(gridMapRegu.ReceivePositions());
    }
    else
    {
        Debug.LogError("GridMapRegulated component not found.");
    }
    // Rest of the code
}
```

## Optimise training with more agents

Again you will need the empty object in the scene to add the CloneAgent script to. This script will clone the agent that has been configured the number of times indicated. It has the following four variables:

- ```Agent game object:``` This variable indicates the configured agent that we want to clone.
- ```Num clones:``` It will be the number of clones of the agent indicated above that we want in the scene.
- ```Ignore agent colliders:``` Boolean variable indicating whether we want to ignore if the agents between them cross each other or not. Keep this option enabled as long as you do not also want to do a collision check between the agents.
- ```Agent layer name:``` It is the layer that has the agent, it is important to know that the objects of that layer (the cloned agents) must be ignored in the case of having the previous option activated.

Each of these cloned agents will be "independent" in the training, in other words, in this way we aim to train an agent capable of finding errors in a general way, not just finding some errors.

## Learn about the errors found

In the library is the CSVManager script, add it to the empty object in the scene to get a report of the errors found. This script will generate a .csv file with the information per row of: the position where an object was traversed that should not have been, the number of times it was passed, the name of the object and the agent(s) that encountered the error. Here is an example of a row that would generate:

```
(-16,37997/0,1056978/30,74754);16;wallUnleveled;Agent_27,Agent_13,Agent_18,Agent_9,Agent_38,Agent_46,Agent_2,Agent_50,Agent_16,Agent_23,Agent_36,Agent_41,Agent_22,Agent_14
```
To use this functionality, in addition to adding the component to the scene, it must be activated otherwise it will not generate the results. The only configuration you will need to do is to enter the name of the csv to generate. The first time it will generate within the project a directory called "ResultsCSV" where all the files .csv generated with this script will be saved. If there is already a file with the name that has been entered, it will not be rewritten, it will create the file with the same name but adding the copy number.

You will be able to use this error report generator both in training and in model execution. It is recommended to do both in order to get the largest number of errors in the scenario. 

## Where do your agents usually go?: Heat Map

Another functionality mentioned was the creation of heat maps. A heat map will be created with the same surface of the configured grids where the number of times a cell has been traversed will be updated with colours. First, it will be necessary to add the PositionData script to the agent. With this script, the positions that the agent passes through every tracking frequency seconds will be stored. Tracking frequency will be the frequency in which the position will be stored in seconds.

Then, the HeatMapReader and HeatMapColorizer scripts will be added to the empty object in the scene. The first script will be in charge of processing the positions collected with PositionData to find to which cell each position corresponds and the second script will be in charge of calculating the colour that the cell should take according to the number of times it has been frequented.

HeatMapReader has the following three variables:

- ```Read frecuency:``` This is the frequency in time it will take to take the registered positions of each agent and process them. This frecuency has to be higher than the tracking frecuency in PositionData.
- ```Xshift:``` It is the displacement on the X-axis where we want the heat map to appear taking as origin point where the grids maps are located.
- ```Nearest pos offset:``` It is the minimum distance you must have to take that an agent position is in a cell. It is important that the size is not too small, the sizes used for grid cells should be taken into account. It is recommended that you choose a size equal to or smaller than the grid with the smallest cell size.

HeatMapColorizer has only the Color adjuster variable, this variable will be the value that adds to the colour for each position in the cell. The colour range will be from dark blue if it has not been frequented to deep red if it has been frequented a lot. It is recommended that if you are going to use many agents or you are going to leave a long execution, the number should be very small. The allowed range is 0 to 1.

For the heatmap to be created, all three of these scripts need to be active in the scene. You may encounter problems with the order of execution of the scripts, HeatMapReader should always be executed before HeatMapColorizer, if the created cubes do not appear blue, it is because it is not being executed in that order. The way to fix this is to go to Edit > Project Settings > Script Execution Order. Here you will set the grid or grids scripts in your scene to run first, then HeatMapReader and finally HeatMapColorizer.

<div style="display: flex;">
    <img src="https://github.com/TheLeshuga/WorldCollisionTester/assets/72620125/e3605701-6f19-4b12-b16f-68e03c1eaf1f" alt="heatmap0" width="450" style="margin-right: 10px;">
    <img src="https://github.com/TheLeshuga/WorldCollisionTester/assets/72620125/4f548f18-9130-42cb-adc5-b36f0de667fc" alt="heatmap1" width="450">
</div>

In the image on the right, you can see how the heat map should look in the first instance. You can also see how the heat map is like a "copy" of the grids. In the image on the left, you see cells with different colours and intensities after a few minutes of execution.

## Visualise your heat map data

As well as for the reporting of found errors, there is also a script similar to CSVManager collecting the heat map information with the CSVManagerHM script. Add the CSVManagerHM script to the empty object in the scene to get the report. This script will generate a .csv file with the information per row of: the position of the cell, the number of times the agents have passed through the cell, the colour of the cell in Colour format and the colour in hexadecimal format. Here is an example of a row that would generate:

```
(-4,573728/-0,2042823/3,84873);3;(0,5/0,5/0)/1;#808000
```

Note that the colour format is formatted as RGB in brackets and the alpha next to it. To use this functionality, in addition to adding the component to the scene, it must be activated otherwise it will not generate the results. The only configuration you will need to do is to enter the name of the csv to generate. The first time it will generate within the project a directory called "ResultsCSV-HM" where all the files .csv generated with this script will be saved. If there is already a file with the name that has been entered, it will not be rewritten, it will create the file with the same name but adding the copy number.

Additionally, the HeatMapVisualizer script can be used to visualise the heat map once the results have been generated. Add the script to the empty object in the scene and fill in the only parameter it requires with the name of the CSV file with the data you want to visualise. HeatMapVisualizer has the following three variables:

- ```Xshift:``` It is the displacement on the X-axis where we want the heat map to appear taking as origin point where the grids maps are located.
- ```Cube size:``` This is the size of the cube you will create to represent the cells. 
- ```Csv file name:``` This is the name of the CSV file from which the results will be taken to recreate the heat map.

![heatmapvisu](https://github.com/TheLeshuga/WorldCollisionTester/assets/72620125/bd2a6de1-e2d3-40fc-8af7-828aded0b315)

The image shows the cells travelled by the agent collected in the CSV file to the right of the scenario. As with the previous scripts, these two scripts will need to be active for them to work.
