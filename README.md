## Neuroevolution (NEAT) for Hill Climb Racing

### Introduction

This project explores the use of neuroevolution (NEAT) to optimize vehicle controls in the game "Hill Climb Racing". The objective is to create an AI-controlled vehicle that can navigate through challenging tracks by learning from its experiences.

### Setup

- Install Python 3.x and its dependencies (neat, socket, json)
- Install Unity
- Clone this repository to your local machine

### Usage

To run the NEAT algorithm:

1. Start the server: Execute file 'neat_HCR.py' inside 'HillClimbRacing\Assets\NEAT' folder.
2. Set parameters: Press play button in unity. Pass parameters (individuals number, iterations number, iteration duration) in the settings menu. Press start button in the settings menu
3. Evaluate: Once training is complete, there will be a graph representing the learning curve of the population

### Expected Results

After training, the NEAT algorithm is expected to produce a vehicle that:

- Can navigate through the tracks without crashing
- Learns to adjust its speed to optimize performance
- Outperforms a human-controlled vehicle

### Dependencies

- Python 3.x
- neat
- socket
- json

### Future Work

- Explore different fitness functions for evaluating vehicle performance
- Optimize NEAT parameters for better results
