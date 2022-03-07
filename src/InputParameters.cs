// Author: Clément Hardy
// With many elements copied from the corresponding class
// in the "Base Fire" extension by Robert M. Scheller and James B. Domingo

using Landis.Utilities;
using System.Collections.Generic;
using System.Text;

namespace Landis.Extension.MagicHarvest
{
	/// <summary>
	/// Parameters for the plug-in.
	/// </summary>
	public interface IInputParameters
	{
		// ------------------------------------------------------------------------------
		// BASIC PARAMETERS

		/// <summary>
		/// Timestep (years)
		/// </summary>
		int Timestep
		{
			get; set;
		}

        /// <summary>
        /// The location of the harvest parameter fil, relative to the folder with the LANDIS-II scenario files.
        /// </summary>
        string HarvestExtensionParameterFile
        {
            get; set;
        }

        /// <summary>
        /// The location of the python file to run, relative to the folder with the LANDIS-II scenario files.
        /// </summary>
        string PythonScriptLocation
		{
			get; set;
		}
	}

	/// <summary>
	/// Parameters for the plug-in.
	/// </summary>
	public class InputParameters
		: IInputParameters
	{
		private int timestep;
        private string harvestExtensionParameterFile;
        private string pythonScriptLocation;

		// ------------------------------------------------------------------------------
		// BASIC PARAMETERS

		/// <summary>
		/// Timestep (years)
		/// </summary>
		public int Timestep
		{
			get
			{
				return timestep;
			}
			set
			{
				if (value < 0)
					throw new InputValueException(value.ToString(), "Value must be = or > 0.");
				timestep = value;
			}
		}

        /// <summary>
        /// The location of the harvest parameter fil, relative to the folder with the LANDIS-II scenario files.
        /// </summary>
        public string HarvestExtensionParameterFile
        {
            get
            {
                return harvestExtensionParameterFile;
            }
            set
            {
                harvestExtensionParameterFile = value;
            }
        }

        /// <summary>
        /// The location of the python file to run, relative to the folder with the LANDIS-II scenario files.
        /// </summary>
        public string PythonScriptLocation
        {
			get
			{
				return pythonScriptLocation;
			}
			set
			{
                pythonScriptLocation = value;
			}
		}

		public InputParameters()
		{
		}
	}
}
