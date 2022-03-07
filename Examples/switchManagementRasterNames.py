# -*- coding: utf-8 -*-
"""
@author: Clément Hardy


"""

#%% 1. LOADING PACKAGES AND FUNCTIONS

import os
# os.chdir(r"D:\OneDrive -UQAM\OneDrive - UQAM\1 - Projets\Thèse - Chapitre 3\2_Projet_extension_REHARVEST\Examples")

print("Python script : Switching management area files !")

os.rename((os.getcwd() + "/input/disturbances/management.tif"), (os.getcwd() + "/input/disturbances/managementOLD.tif"))
os.rename((os.getcwd() + "/input/disturbances/managementAlt.tif"), (os.getcwd() + "/input/disturbances/management.tif"))
os.rename((os.getcwd() + "/input/disturbances/managementOLD.tif"), (os.getcwd() + "/input/disturbances/managementAlt.tif"))

print("Python script : Switching harvest parameter files !")

# If harvest_70 does exist, we do the switch for _0 to _70

print("Does path exists ? " + str(os.path.isfile(os.getcwd() + "/input/disturbances/harvest_70.txt")))

if os.path.isfile(os.getcwd() + "/input/disturbances/harvest_70.txt"):
    os.rename((os.getcwd() + "/input/disturbances/harvest.txt"), (os.getcwd() + "/input/disturbances/harvest_0.txt"))
    os.rename((os.getcwd() + "/input/disturbances/harvest_70.txt"), (os.getcwd() + "/input/disturbances/harvest.txt"))
else: #If not, we do the opposite.
    os.rename((os.getcwd() + "/input/disturbances/harvest.txt"), (os.getcwd() + "/input/disturbances/harvest_70.txt"))
    os.rename((os.getcwd() + "/input/disturbances/harvest_0.txt"), (os.getcwd() + "/input/disturbances/harvest.txt"))


print("Python script : Switching finished ! Ending...")
