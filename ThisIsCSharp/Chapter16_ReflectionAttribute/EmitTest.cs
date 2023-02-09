using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsCSharp.Chapter16_ReflectionAttribute
{
    class EmitTest
    {
        static void Main(string[] args)
        {
            AssemblyBuilder newAssembly =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    new AssemblyName("CalculatorAssembly"),
                    AssemblyBuilderAccess.Run);
            ModuleBuilder newModule = newAssembly.DefineDynamicModule("Calculator");

            TypeBuilder newType = newModule.DefineType("Sum1to100");

            MethodBuilder newMethod = newType.DefineMethod(
                "Calculate",
                MethodAttributes.Public,
                typeof(int),
                new Type[0]);

            ILGenerator generator = newMethod.GetILGenerator();

            generator.Emit(OpCodes.Ldc_I4, 1);

            for (int i =2; i <=100;i++)
            {
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Add);
            }

            generator.Emit(OpCodes.Ret);
            newType.CreateType();

            object sum1to100 = Activator.CreateInstance(newType);
            MethodInfo Calculate = sum1to100.GetType().GetMethod("Calculate");
            Console.WriteLine(Calculate.Invoke(sum1to100, null));
        }
    }
}
