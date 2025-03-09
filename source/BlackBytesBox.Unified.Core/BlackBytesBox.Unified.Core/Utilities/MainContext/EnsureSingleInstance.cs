namespace BlackBytesBox.Unified.Core.Utilities.MainContext
{
    using System;
    using System.Reflection;
    using System.Threading;

    /// <summary>
    /// Provides utility methods for managing the application's execution context, primarily for use within the Program.Main entry point.
    /// </summary>
    public static partial class MainContext
    {
#pragma warning disable IDE0052 // Remove unread private members

        // Holds the application's mutex to ensure a single instance.
        private static Mutex? applicationMutex;

#pragma warning restore IDE0052 // Remove unread private members

        /// <summary>
        /// Ensures that only a single instance of the application is running.
        /// If another instance is detected, the specified duplicate instance action is executed,
        /// and optionally the application is terminated.
        /// </summary>
        /// <param name="duplicateInstanceAction">
        /// An <see cref="Action"/> to execute if a duplicate instance is detected.
        /// This action can be used to notify the user or log the occurrence of a duplicate instance.
        /// </param>
        /// <param name="exitOnDuplicate">
        /// A boolean value indicating whether to terminate the application when a duplicate instance is detected. Defaults to true.
        /// </param>
        /// <remarks>
        /// The method attempts to create a named <see cref="Mutex"/> using the application's entry assembly name.
        /// If the mutex already exists, it indicates that another instance of the application is running.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Ensure single instance; if a duplicate is detected, notify the user and exit.
        /// MainContext.EnsureSingleInstance(() => Console.WriteLine("Another instance is already running."));
        /// </code>
        /// </example>
        public static void EnsureSingleInstance(Action duplicateInstanceAction, bool exitOnDuplicate = true)
        {
            // Retrieve the application name from the entry assembly or use a default value.
            string entryAssemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? "UniqueAppName";
            // Attempt to create a mutex with the application name.
            applicationMutex = new Mutex(true, entryAssemblyName, out bool createdNew);

            // If the mutex was not created, it means another instance is already running.
            if (!createdNew)
            {
                duplicateInstanceAction();
                if (exitOnDuplicate)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}