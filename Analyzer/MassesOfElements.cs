using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public static class MassesOfElements
    {
        //i think there is not any point to move this to json, cause it is only for codons that
        //require only five elements, so im leaving this as it is

        //all available elements and their masses
        static double OxygenMass = 15.999;
        static double NitrogenMass = 14.0067;
        static double HydrogenMass = 1.00784;
        static double SulfurMass = 32.065;
        static double CarbonMass = 12.011;

        /// <summary>
        /// Get mass of compound for example: CO2
        /// 
        /// Limitations: 
        /// - method does not count numbers higher than 9 so C12 will cause an error
        /// - there is only a couple of avaliable elements, see class for details
        /// </summary>
        public static double GetCompoundMass(string compound)
        {
            double mass = 0;

            string currentElement = string.Empty;
            compound += " ";

            for (int i = 0; i < compound.Length; i++)
            {
                var currentChar = compound[i];

                if ((char.IsUpper(currentChar) && currentElement == string.Empty) || char.IsLower(currentChar))
                {
                    currentElement += currentChar;
                    continue;
                }

                mass += GetElementMass(currentElement) * (char.IsDigit(currentChar) ? int.Parse(currentChar.ToString()) : 1);
                currentElement = currentChar.ToString();
            }

            return mass;
        }

        /// <summary>
        /// Get mass of element, returns 0 if element is not avaliable
        /// 
        /// Limitations:
        /// - there is only a couple of avaliable elements, see class for details
        /// </summary>
        public static double GetElementMass(string element)
        {
            return element switch
            {
                "O" => OxygenMass,
                "N" => NitrogenMass,
                "H" => HydrogenMass,
                "S" => SulfurMass,
                "C" => CarbonMass,
                _ => 0
            };
        }
    }
}
