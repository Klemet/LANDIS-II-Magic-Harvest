<p align="center">
  <img src="https://raw.githubusercontent.com/Klemet/LANDIS-II-Magic-Harvest/main/screenshots/logoMagicHarvest.svg" />
</p>
<h1 align="center">Magic Harvest</h1>


# 📑 Description

**Magic Harvest** is an extension for the [LANDIS-II](http://www.landis-ii.org/) model.

It allows users to dynamically change the parameters of the harvest extensions of LANDIS-II ([Base Harvest](http://www.landis-ii.org/extensions/base-harvest) and [Biomass Harvest](http://www.landis-ii.org/extensions/biomass-harvest)) during a simulation, by running a command to call another program during the simulation. This command can edit the parameters files as wanted (for exemple, by calling a R or Python script). Magic Harvest then forces the harvest extension to re-load its parameters now that the files are modified. 

# ✨ How does it work ?

<p align="center">
  <img src="https://raw.githubusercontent.com/Klemet/LANDIS-II-Magic-Harvest/main/screenshots/magicHarvestExplanation.svg" />
</p>


⚠ **WARNING** : As Magic Harvest re-loads the parameters of your harvest extension, certain things have to be taken into account.

First, **you should be careful of the ID number associated to the prescriptions, that is used to create the prescription maps**.

When Magic Harvest re-loads the parameters of the harvest extension, the prescriptions that you define in the parameter file of the harvest extension are re-numbered starting from one.

What that means is that **the ID "1" in your prescription maps might refer to a different prescription after Magic Harvest is done**.

If you do not modify your the prescriptions that you define in the parameter file of the harvest extension, you will not have this problem. But if you do change them, **remember it** !

Second, **the scheduling of your repeated prescriptions will be lost in the process !**. This is because of the re-initialization of the harvest extension. You can track this scheduling via your python or R script in many different ways to manage it "manually" (via your script rather than with the internal variables of the harvest extensions), by dumping the schedule for each stand into a file (e.g. a .JSON file) that will be read by your script at the next time step.

## Can I use the extension just to run a script, without altering anything that happens in the harvest extension ?

Yes; this functionality has been added to v1.3. You can now set the optional parameter `NoHarvestReInitialization True` **at the end of your parameter file**. This will disable the re-initialization of the harvest extension, keeping every parameter, schedules or variables of the harvest extension intact. In essence, this allows you to use Magic Harvest as a simple tool to run a script during a LANDIS-II simulation.

# 🧱 Requirements

To use Magic Harvest, you need:

- The [LANDIS-II model v7.0](http://www.landis-ii.org/install) installed on your computer.
- One of the succession extensions of LANDIS-II installed on your computer.
- One of the harvest extensions ([Base Harvest](http://www.landis-ii.org/extensions/base-harvest) or [Biomass Harvest](http://www.landis-ii.org/extensions/biomass-harvest)) installed on your computer.
- The program that you want to launch during the simulation (e.g., Python or R) installed on your computer and able to be activated through a terminal (or command prompt in Windows)
- The Magic Harvest extension installed on your computer (see Download section below).
- The parameter files for your scenario (see Parameterization section below).


# 💾 Download

You can download the latest release version for Windows on the [release page](https://github.com/Klemet/LANDIS-II-Magic-Harvest/releases/). To install it on your computer, just launch the installer (`LANDIS-II-V7.Magic.harvest.X.X-setup.exe`).

For Linux or for Compute Canada's servers, you'll have to clone the repo and use the `dotnet build -c Release` command, as for any other LANDIS-II extension you want to use on Linux.

# 🛠 Parameterization and use

The extension only requires four parameters :

- `Timestep` indicates the frequency at which Magic Harvest should be run during the LANDIS-II simulation
- `HarvestExtensionParameterFile` indicates the path of the parameter file of the harvest extension (Base Harvest or Biomass Harvest), relative to the folder where the LANDIS-II scenario is launched from. It should be the same path as indicated for the harvest extension in the main scenario file.
- `ProcessToLaunch` is the name of the program that you want to call. You must be able to activate it in a terminal beforehand for it to work during a LANDIS-II simulation. This program can be Python (e.g., `python`) or `Rscript` to run a R script.
- `ProcessArguments` contains the arguments that you want to give to the process to launch. For exemple, if you want to run a Python or an R script, `ProcessArguments` will have to contain the path to the script. You can add other arguments separated by spaces; but if you use spaces, remember to contain the whole string for the parameter between quotation marks (e.g., `"./RscriptTest.R second_argument third_argument"`).
	- If you don't want to give any process argument, use `""` or `{none}` for this parameter.
	- If you want to give the current time step at which Magic Harvest is being launched as an argument to your program (very useful in the case of Python or R scripts), put the string `{timestep}` inside `ProcessArguments` were you want the time step to be inserted as an argument. For exemple, you can use `"./RscriptTest.R {timestep}"`

# 🎮 Example and testing

If you want to experiment with the extension or test it, you can [download the example files](https://downgit.github.io/#/home?url=https://github.com/Klemet/LANDIS-II-Magic-Harvest/tree/main/Examples).

To launch the example scenarios, you'll need the [Age-Only succession](http://www.landis-ii.org/extensions/age-only-succession) extension and the [Base Harvest](http://www.landis-ii.org/extensions/base-harvest) or [Biomass Harvest](http://www.landis-ii.org/extensions/biomass-harvest) extension installed on your computer, in addition to Magic Harvest. Just launch the `.bat` files in the exemple scenarios folders, and the selected example scenario should run.

The example scenarios lasts 150 years of simulation, and updates the harvest parameter with magic harvest every 70 years; by default, it uses a python script to switch between two rasters of management areas, and two parameter files for the base harvest extension.


# ☎️ Support

If you have a question, please send me an e-mail at clem.hardy@outlook.fr. I'll do my best to answer you in time.

You can also ask for help in the [LANDIS-II users group](http://www.landis-ii.org/users).

If you come across any issue or suspected bug when using Magic Harvest, please post about it in the [issue section of the Github repository of the module](https://github.com/Klemet/LANDIS-II-Magic-Harvest/issues).


# ✒️ Author

[Clément Hardy](http://www.cef-cfr.ca/index.php?n=Membres.ClementHardy)

PhD Student at the Université du Québec à Montréal

Mail : clem.hardy@outlook.fr

Github : [https://github.com/Klemet](https://github.com/Klemet)


# 💚 Acknowledgments

This work would not be possible without the incredible project that is LANDIS-II, and without the care and passion that the LANDIS-II fondation have to make the project as participative and accessible as it is. I thank them all tremendously.
