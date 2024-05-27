// Author: Clément Hardy
// With many elements copied from the corresponding class
// in the "Base Fire" extension by Robert M. Scheller and James B. Domingo

using Landis.Utilities;
using System.Collections.Generic;
using Landis.Core;
using System.Data;
using System;

namespace Landis.Extension.MagicHarvest
{
	/// <summary>
	/// A parser that reads the plug-in's parameters from text input.
	/// </summary>
	// The class heritates from the text parser of LANDIS-II, which contains properties that allows it to know where it is in the file
	public class InputParameterParser
		: TextParser<IInputParameters>
	{

		//---------------------------------------------------------------------
		public override string LandisDataValue
		{
			get
			{
				return PlugIn.ExtensionName;
			}
		}
		public InputParameterParser()
		{
			// FIXME from Scheller and Domingo: Hack to ensure that Percentage is registered with InputValues
			Landis.Utilities.Percentage p = new Landis.Utilities.Percentage();
		}

		//---------------------------------------------------------------------

		protected override IInputParameters Parse()
		{
			// ------------------------------------------------------------------------------
			// BASIC PARAMETERS

			// To start, we look at the "LandisData" parameter. If it's not the name of the extension, 
			// we raise an exception.
			InputVar<string> landisData = new InputVar<string>("LandisData");
			ReadVar(landisData);
			if (landisData.Value.Actual != PlugIn.ExtensionName)
				throw new InputValueException(landisData.Value.String, "The value is not \"{0}\"", PlugIn.ExtensionName);

			// We create the object that will contain our parameters
			InputParameters parameters = new InputParameters();

			// We read the timestep
			InputVar<int> timestep = new InputVar<int>("Timestep");
			ReadVar(timestep);
			parameters.Timestep = timestep.Value;

            // We read the harvest extension parameter file location
            InputVar<string> HarvestExtensionParameterFile = new InputVar<string>("HarvestExtensionParameterFile");
            ReadVar(HarvestExtensionParameterFile);
            parameters.HarvestExtensionParameterFile = HarvestExtensionParameterFile.Value;

            // We read the process to launch
            InputVar<string> ProcessToLaunch = new InputVar<string>("ProcessToLaunch");
			ReadVar(ProcessToLaunch);
			parameters.ProcessToLaunch = ProcessToLaunch.Value;

            // We read the arguments, if they are here
            InputVar<string> ProcessArguments = new InputVar<string>("ProcessArguments");
            ReadVar(ProcessArguments);
            parameters.ProcessArguments = ProcessArguments.Value;

			// We read the optional parameter that will overrride the re-initialization of the harvest extension
            InputVar<string> NoHarvestReInitialization = new InputVar<string>("NoHarvestReInitialization");
			try
			{
				ReadVar(NoHarvestReInitialization);
				if (NoHarvestReInitialization.Value.ToString().ToLower() != "false" & NoHarvestReInitialization.Value.ToString().ToLower() != "true")
				{
					throw new Exception("NoHarvestReInitialization parameter value should be true or false.");
				}
				else { parameters.NoHarvestReInitialization = bool.Parse(NoHarvestReInitialization.Value.ToString().ToLower()); }
				
			}
			catch (Exception ex)
			{
				if (!ex.Message.Contains("Expected the name \"NoHarvestReInitialization\""))
				{
					throw;
				}
				else
				{
					Console.WriteLine($"Additional parameter \"NoHarvestReInitialization\" not detected, put to \"false\" by default.");
					parameters.NoHarvestReInitialization = false;
				}
            }
            

            // Now that everything is done, we return the parameter object.
            return (parameters);
		}

		//---------------------------------------------------------------------

		private void ValidatePath(InputValue<string> path)
		{
			if (path.Actual.Trim(null) == "")
				throw new InputValueException(path.String,
											  "Invalid file path: {0}",
											  path.String);
		}

	}
}
