# -*- coding: utf-8 -*-
"""
@author: Clément Hardy


"""

#%% 1. LOADING PACKAGES AND FUNCTIONS

import os
# os.chdir(r"D:\OneDrive -UQAM\OneDrive - UQAM\1 - Projets\Thèse - Chapitre 3\2_Projet_extension_REHARVEST\Examples")

print("Python script : Switching management area files !")

os.rename((os.getcwd() + "/biomass-harvest_Management_s2e1.tif"), (os.getcwd() + "/biomass-harvest_Management_s2e1_OLD.tif"))
os.rename((os.getcwd() + "/biomass-harvest_Management_s2e1_ALT.tif"), (os.getcwd() + "/biomass-harvest_Management_s2e1.tif"))
os.rename((os.getcwd() + "/biomass-harvest_Management_s2e1_OLD.tif"), (os.getcwd() + "/biomass-harvest_Management_s2e1_ALT.tif"))

print("Python script : Switching harvest parameter files !")

os.rename((os.getcwd() + "/biomass-harvest_SetUp_s2e1.txt"), (os.getcwd() + "/biomass-harvest_SetUp_s2e1_OLD.txt"))
os.rename((os.getcwd() + "/biomass-harvest_SetUp_s2e1_ALT.txt"), (os.getcwd() + "/biomass-harvest_SetUp_s2e1.txt"))
os.rename((os.getcwd() + "/biomass-harvest_SetUp_s2e1_OLD.txt"), (os.getcwd() + "/biomass-harvest_SetUp_s2e1_ALT.txt"))

print("Python script : Switching finished ! Ending...")
