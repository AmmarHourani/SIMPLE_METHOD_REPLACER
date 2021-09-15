using System;
//using System.Linq;
using System.Windows.Forms;

using System.Reflection;
using HOOKMEDLL;

using System.Runtime.CompilerServices;



namespace TestDLL
{
    public static class Class1
    {


        public static int TestMethod(string TestParam)
        {
            MessageBox.Show("Yay, C# DLL Injected Successfully !");
            MessageBox.Show(TestParam + "\nSURE !" );
            

            MethodInfo methodToReplace = typeof(Calculator).GetMethod("Add", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            MethodInfo methodToInject = typeof(Class1).GetMethod("MyPrefix", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            Replace(methodToReplace, methodToInject);
            return 0;
        }

        static void MyPrefix(ref int __result, int x, int y)
        {
            MessageBox.Show("prefix being executed");
            __result = x + y + 99;
            // skip the original

        }


        public static unsafe MethodReplacementState Replace(this MethodInfo methodToReplace, MethodInfo methodToInject)
        {
            //#if DEBUG
            RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
            RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);
            //#endif
            MethodReplacementState state;

            IntPtr tar = methodToReplace.MethodHandle.Value;
            if (!methodToReplace.IsVirtual)
                tar += 8;
            else
            {
                var index = (int)(((*(long*)tar) >> 32) & 0xFF);
                var classStart = *(IntPtr*)(methodToReplace.DeclaringType.TypeHandle.Value + (IntPtr.Size == 4 ? 40 : 64));
                tar = classStart + IntPtr.Size * index;
            }
            var inj = methodToInject.MethodHandle.Value + 8;
#if DEBUG
            tar = *(IntPtr*)tar + 1;
            inj = *(IntPtr*)inj + 1;
            state.Location = tar;
            state.OriginalValue = new IntPtr(*(int*)tar);

            *(int*)tar = *(int*)inj + (int)(long)inj - (int)(long)tar;
            return state;

#else
            state.Location = tar;
            state.OriginalValue = *(IntPtr*)tar;
            *(IntPtr*)tar = *(IntPtr*)inj;
            return state;
#endif
        }
    }

    public struct MethodReplacementState : IDisposable
    {
        internal IntPtr Location;
        internal IntPtr OriginalValue;
        public void Dispose()
        {
            this.Restore();
        }

        public unsafe void Restore()
        {
#if DEBUG
            *(int*)Location = (int)OriginalValue;
#else
            *(IntPtr*)Location = OriginalValue;
#endif
        }
    }
}

 