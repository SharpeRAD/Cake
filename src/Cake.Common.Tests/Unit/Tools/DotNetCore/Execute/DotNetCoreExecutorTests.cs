﻿using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Execute;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Execute;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Build
{
    public sealed class DotNetCoreExecutorTests
    {
        public sealed class TheExecuteMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Arguments = "--args";
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Arguments = "--args";
                fixture.Settings = new DotNetCoreExecuteSettings();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DotNetCore: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Arguments = "--args";
                fixture.Settings = new DotNetCoreExecuteSettings();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DotNetCore: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Settings = new DotNetCoreExecuteSettings();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/bin/Debug/app.dll", result.Args);
            }

            [Fact]
            public void Should_Add_Verbose()
            {
                // Given
                var fixture = new DotNetCoreExecutorFixture();
                fixture.AssemblyPath = "./bin/Debug/app.dll";
                fixture.Settings.Verbose = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--verbose /Working/bin/Debug/app.dll", result.Args);
            }
        }
    }
}
