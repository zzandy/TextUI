using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using TextUI.Interfaces;

namespace TextUI.Rendering
{
    public sealed class Screen
    {
        private SafeFileHandle visible;
        private SafeFileHandle pending;

        public IRender Root { get; set; }

        public Screen(IRender root=null)
        {
            Root = root;
            visible = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            pending = CreateConsoleScreenBuffer(0x40000000, 2, IntPtr.Zero, 1, IntPtr.Zero);
        }

        public void Render()
        {
            if(Root==null)throw new ApplicationException("Root UI element must be set.");

            short h = (short)Console.WindowHeight;
            short w = (short)Console.WindowWidth;

            var buf = new CharInfo[w * h];

            SmallRect rect = new SmallRect { Left = 0, Top = 0, Right = w, Bottom = h };

            Root.Render(new Canvas(w, h, buf));

            WriteConsoleOutput(pending, buf,
                          new Coord { X = w, Y = h },
                          new Coord { X = 0, Y = 0 },
                          ref rect);

            SetConsoleActiveScreenBuffer(pending.DangerousGetHandle());

            var tmp = pending;
            pending = visible;
            visible = tmp;
        }

        public sealed class Canvas : ICanvas
        {
            private readonly CharInfo[] buf;

            public Canvas(short w, short h, CharInfo[] buf)
            {
                this.W = w;
                this.H = h;
                this.buf = buf;
            }

            public short W { get; }

            public short H { get; }

            public int Width => W;

            public int Height => H;

            public void Put(int x, int y, char character)
            {
                buf[y * W + x].Attributes = 1 | 2 | 8 | 0x10;
                buf[y * W + x].Char.UnicodeChar = character;
            }
        }

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint GetLastError();

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetConsoleScreenBufferInfo(
              IntPtr hConsoleOutput,
              ref ConsoleScreenBufferInfo lpConsoleScreenBufferInfo
            );


        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
          string fileName,
          [MarshalAs(UnmanagedType.U4)] uint fileAccess,
          [MarshalAs(UnmanagedType.U4)] uint fileShare,
          IntPtr securityAttributes,
          [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
          [MarshalAs(UnmanagedType.U4)] int flags,
          IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateConsoleScreenBuffer(
           [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr buffer);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SetConsoleActiveScreenBuffer(IntPtr handle);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Color
        {
            public readonly byte R;
            public readonly byte G;
            public readonly byte B;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleScreenBufferInfo
        {
            public uint cbSize;
            public Coord dwSize;
            public Coord dwCursorPosition;
            public short wAttributes;
            public SmallRect srWindow;
            public Coord dwMaximumWindowSize;

            public ushort wPopupAttributes;
            public bool bFullscreenSupported;

            //internal Color black;
            //internal Color darkBlue;
            //internal Color darkGreen;
            //internal Color darkCyan;
            //internal Color darkRed;
            //internal Color darkMagenta;
            //internal Color darkYellow;
            //internal Color gray;
            //internal Color darkGray;
            //internal Color blue;
            //internal Color green;
            //internal Color cyan;
            //internal Color red;
            //internal Color magenta;
            //internal Color yellow;
            //internal Color white;

            // has been a while since I did this, test before use
            // but should be something like:
            //
            // [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
            // public COLORREF[] ColorTable;
        }
    }
}