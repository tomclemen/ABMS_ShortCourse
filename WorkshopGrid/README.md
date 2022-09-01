# MARS grid-based Model Starter

This model includes some basic building blocks for designing grid-based models in MARS. The environment of a grid-based model consists of a two-dimensional grid. Agents can move on the grid and interact with information in the grid cells as well as with each other.

## Project structure

Below is a brief description of each component of the project structure:

- `Program.cs`: the entry point of the model from which the model can be started. See [Model setup and execution](#model-setup-and-execution) below for more details.
- `config.json`: a JavaScript Object Notation (JSON) with which the model can be configured. See [Model configuration](#model-configuration) below for more details.
- `Model`: a directory that holds the agent types, layer types, entity types, and other classes of the model. See [Model description](#model-description) for more details.
- `Resources`: a directory that holds the external resources of the model that are integrated into the model at runtime. This includes initialization and parameterization files for agent types and layer types.

## Model description

The model consists of the following agent types and layer types:

- Agent types:
  - `StraightRunner`: an agent that moves randomly along **straight** lines.
  - `DiagonalRunner`: an agent that moves randomly along **diagonal** lines.
  - `HelperAgent`: an agent that is implemented purely for technical reasons. Data from the `GridLayer` is sent to the web socket of the visualization tool only when that data is changed. The `HelperAgent` performs a change to the `GridLayer` data to enable the visualization of the grid. Agents of this type are not displayed in the visualization component.
- Layer types:
  - `GridLayer`: the layer on which the agents live and move.
- Other classes:
  - `MovementDirection`: an enumeration of eight movement directions. Each movement changes an agent's position by one unit (grid cell) in the horizontal direction and/or vertical direction.

## Model configuration

The model can be configured via a JavaScript Object Notation (JSON) file called `config.json`. Below are some of the main configurable parameters:

- `startTime` and `endTime`: the start time and end time, respectively, of the simulation
- `deltaT`: the length of a single time step. The simulation time is given by the number of `deltaT` time steps that fit into the range defined by `startTime` and `endTime`
- `pythonVisualization`: a boolean flag that, if set to `true`, prompts the simulation framework to send tick-based data to an external web socket, where it is further processed by a visualization tool. See below for how to set up the visualization tool and get a visualized simulation output.
- `layers`: the layer types that should be included in the simulation
- `agents`: the agent types that should be included in the simulation

For more detailed information on configuration parameters, please click [here](https://mars.haw-hamburg.de/articles/core/model-configuration/index.html).

## Model setup and execution

The following tools are required on your machine to run a full simulation and visualization of this model:

- A C# Interactive Development Environment (IDE), preferably [JetBrains Rider](https://www.jetbrains.com/rider/)
- [.NET Core](https://dotnet.microsoft.com/en-us/download) 6.0 or higher
- [Python](https://www.python.org/downloads/) 3.8 or higher

To set up and run the simulation and visualization, please follow these steps:

1. Download or clone this repository (`mars-learning`)
2. Open this directory: `Examples/2022 Short Course South Africa/WorkshopGrid/Visualization`
3. Follow these instructions to start the visualization tool (alternatively, see the README in the `Visualization` directory):
    1. Open a terminal in the `Visualization` directory
    2. Execute the following command:
        1. macOS: `pip3 install -r requirements.txt`
        2. Windows: `pip install -r requirements.txt`
    3. Once the installation has finished, execute the following command:
        1. macOS: `python3 main.py`
        2. Windows: `python main.py`
    4. A black PyGame window should open. **Note:** Do not close the terminal.
    5. Alternatively to the above, the visualization tool can be started with a Python IDE such as [JetBrains PyCharm](https://www.jetbrains.com/pycharm/).
4. Open JetBrains Rider.
5. Open the solution file `Examples/2022 Short Course South Africa/WorkshopGrid/WorkshopGrid.sln`.
6. Run the `Main()` method in the file `Program.cs`.
7. The simulation should run in Rider and, simultaneously, a visualization should be displayed in the PyGame window.

## Interacting with the visualization

While the visualization is running, its speed (framerate) can be changed by pressing the up arrow (increase speed) or down arrow (decrease speed) on your keyboard.
