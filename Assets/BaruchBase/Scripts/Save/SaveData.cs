using System.Reflection;

namespace Baruch
{

    [System.Serializable]
    public struct SaveData
    {
        public FieldInfo FieldInfo;
        public object Value;

        public SaveData(FieldInfo fieldInfo,  object value)
        {
            FieldInfo = fieldInfo;
            Value = value;


        }

        public string FieldName => FieldInfo.Name;

        public override string ToString()
        {
            return $"{FieldInfo},{Value}";
        }
    }
}

