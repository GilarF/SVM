using System.Collections.Generic;
using StardewModdingAPI;

namespace ModSettingsTab.Framework.Integration
{
    public class Param
    {
        public string Name { get; set; }
        public I18n Description { get; set; }
        public ParamType Type { get; set; }
        public I18n Label { get; set; }

        // CheckBox
        public bool CheckBoxIsChecked { get; set; }

        // DropDown
        public List<string> DropDownOptions { get; set; }
        public int DropDownSelectedOption { get; set; }

        // InputListener
        public SButton InputListenerButton { get; set; }

        // PlusMinus
        public List<string> PlusMinusOptions { get; set; }
        public int PlusMinusSelectedOption { get; set; }

        // Slider
        public int SliderValue { get; set; }
        public int SliderMaxValue { get; set; }
        public int SliderMinValue { get; set; }
        public int SliderStep { get; set; }

        // TextBox
        public string TextBoxText { get; set; }
        public bool TextBoxNumbersOnly { get; set; }
        public bool TextBoxFloatOnly { get; set; }
    }
}