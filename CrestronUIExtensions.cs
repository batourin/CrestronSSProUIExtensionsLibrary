using System;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       				// For Basic SIMPL#Pro classes

namespace Daniels.UI
{
    public static class UISLRJoins
    {
        public const string SetNumberOfItems = "Set Number of Items";

    }

    public class UISLRHelper
    {
        public readonly uint Id;
        private readonly uint _digitalIncrement;
        private readonly uint _analogIncrement;
        private readonly uint _serialIncrement;

        public readonly string SetNumberOfItems = "Set Number of Items";
        
        private const string booleanInput = "fb";
        private const string booleanOutput = "press";
        private const string ushortInput = "an_fb";
        private const string ushortOutput = "an_act";
        private const string stringInput = "text-o";
        private const string stringOutput = "text";

        public UISLRHelper(uint id, uint digitalIncrement, uint analogIncrement, uint serialIncerement)
        {
            Id = id;
            _digitalIncrement = digitalIncrement;
            _analogIncrement = analogIncrement;
            _serialIncrement = serialIncerement;
        }

        public string ItemVisible(uint index)
        {
            return "Item " + index + " Visible";
        }

        public string ItemEnable(uint index)
        {
            return "Item " + index + " Enable";
        }

        public string BooleanInput(uint index, uint join)
        {
            return booleanInput + ((index - 1) * _digitalIncrement + join).ToString();
        }
        public string BooleanOutput(uint index, uint join)
        {
            return booleanOutput + ((index - 1) * _digitalIncrement + join).ToString();
        }
        public string UShortInput(uint index, uint join)
        {
            return ushortInput + ((index - 1) * _analogIncrement + join).ToString();
        }
        public string UShortOutput(uint index, uint join)
        {
            return ushortOutput + ((index - 1) * _analogIncrement + join).ToString();
        }
        public string StringInput(uint index, uint join)
        {
            return stringInput + ((index - 1) * _serialIncrement + join).ToString();
        }
        public string StringOutput(uint index, uint join)
        {
            return stringOutput + ((index - 1) * _serialIncrement + join).ToString();
        }

    }
}

