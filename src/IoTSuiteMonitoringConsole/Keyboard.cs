using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danvy.Tools
{
    public static class Keyboard
    {
        private const int KeyPressed = 0x8000;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetKeyState(int key);
        public static bool IsKeyDown(VirtualKey key)
        {
            return (GetKeyState((int)key) & KeyPressed) != 0;
        }
    }
}
