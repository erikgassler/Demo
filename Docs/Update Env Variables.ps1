
function Get-KeyValues {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$True, ValueFromPipeline=$True)]
		[PSCustomObject]$obj
	)
	$obj | Get-Member -MemberType NoteProperty | ForEach-Object {
		$key = $_.Name
		[PSCustomObject]@{Key = $key; Value = $obj."$key"}
	}
}
Clear-Host

# Update empty strings with values from your Azure AAD Application
$keyvalues = ConvertFrom-Json '{
"Demo:Domain": "Demo.ws",
"Demo:ClientId": "<ClientId>",
"Demo:ClientSecret": "<ClientSecret>",
"Demo:TenantId": "<TenantId>",
"Demo:ConnectionStrings:SqlDatabase": "<SqlConnectionString>"
}'

$keyvalues | Get-KeyValues | ForEach-Object {
	# Set system variables to be ingested during debugging (Restart Visual Studio if running so it can load updated values).
	[System.Environment]::SetEnvironmentVariable($_.Key,$_.Value,[System.EnvironmentVariableTarget]::Machine)
	[System.Environment]::SetEnvironmentVariable($_.Key,$_.Value,[System.EnvironmentVariableTarget]::User)

	# set IIS values as well

	# Use me on initial setting to add
	Add-WebConfigurationProperty -pspath 'MACHINE/WEBROOT/APPHOST'  -filter "system.webServer/aspNetCore/environmentVariables" -name "." -value @{name=$_.Key;value=$_.Value}
    
	# Use me on subsequent settings to update
	#Set-WebConfigurationProperty -pspath 'MACHINE/WEBROOT/APPHOST'  -filter "system.webServer/aspNetCore/environmentVariables/environmentVariable[@name='$($_.Key)' and @value='$($_.Value)']" -name $_.Key -value $_.Value

	Write-Host Set $_.Key
}
