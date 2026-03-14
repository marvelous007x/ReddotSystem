using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class EnumSearchWindow : AdvancedDropdown
{
    private SerializedProperty _prop;
    private string[] _displayNames;

    public EnumSearchWindow(AdvancedDropdownState state, SerializedProperty prop) : base(state)
    {
        _prop = prop;
        _displayNames = prop.enumDisplayNames;
        // 设置窗口尺寸
        this.minimumSize = new Vector2(200, 300);
    }

    // 构建搜索树结构
    protected override AdvancedDropdownItem BuildRoot()
    {
        var root = new AdvancedDropdownItem("选择");

        for (int i = 0; i < _displayNames.Length; i++)
        {
            // 添加菜单项，并将索引作为 id
            root.AddChild(new GenericAdvancedDropdownItem<int>(_displayNames[i]){data = i});
        }

        return root;
    }

    // 当用户选中某一项时触发
    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        _prop.enumValueIndex = ((GenericAdvancedDropdownItem<int>)item).data;
        _prop.serializedObject.ApplyModifiedProperties();
    }
}

[CustomPropertyDrawer(typeof(Enum), true)]
public class EnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 开始绘制
        EditorGUI.BeginProperty(position, label, property);
        // 1. 绘制字段标签
        Rect contentRect = EditorGUI.PrefixLabel(position, label);

        // 2. 绘制一个看起来像下拉框的按钮
        // 显示当前选中的枚举名称
        string currentName = property.enumValueIndex == -1 ? "" : property.enumDisplayNames[property.enumValueIndex];
        if (GUI.Button(contentRect, currentName, EditorStyles.popup))
        {
            // 3. 点击按钮时，弹出搜索窗口
            var state = new AdvancedDropdownState();
            var searchWindow = new EnumSearchWindow(state, property);

            // 在按钮下方弹出
            searchWindow.Show(contentRect);
        }

        EditorGUI.EndProperty();
    }
}