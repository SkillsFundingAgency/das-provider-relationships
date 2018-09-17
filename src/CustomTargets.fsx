Target "Restore Solution Packages" (fun _ ->
     "./SFA.DAS.ProviderRelationships.sln"
     |> RestoreMSSolutionPackages (fun p ->
         { p with
             OutputPath = ".\\packages"
             Retries = 4 })
 )