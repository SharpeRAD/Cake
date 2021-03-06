﻿using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.Build
{
    /// <summary>
    /// .NET Core project builder.
    /// </summary>
    public sealed class DotNetCoreBuilder : DotNetCoreTool<DotNetCoreBuildSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreBuilder" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public DotNetCoreBuilder(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Build the project using the specified path and settings.
        /// </summary>
        /// <param name="project">The target project path.</param>
        /// <param name="settings">The settings.</param>
        public void Build(string project, DotNetCoreBuildSettings settings)
        {
            if (project == null)
            {
                throw new ArgumentNullException("path");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(project, settings));
        }

        private ProcessArgumentBuilder GetArguments(string project, DotNetCoreBuildSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("build");

            // Specific path?
            if (project != null)
            {
                builder.Append(project);
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("--output");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Temporary output directory
            if (settings.BuildBasePath != null)
            {
                builder.Append("--build-base-path");
                builder.AppendQuoted(settings.BuildBasePath.MakeAbsolute(_environment).FullPath);
            }

            // Runtime
            if (!string.IsNullOrEmpty(settings.Runtime))
            {
                builder.Append("--runtime");
                builder.Append(settings.Runtime);
            }

            // Frameworks
            if (settings.Frameworks != null && settings.Frameworks.Count > 0)
            {
                builder.Append("--framework");
                builder.AppendQuoted(string.Join(";", settings.Frameworks));
            }

            // Configuration
            if (!string.IsNullOrEmpty(settings.Configuration))
            {
                builder.Append("--configuration");
                builder.Append(settings.Configuration);
            }

            // Version suffix
            if (!string.IsNullOrEmpty(settings.VersionSuffix))
            {
                builder.Append("--version-suffix");
                builder.Append(settings.VersionSuffix);
            }

            // Build Profile
            if (settings.BuildProfile)
            {
                builder.Append("--build-profile");
            }

            // No Incremental
            if (settings.NoIncremental)
            {
                builder.Append("--no-incremental");
            }

            // No Incremental
            if (settings.NoDependencies)
            {
                builder.Append("--no-dependencies");
            }

            return builder;
        }
    }
}
