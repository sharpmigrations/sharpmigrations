function Update-Database {
	param(
		$version = -1,
		[String] $assemblyPath,
		[String] $connectionString,
		[String] $provider,
		[String] $group = "default"
	)
	
	if($assemblyPath -eq "") {
		$assemblyPath = Get-AssemblyPath
	}
	
	Write-Host "---------------------------------------------------"
	Write-Host "SharpMigrator PowerShell"
	Write-Host "---------------------------------------------------"
	Write-Host "Assembly: " $assemblyPath
	
	if($connectionString -eq "") {
		$connectionStringAndProvider = Get-ConnectionString
		$connectionString = $connectionStringAndProvider[0]
		$provider = $connectionStringAndProvider[1]
	}
	elseif($provider -eq "") {
		WriteHost "Please. specify a provider"
		Exit
	}
	
	$args = @("-a",
			$assemblyPath,
			"-p",
			$provider,
			"-c",
			$connectionstring,
			"-g",
			$group,
			"-m",
			"auto",
			"-v",
			$version
			)            
	& "SharpMigrator.exe" $args
}

function Invoke-Seed {
	param(
		[string] $seedName = $(throw "please, inform seed name"),
		[string] $seedArgs,
		[String] $assemblyPath,
		[String] $connectionString,
		[String] $provider,
		[String] $group = "default"
	)
	
	if($assemblyPath -eq "") {
		$assemblyPath = Get-AssemblyPath
	}
	
	Write-Host "---------------------------------------------------"
	Write-Host "SharpMigrator PowerShell"
	Write-Host "---------------------------------------------------"
	Write-Host "Assembly: " $assemblyPath
	
	if($connectionString -eq "") {
		$connectionStringAndProvider = Get-ConnectionString
		$connectionString = $connectionStringAndProvider[0]
		$provider = $connectionStringAndProvider[1]
	}
	elseif($provider -eq "") {
		WriteHost "Please. specify a provider"
		Exit
	}

	$args = @("-a",
			$assemblyPath,
			"-p",
			$provider,
			"-c",
			$connectionstring,
			"-g",
			$group,
			"-m",
			"seed",
			"-s",
			$seedName
			)        
	if($seedArgs -ne "") {
		$args+="-i"
		$args+=$seedArgs
	}		    
	& 'SharpMigrator.exe' $args
}

function Get-AssemblyPath {
  $project = Get-Project
	$outputPath = $project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
	$assemblyName = $project.Properties.Item("AssemblyName").Value.ToString();
	
	$outputType = $project.Properties.Item("OutputType").Value.ToString();
	if ($outputType -eq "1") {
		$extension = ".exe"
	}
	else {
		$extension = ".dll"
	} 
	$fileInfo = new-object -typename System.IO.FileInfo -ArgumentList $project.FullName
	$projectDirectory = $fileInfo.DirectoryName
	
	$absoluteOutputPath = Join-Path $projectDirectory $outputPath
	$assemblyPath = Join-Path $absoluteOutputPath $assemblyName
	$assemblyPath = $assemblyPath + $extension
	return $assemblyPath
}

function Get-ConnectionString {
  $appConfigFile = $assemblyPath + ".config"
  Write-Host "---------------------------------------------------"
	Write-Host "Available connection strings in app.config:"
	Write-Host "---------------------------------------------------"
	$appConfig = [xml](cat $appConfigFile)
	$i = 1
	$connectionStrings = @()
	$providers = @()
	foreach($connString in $appConfig.configuration.connectionStrings.add) {
		Write-Host "" [$($i)] Name: $connString.name " ConnStr: "  $connString.connectionString
		$connectionStrings+= $connString.connectionString
		$providers+= $connString.providerName
		$i = $i + 1
	}
	[Int]$connectionStringIndex = Read-Host "Pick one or (0) to exit: " 
	if($connectionStringIndex -eq 0) {
		Exit
	}
	$connectionStringIndex = $connectionStringIndex -1;
	$connectionString = $connectionStrings[$connectionStringIndex]
	$provider = $providers[$connectionStringIndex]
	
	return @($connectionString, $provider)
}

Export-ModuleMember -function Update-Database;
Export-ModuleMember -function Invoke-Seed;