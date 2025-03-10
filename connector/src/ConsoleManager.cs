/*
 * Author: Alessandro de Oliveira Binhara
 * Igloobe Company
 * 
 * This file implements the ConsoleManager static class that provides utility 
 * methods for console window management in the Igloobe system. It allows creating, 
 * showing, hiding, and closing console windows for debugging and diagnostic purposes.
 */
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.IO;
using System.Diagnostics;

namespace Br.Com.IGloobe.Connector {

    [SuppressUnmanagedCodeSecurity]
    public static class ConsoleManager {

        private const string Kernel32DllName = "kernel32.dll";

        [DllImport(Kernel32DllName)]
        private static extern bool AllocConsole();

        [DllImport(Kernel32DllName)]
        private static extern bool FreeConsole();

        [DllImport(Kernel32DllName)]
        private static extern IntPtr GetConsoleWindow();

        //[DllImport(Kernel32_DllName)]
        //private static extern int GetConsoleOutputCP();

        public static bool HasConsole {
            get { return GetConsoleWindow() != IntPtr.Zero; }
        }

        public static void Show() {
            if (HasConsole) return;

            AllocConsole();
            InvalidateOutAndError();
        }

        public static void Hide() {
            if (!HasConsole) return;

            SetOutAndErrorNull();
            FreeConsole();
        }

        public static void Toggle() {
            if (HasConsole) {
                Hide();
                return;
            }

            Show();
        }

        static void InvalidateOutAndError() {
            Type type = typeof(Console);

            System.Reflection.FieldInfo output = type.GetField("_out",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.FieldInfo error = type.GetField("_error",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.MethodInfo initializeStdOutError = type.GetMethod("InitializeStdOutError",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Debug.Assert(output != null);
            Debug.Assert(error != null);

            Debug.Assert(initializeStdOutError != null);

            output.SetValue(null, null);
            error.SetValue(null, null);

            initializeStdOutError.Invoke(null, new object[] { true });
        }

        static void SetOutAndErrorNull() {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
}
