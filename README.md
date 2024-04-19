# World Collision Tester
  <p align="left">
   <img src="https://img.shields.io/badge/STATUS-EN%20DESAROLLO-green">
   </p>
   
## About
World Collision Tester is a library for the Unity game engine to facilitate the search for world collision errors by using agents trained with RL-PPO to find those errors.

![scene](https://github.com/TheLeshuga/WorldCollisionTester/assets/72620125/7c7422b1-8882-47bf-b4d5-e3372a55ccf4)

This library, combined with [ML-Agents](https://github.com/Unity-Technologies/ml-agents) to train the agents, offers the ability to clone agents to improve training, detect collisions with objects that should not be traversed, adapting to the different possible shapes of the character, create a heat map of the areas through which the agent has passed, and generate reports on the errors found and the heat map.

## Functionality 
- ```Detect collision errors:``` Check collisions of the agent simulating the game character with the environment to report bugs.
- ```Heat Map:``` It generates a heat map that reflects the shape of the specified scenario, and adjusts the colour of the areas according to the frequency of the agent's steps in each zone.
- ```Collision error report:``` Generates a CSV file containing information about the errors, such as the position, the number of occurrences of the error, the name of the object involved and the names of the agents that encountered the error.
- ```Heat Map report:``` Generates a CSV file containing information about the heat map such as the position of the grid cell, the number of times the grid cell was walked over and the colour of the cell.

## Used Technologies

- Unity (2023.1.13f1)
- C#
- ML-Agents

## Installation Step
It is recommended to create a virtual environment to avoid conflicts between packages and/or versions. If you have any problem with the ML-Agents version or the compatibility between packages, you can check the installation guide from [ML-Agents installation markdown](https://github.com/Unity-Technologies/ml-agents/blob/release_18_docs/docs/Installation.md).

### Windows
1. **Install Python:** If you haven't already, download and install Python from [python.org](https://www.python.org/downloads/) (version 3.9 or higher).

2. **Install required packages:** Open a command prompt and run the following commands to install the required packages:

```bash
pip install torch==1.7.1
pip install protobuf==3.20.0
pip install mlagents==0.30.0
```

### Linux

1. **Install Python:** If you haven't already, you can install Python using your package manager. For Debian-based systems like Ubuntu, you can use:

```bash
sudo apt-get update
sudo apt-get install python3
```

2. **Install pip:** If pip is not already installed, you can install it using:

```bash
sudo apt-get install python3-pip
```

3. **Install required packages:** Open a terminal and run the following commands to install the required packages:

```bash
pip3 install torch==1.7.1
pip3 install protobuf==3.20.0
pip3 install mlagents==0.30.0
```

### Download Unity:

You can download the specific version of Unity used in the project from [Unity Releases](https://unity.com/releases/editor/whats-new/2023.1.13).


## User Manual

For detailed instructions on how to use the library, please refer to the [User Manual](https://github.com/TheLeshuga/WorldCollisionTester/blob/main/UserManual.md) located in the repository.

## License
This project is licensed under the GPL-3.0 license. You can find the full text of the license in the [LICENSE](LICENSE) file.
## Author
This project was created by Sira Garcerán García.
