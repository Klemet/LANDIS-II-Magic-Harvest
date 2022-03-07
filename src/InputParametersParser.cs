// Author: Clément Hardy
// With many elements copied from the corresponding class
// in the "Base Fire" extension by Robert M. Scheller and James B. Domingo

using Landis.Utilities;
using System.Collections.Generic;
using Landis.Core;

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

            // We read the python script location
            InputVar<string> PythonScriptLocation = new InputVar<string>("PythonScriptLocation");
			ReadVar(PythonScriptLocation);
			parameters.PythonScriptLocation = PythonScriptLocation.Value;

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
