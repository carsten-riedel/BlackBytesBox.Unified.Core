using System;
using System.IO;

using BlackBytesBox.Unified.Core.Extensions.StringExtensions;

namespace BlackBytesBox.Unified.Core.Utilities.MainContext
{
    /// <summary>
    /// Provides utility methods for managing the application's execution context, primarily for use within the Program.Main entry point.
    /// </summary>
    public static partial class MainContext
    {
        /// <summary>
        /// Retrieves or creates a directory based on the application's name, which is derived from the primary file location.
        /// Optionally, a unique identifier is appended to the directory name when the <paramref name="useStartupAppLocation"/> flag is set to true.
        /// </summary>
        /// <param name="candidateBaseDirectories">
        /// An optional array of candidate base directories where the application directory should be searched for or created.
        /// If not provided, a default set of system directories will be used.
        /// </param>
        /// <param name="useStartupAppLocation">
        /// When set to true, appends a unique identifier (derived from the primary file location) to the application directory name.
        /// </param>
        /// <param name="subdirectoriesToEnsure">
        /// An optional array of subdirectory names that should be ensured (created if missing) within the application directory.
        /// </param>
        /// <param name="throwIfFails">
        /// Indicates whether an exception should be thrown if the operation to retrieve or create the directory fails.
        /// </param>
        /// <returns>
        /// A <see cref="DirectoryInfo"/> object representing the application directory if successfully retrieved or created; otherwise, null.
        /// </returns>
        /// <example>
        /// <code>
        /// // Retrieve or create the application directory using default settings.
        /// var configDirectory = MainContext.TryGetOrCreateAppNameDirectory();
        /// </code>
        /// </example>
        public static DirectoryInfo? TryGetOrCreateAppNameDirectory(string[]? candidateBaseDirectories = null, bool useStartupAppLocation = false, string[]? subdirectoriesToEnsure = null, bool throwIfFails = false)
        {
            // Get the primary file location of the application.
            var primaryFileLocation = TryGetPrimaryFileLocation();
            // Generate a short unique identifier based on the primary file location.
            var primaryFileIdentifier = primaryFileLocation.ToShortUUID();
            // Derive the application directory name from the primary file name (without its extension).
            string? applicationDirectoryName = Path.GetFileNameWithoutExtension(primaryFileLocation);
            if (string.IsNullOrEmpty(applicationDirectoryName))
            {
                return null;
            }

            // Append the unique identifier if requested.
            if (useStartupAppLocation)
            {
                applicationDirectoryName = $"{applicationDirectoryName}_{primaryFileIdentifier}";
            }

            // Use the provided candidate directories or fall back to a set of default directories.
            candidateBaseDirectories ??= new[]
            {
                AppContext.BaseDirectory,
                AppDomain.CurrentDomain.BaseDirectory,
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Path.GetTempPath()
            };

            // Iterate through the candidate directories to find or create a valid application directory.
            foreach (var baseDirectory in candidateBaseDirectories)
            {
                var resultDirectory = EnsureWriteableDirectoryExists(baseDirectory, applicationDirectoryName, throwIfFails);
                if (resultDirectory != null)
                {
                    // Ensure that the specified subdirectories exist within the application directory.
                    if (subdirectoriesToEnsure != null)
                    {
                        foreach (var subdirectory in subdirectoriesToEnsure)
                        {
                            EnsureWriteableDirectoryExists(resultDirectory.FullName, subdirectory, throwIfFails);
                        }
                    }
                    return resultDirectory;
                }
            }

            return null;
        }
    }
}
