// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open System

// Properties
let buildDir = "./build/"
let solutionFile = "crate-mono.sln"


// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "Build" (fun _ ->
    !! "crate-mono.sln"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)


Target "RunTests" (fun _ ->
    !! (buildDir + "/Crate.*.Test.dll")
    |> NUnit (fun p ->
        { p with
            DisableShadowCopy = true
            TimeOut = TimeSpan.FromMinutes 20.
            ToolPath = "packages/NUnit.Runners/tools/"
            OutputFile = "TestResult.xml" })
)


// Dependencies
"Clean"
  ==> "Build"
  ==> "RunTests"


// start build
RunTargetOrDefault "Build"
