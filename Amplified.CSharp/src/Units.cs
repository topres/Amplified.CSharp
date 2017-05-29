using System;
using System.Threading.Tasks;
using Amplified.CSharp.Internal.Extensions;

namespace Amplified.CSharp
{
    public static class Units
    {
        public static Unit Unit() => default(Unit);

        public static Func<Unit> Unit(Action action) => () =>
        {
            action();
            return default(Unit);
        };

        public static Func<T, Unit> Unit<T>(Action<T> action) => arg =>
        {
            action(arg);
            return default(Unit);
        };

        public static Func<None, Unit> Unit(Action<None> action) => arg =>
        {
            action(arg);
            return default(Unit);
        };

        public static Func<Task<Unit>> Unit(Func<Task> func) => () => func().WithResult(default(Unit));

        public static Func<T, Task<Unit>> Unit<T>(Func<T, Task> func) => arg => func(arg).WithResult(default(Unit));
    }
}