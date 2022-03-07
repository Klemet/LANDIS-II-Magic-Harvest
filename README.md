<p align="center">
  <img src="https://raw.githubusercontent.com/Klemet/LANDIS-II-Magic-Harvest/main/screenshots/logoMagicHarvest.svg" />
</p>
<h1 align="center">Magic Harvest</h1>


# 📑 Description

**Magic Harvest** is an extension for the [LANDIS-II](http://www.landis-ii.org/) model.

It allows users to dynamically change the parameters of the harvest extensions of LANDIS-II ([Base Harvest](http://www.landis-ii.org/extensions/base-harvest) and [Biomass Harvest](http://www.landis-ii.org/extensions/biomass-harvest)) during a simulation, by running a Python script to edit the parameters files as wanted, and re-loading them.

# ✨ How does it work ?

<p align="center">
  <img src="https://raw.githubusercontent.com/Klemet/LANDIS-II-Magic-Harvest/main/screenshots/magicHarvestExplanation.svg" />
</p>


⚠ **WARNING** : As Magic Harvest re-loads the parameters of your harvest extension, certain things have to be taken into account.

In particular, **you should be careful of the ID number associated to the prescriptions, that is used to create the prescription maps**.

When Magic Harvest re-loads the parameters of the harvest extension, the prescriptions that you define in the parameter file of the harvest extension are re-numbered starting from one.

What that means is that **the ID "1" in your prescription maps might refer to a different prescription after Magic Harvest is done**.

If you do not modify your the prescriptions that you define in the parameter file of the harvest extension, you will not have this problem. But if you do change them, **remember it** !

# 🧱 Requirements

To use Magic Harvest, you need:

- The [LANDIS-II model v7.0](http://www.landis-ii.org/install) installed on your computer.
- One of the succession extensions of LANDIS-II installed on your computer.
- One of the harvest extensions ([Base Harvest](http://www.landis-ii.org/extensions/base-harvest) or [Biomass Harvest](http://www.landis-ii.org/extensions/biomass-harvest)) installed on your computer.
- [Python](https://www.python.org/downloads/) installed on your computer
- The Magic Harvest extension installed on your computer (see Download section below).
- The parameter files for your scenario (see Parameterization section below).


# 💾 Download

Version 1.0 can be downloaded [here](https://github.com/Klemet/LANDIS-II-Magic-Harvest/releases/download/v1.0/LANDIS-II-V7.Magic.harvest.1.0-setup.exe). To install it on your computer, just launch the installer.


# 🛠 Parameterization and use

The extension only requires two parameters :

- One indicating where is the parameter file for the harvest extension that you use
  - The path should be relative to the scenario file of LANDIS-II, **not where the parameter file for Magic Harvest is**
    - It should therefore be the same path to the harvest extension parameter file that you indicated in the scenario file
- One indicating where is the python script that you want to launch
  - Again, the path should be relative to where the scenario file of LANDIS-II is; **not where the parameter file for Magic Harvest is**.


# 🎮 Example and testing

If you want to experiment with the extension or test it, you can [download the example files](https://downgit.github.io/#/home?url=https://github.com/Klemet/LANDIS-II-Magic-Harvest/tree/main/Examples).

To launch the example scenario, you'll need the [Age-Only succession](http://www.landis-ii.org/extensions/age-only-succession) extension and the [Base Harvest](http://www.landis-ii.org/extensions/base-harvest) extension installed on your computer, in addition to Magic Harvest. Just launch the `test_scenario.bat` file, and the example scenario should run.

The example scenarios lasts 150 years of simulation, and updates the harvest parameter with magic harvest every 70 years; the update is simply a switch between two rasters of management areas, and two parameter files for the base harvest extension.


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