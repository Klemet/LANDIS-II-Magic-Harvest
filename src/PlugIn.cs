// Author: Clément Hardy

using Landis.Core;
using Landis.SpatialModeling;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HarvestMgmtLib = Landis.Library.HarvestManagement;

namespace Landis.Extension.MagicHarvest
{
	public class PlugIn
		: ExtensionMain
	{
		// Properties of the Plugin class : type (disturbance), name (Magic Harvest), 
		// and several other used for the output of data and the reading of its parameters.
		// The PlugIn class needs 4 functions : a constructor, LoadParameters, Initialize et Run
		public static readonly ExtensionType ExtType = new ExtensionType("disturbance:harvest");
		public static readonly string ExtensionName = "Magic Harvest";
		// Properties to contain the parameters
		private static IInputParameters parameters;

		// Properties to contain the "Core" object of LANDIS-II to reference it in other functions.
		private static ICore modelCore;

		//---------------------------------------------------------------------

		// Constructor of the PlugIn class. Heritates from the construction of the "ExtensionMain" class.
		// It just fills the properties containing the type of the extension and its name : nothing else.
		public PlugIn()
			: base(ExtensionName, ExtType)
		{

		}

		//---------------------------------------------------------------------
		// Properties to get the Model Core in read-only
		public static ICore ModelCore
		{
			get
			{
				return modelCore;
			}
		}

		//---------------------------------------------------------------------
		// Property to contain the parameters in read-only
		public static IInputParameters Parameters
		{
			get
			{
				return parameters;
			}
		}

		//---------------------------------------------------------------------

		// Function launched at the beginning of the LANDIS-II simulation to initialize the parameters of the extension.
		// It requires a reference to the .txt file where the parameters are.
		public override void LoadParameters(string dataFile, ICore mCore)
		{
            modelCore = mCore;

            // We read the parameters in the .txt file
            InputParameterParser parser = new InputParameterParser();
            parameters = Landis.Data.Load<IInputParameters>(dataFile, parser);
            modelCore.UI.WriteLine("   Magic Harvest : Parameters loaded");
        }

        //---------------------------------------------------------------------

        // Function launched before the simulation starts properly. Used to initialize other things.
        // In the case of this module, it's use to initialize the road network by checking different things.
        public override void Initialize()
		{
            Timestep = parameters.Timestep;
            modelCore.UI.WriteLine("   Magic Harvest : Initialized");
        }

		// Function called at every time step where the extension is activated. 
		// Contains the effects of the extension on the landscape.
		public override void Run()
		{
            // ----------------------------------------------------------------------------------
            // 1. DETECTING THE HARVEST EXTENSION

            modelCore.UI.WriteLine("Magic Harvest is now running");
            // modelCore.UI.WriteLine("Magic Harvest : Printing extensions detected");
            List<ExtensionMain> listOfExtensions = (List<ExtensionMain>)modelCore.GetType().GetField("disturbAndOtherExtensions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modelCore);
            // modelCore.UI.WriteLine(listOfExtensions.ToString());
            List<ExtensionMain> listOfHarvestExtension = new List<ExtensionMain>();
            foreach (ExtensionMain extension in listOfExtensions)
            {
                // modelCore.UI.WriteLine(extension.Name);
                if (extension.Name.Contains("Base Harvest") || extension.Name.Contains("Biomass Harvest")) { listOfHarvestExtension.Add(extension); }
            }
            ExtensionMain harvestExtension = listOfHarvestExtension[0];
            modelCore.UI.WriteLine("Magic Harvest : Selected the extension " + harvestExtension.Name + ".");

            // ----------------------------------------------------------------------------------

            // ----------------------------------------------------------------------------------
            // 2. LAUNCHING THE PYTHON SCRIPT

            // We create the command that we will launch in the cmd
            string processArgumentsReplaced;
            // If there is a {timestep} indicator in the command provided by the parameter, we replace it by the current timestep
            processArgumentsReplaced = parameters.ProcessArguments.Replace("{timestep}", modelCore.CurrentTime.ToString());
            if (processArgumentsReplaced.Contains("{none}")) { processArgumentsReplaced = ""; }
            // We launch the command and wait for it to finish
            modelCore.UI.WriteLine("Magic Harvest : Launching process " + parameters.ProcessToLaunch + " with arguments " + processArgumentsReplaced);
            // modelCore.UI.WriteLine("Magic Harvest : USING NON-CMD COMMAND");
            Process cmd = System.Diagnostics.Process.Start(parameters.ProcessToLaunch, processArgumentsReplaced);
            cmd.WaitForExit();
            modelCore.UI.WriteLine("Magic Harvest : Command has finished running");

            // We only re-initialize the harvest extension if it has not been disabled by the optional parameter NoHarvestReInitialization
            if (parameters.NoHarvestReInitialization is false)
            {

                // ----------------------------------------------------------------------------------

                // ----------------------------------------------------------------------------------
                // 3. EMPTYING THE LIST OF MANAGEMENT AREAS

                // We empty the list of stands associated with the management areas.
                // If we don't do this, then stands that should not be in a management area anymore could still be into it
                // after we re-initialize the management areas.
                // This is because stands are associated to management areas via a Add() function, but the list
                // in which they are put (ManagementArea.stands) is not "reset" during initialization.
                // See https://github.com/LANDIS-II-Foundation/Library-Harvest-Mgmt/blob/aec2572e341122ecd14a39cf2961dccbd3b6073c/src/Stands.cs#L69 for details.
                // We don't reset the area of the management area because this is redone in FinishInitialization() (see https://github.com/LANDIS-II-Foundation/Library-Harvest-Mgmt/blob/aec2572e341122ecd14a39cf2961dccbd3b6073c/src/ManagementArea.cs#L187)

                // We load the list of management areas currently in the harvest extension to access them.
                HarvestMgmtLib.ManagementAreaDataset managementAreasList = (HarvestMgmtLib.ManagementAreaDataset)harvestExtension.GetType().GetField("managementAreas", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(harvestExtension);
                foreach (HarvestMgmtLib.ManagementArea mgmtArea in managementAreasList)
                {
                    // We get the list of stands in the management area
                    List<HarvestMgmtLib.Stand> listOfStandsInManagementArea = (List<HarvestMgmtLib.Stand>)mgmtArea.GetType().GetField("stands", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(mgmtArea);

                    // We remove the association of the management area to the stand
                    foreach (HarvestMgmtLib.Stand stand in listOfStandsInManagementArea)
                    {
                        // The property of the stand that contains the management area is private; We use reflection to set it to 0.
                        stand.GetType().GetField("mgmtArea", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(stand, null);
                    }
                    // We set the "stands" property of the management area to a new, empty list. This make it so that the management area has no stands associated to it.
                    mgmtArea.GetType().GetField("stands", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mgmtArea, new List<HarvestMgmtLib.Stand>());
                }
                // We also empty the management areas loaded by the extension to re-initialize it later (what we did earlier was to make a copy).
                harvestExtension.GetType().GetField("managementAreas", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(harvestExtension, new HarvestMgmtLib.ManagementAreaDataset());

                HarvestMgmtLib.SiteVars.ManagementArea.SiteValues = null;

                // To remove if the above line functions
                //foreach (Site site in modelCore.Landscape.AllSites)
                //{
                //    HarvestMgmtLib.SiteVars.ManagementArea[site] = null;
                //}

                // ----------------------------------------------------------------------------------

                // ----------------------------------------------------------------------------------
                // 4. RESETTING THE STATIC NUMBER ID FOR PRESCRIPTIONS

                // we reset the static number that gives the unique number of each prescription
                // If we don't do that, then each time that the prescriptions are re-loaded, they will get a different number.
                // Might propose a parameter to change this.
                // For that, we need to find an instance of a prescription. As soon as we do, we can reset the number through
                // reflection, and keep going.
                // Notice that the binding flags used to retrieve this field is different than the others, because the field is static.
                Type prescription = typeof(HarvestMgmtLib.Prescription);
                FieldInfo prescriptionNextNumber = prescription.GetField("nextNumber", BindingFlags.NonPublic | BindingFlags.Static);
                prescriptionNextNumber.SetValue(null, 1);

                // ----------------------------------------------------------------------------------

                // ----------------------------------------------------------------------------------
                // 5. RELOADING THE PARAMETERS OF THE EXTENSION

                modelCore.UI.WriteLine("Magic Harvest : Re-loading the parameters of the harvest extension");

                // Now we take care of Biomass Harvest by reproducing https://github.com/LANDIS-II-Foundation/Extension-Biomass-Harvest/blob/e4fe90fb5a761ac00d01ee0ff39eae5f6d81b098/src/PlugIn.cs#L115
                if (harvestExtension.Name.Contains("Biomass Harvest"))
                {
                    modelCore.UI.WriteLine("Magic Harvest : Acting for Biomass Harvest...");

                    // Force harvest extension to re-read its parameters
                    modelCore.UI.WriteLine("Magic Harvest : Parsing parameters...");
                    Landis.Extension.BiomassHarvest.ParametersParser parser = new Landis.Extension.BiomassHarvest.ParametersParser(modelCore.Species);
                    HarvestMgmtLib.IInputParameters baseParameters = Landis.Data.Load<HarvestMgmtLib.IInputParameters>(parameters.HarvestExtensionParameterFile, parser);
                    BiomassHarvest.IParameters reloadedHarvestParameters = (BiomassHarvest.IParameters)baseParameters;

                    // We reload the site variables
                    modelCore.UI.WriteLine("Magic Harvest : Re-initializing site variables");
                    HarvestMgmtLib.SiteVars.GetExternalVars();
                    // HarvestMgmtLib.SiteVars.Initialize(); // Not working ?

                    // Re-initialize the management areas
                    modelCore.UI.WriteLine("Magic Harvest : Reading management areas map...");
                    // We update both the "real" managementAreas private property of the harvest extension; and the "copy" we made earlier,
                    // because we will need this copy updated to give it to ManagementAreas.ReadMap() below.
                    harvestExtension.GetType().GetField("managementAreas", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(harvestExtension, reloadedHarvestParameters.ManagementAreas);
                    managementAreasList = reloadedHarvestParameters.ManagementAreas;
                    // We make Biomass Harvest re-read the management area map
                    ModelCore.UI.WriteLine("   Reading management-area map {0} ...", reloadedHarvestParameters.ManagementAreaMap);
                    HarvestMgmtLib.ManagementAreas.ReadMap(reloadedHarvestParameters.ManagementAreaMap, managementAreasList);

                    // We make Biomass Harvest re-read the stands map
                    modelCore.UI.WriteLine("Magic Harvest : Reading stands map...");
                    PlugIn.ModelCore.UI.WriteLine("   Reading stand map {0} ...", reloadedHarvestParameters.StandMap);
                    HarvestMgmtLib.Stands.ReadMap(reloadedHarvestParameters.StandMap);

                    // We reload the site variables again (that's how it is in the code of Biomass Harvest)
                    modelCore.UI.WriteLine("Magic Harvest : Re-initializing site variables one more time");
                    HarvestMgmtLib.SiteVars.GetExternalVars();

                    // We finish initialisation of the management areas.
                    foreach (HarvestMgmtLib.ManagementArea mgmtArea in managementAreasList)
                    {
                        mgmtArea.FinishInitialization();
                    }

                    // We re-load the prescription maps
                    harvestExtension.GetType().GetField("prescriptionMaps", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(harvestExtension, new HarvestMgmtLib.PrescriptionMaps(reloadedHarvestParameters.PrescriptionMapNames));
                    harvestExtension.GetType().GetField("nameTemplate", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(harvestExtension, reloadedHarvestParameters.PrescriptionMapNames);

                    // We re-load the biomass map name if they have changed
                    if (reloadedHarvestParameters.BiomassMapNames != null)
                        harvestExtension.GetType().GetField("biomassMaps", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(harvestExtension, new BiomassHarvest.BiomassMaps(reloadedHarvestParameters.BiomassMapNames));

                    modelCore.UI.WriteLine("Magic Harvest : Re-loading is finished.");
                }
                else
                {
                    throw new Exception("Magic Harvest couldn't detect Biomass Harvest properly. Maybe another harvest extension has been used ?");
                }
                // ----------------------------------------------------------------------------------
            }


        } // End of run function

        public override void AddCohortData()
        {
            return;
        }
    } // End of PlugIn class
} // End of namespace
