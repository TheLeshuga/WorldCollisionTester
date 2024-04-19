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



It is necessary to include the "heatBox" tag in the project for the correct functioning of the grid maps.
