using System.Collections.Generic;
using StardewModdingAPI;

namespace ModSettingsTab.Framework.Integration
{
    public class Param
    {
        public string Name { get; set; } = null;
        public I18n Description { get; set; } = null;
        public ParamType Type { get; set; } = ParamType.TextBox;
        public I18n Label { get; set; } = null;

        // DropDown
        public List<string> DropDownOptions { get; set; } = new List<string>{"EMPTY"};

        // InputListener
        public SButton InputListenerButton { get; set; } = SButton.None;

        // PlusMinus
        public List<string> PlusMinusOptions { get; set; } = new List<string>{"EMPTY"};

        // Slider
        public int SliderMaxValue { get; set; } = 100;
        public int SliderMinValue { get; set; } = 0;
        public int SliderStep { get; set; } = 1;

        // TextBox
        public bool TextBoxNumbersOnly { get; set; } = false;
        public bool TextBoxFloatOnly { get; set; }  = false;
    }
}