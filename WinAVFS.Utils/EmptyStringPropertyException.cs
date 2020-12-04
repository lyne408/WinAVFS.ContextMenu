using System;

namespace WinAVFS.Utils
{
    class EmptyStringPropertyException:Exception
    {
        private string className;
        private string propertyName;
        private bool isInstanceProperty;
        public EmptyStringPropertyException(string className, string propertyName, bool isInstanceProperty = true)
        {
            this.className = className;
            this.propertyName = propertyName;
            this.isInstanceProperty = isInstanceProperty;

        }
        public new string Message = "The property <MenuText> should not be empty string.";
    }
}
