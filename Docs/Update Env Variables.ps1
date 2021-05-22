
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
"Demo:ConnectionString:SqlDatabase"
}'

$keyvalues | Get-KeyValues | ForEach-Object {
	[System.Environment]::SetEnvironmentVariable($_.Key,$_.Value,[System.EnvironmentVariableTarget]::Machine)
	[System.Environment]::SetEnvironmentVariable($_.Key,$_.Value,[System.EnvironmentVariableTarget]::User)
	Write-Host Set $_.Key
}
