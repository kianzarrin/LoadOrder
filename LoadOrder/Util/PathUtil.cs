namespace LoadOrderTool.Util {
    using System;
    using System.IO;
    using System.Linq;

    public static class PathUtil {
        /// <summary>
        /// Non performant Path.Combine(params string[] paths) implementation for .NET 3.5 and lower, to be able to handle multiple values.
        /// Do not use it in the gameloop, or see the performance crumble, and the few fps you had left burn.
        /// </summary>
        /// <param name="paths">Paths to be combined.</param>
        /// <returns>Returns the full path.</returns>
        public static string Combine(params string[] paths) {
            if (paths is null) {
                throw new ArgumentNullException(nameof(paths));
            }

            return paths
                .Where(p => p != null)
                .Aggregate((a, b) => Path.Combine(a, b));
        }
    }
}
