using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class GenericAdvancedDropdownItem<T> : AdvancedDropdownItem
{
    public T data;
    public  GenericAdvancedDropdownItem(string name) : base(name)
    {}
}
