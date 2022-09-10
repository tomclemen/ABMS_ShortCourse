# MARS geo-referenced Model Starter

This project shows how to integrate your own georeferenced simulation region into a MARS model. It is done by using graphs from OpenStreetMap (OSM) and Point of Interest (POI) data from Geofabrik/OSM.

## Getting geodata

The data can be downloaded with the provided Jupyter notebooks. These notebooks provide a starting point for downloading data and further filtering/adjusting them to your needs. Note that the Geographic Information System (GIS) dependencies of the used Python libraries can be quite difficult to install. With the provided Docker container, you have a ready-to-use JupyterLab instance with all installed dependencies. To use the container, please: 

1. [Install Docker Desktop](https://www.docker.com/get-started/) and start it
2. Run the bash script `./notebookdocker.sh` on Linux/macOS or the `./notebookdocker.bat` on Windows in your terminal
3. Open JupyterLab on [http://localhost:8888/](http://localhost:8888/)

You can also use your own Jupyter Notebook instance. But in that case, you need to install the dependencies yourself.

If you have no access to Docker (HAW lab computers), you can also use the [JupyterHub of the HAW ICC](https://jupyterhub.informatik.haw-hamburg.de/hub/login) (a HAW account is needed).

1. Login into the JupyterHub
2. Select one instance (no GPUs are needed)
3. Upload the three `*.ipynb` Notebooks and open and execute them (see steps below)

The documentation of the HAW JupyterHub can be [found here](https://icc.informatik.haw-hamburg.de/docs/services/jupyterhub/).

Alternatively also [Google Colab](https://colab.research.google.com/) can be used. Be aware that you usually have no persistent storage on Google Colab, so the downloaded data need to be saved locally before the Notebook is closed.

### Graph data

For movement of our agents, we need a street network (graph). To get such a graph, we use data from OSM. To get the data, we first need to define our area of interest (AOI) in which our simulation will be situated. For this, we need to define a Well-Known Text (WKT) geometry that describes our chosen AOI. This can be done with, for example, [https://geojson.io/](https://geojson.io/). Draw a rectangle with the button the toolbar on the right side of the map and save your rectangle as a WKT file (`Save > WKT` in the menu above the map).

After you saved the file, open the provided Jupyter Notebook `Download Graph.ipynb` in JupyterHub for further information. Make sure you see the downloaded WKT file alongside the notebook JupyterHub, if not upload it.

### POI data

For POI data, we utilize OSM data as well. The people of Geofabrik have done some preprocessing of raw OSM data, which makes the ingestion into the model easier. See the .shp file download for South Africa here: [south-africa-latest-free.shp.zip](http://download.geofabrik.de/africa/south-africa.html) (all other countries are provided as well). For our AOI of Port Elizabeth, we now need to extract all relevant POIs like restaurants, cafes, shops, etc. 

For this, please download the data and have a look at the `Prepare POIs.ipynb` notebook, as before you might need to upload and rename the WKT file as well.

## Running the model

In this starter template, the data needed for running a simulation on the area of Port Elizabeth have already been downloaded and put into the Resources folder of the model at `GeodataBlueprint/Resources`.

If you did run the both notebooks with a new AOI and the provided Docker container, the files have already been updated (to prevent data loos existing files for the graph and POIs are renamed to `bkp_*` files in the Resources directory if you need them later).  
In case you did run the notebooks on an other JupyterHub download the created `GeodataBlueprint/Resources` folder and copy it's contents into the local folder.

To run the model open the `GeodataBlueprint.sln` with Rider and run it.

After running the model, some log files and movement files are available that we now can analyze.

## Analysis

For analysis of the created data, please have a look at the `Analyze.ipynb` notebook. It shows some basic statistics about the model as well as an interactive map with the movement of your agents.
