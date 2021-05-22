# Important - This file is for setting up a new IIS website running on a Windows 10 machine.
# You need to have already installed IIS on your machine before running this.

# Important - Set the correct paths here!
$MyRepoRootPath = "C:\Demo\IIS\" # Make sure to add folder permissions for IUSR / IIS_IUSRS
$certStorage = "C:\Demo\Certs\" # Storage folder for the website certificate to be saved in

# Okay to run if already setup - will skip if domain is already setup in IIS
$setupAppsToIIS = @(
	@{
		IISProjectName	= "Demo";
		FullDomainName	= "Demo.ws"; # https://Demo.ws
		AppId			= 0; # Unique number from 0-155 - 0 binds domain to IP address 127.0.0.100
	}
}

# You should not need to change anything below this line
$hostsFile = "$env:windir\System32\drivers\etc\hosts" # Windows hosts file - you should not need to change this.
function SetupWebsiteSSL {
	Param($name, $domain)
	Process {
		CreateSelfSignedCertInMyStore $domain
		SetWebsiteBinding $name $domain
		ExportCert $domain
		AddCertToRootStore $domain
	}
}

function SetWebsiteBinding{
	Param($name, $domain)
	Process {
		$cert = (Get-ChildItem cert:\LocalMachine\My | where-object { $_.Subject -like "*$domain*" } | Select-Object -First 1).Thumbprint
		$httpsBinding = Get-WebBinding -Name "$name" -Protocol "https"
		$httpsBinding.AddSslCertificate($cert, "my")
	}
}

function CreateSelfSignedCertInMyStore{
	Param($domain)
	Process {
		New-SelfSignedCertificate -DnsName $domain -CertStoreLocation cert:\LocalMachine\My -NotAfter (Get-Date).AddYears(10)
	}
}

function ExportCert{
	Param($domain)
	Process {
		$cert = (Get-ChildItem cert:\LocalMachine\My | where-object { $_.Subject -like "*$domain*" } | Select-Object -First 1).Thumbprint
		Get-Item -Path Cert:\LocalMachine\My\$cert  |
		Export-Certificate -Type P7B -FilePath "$($certStorage)$domain.p7b"  -Verbose
	}
}

function AddCertToRootStore{
	Param($domain)
	Process {
		CERTUTIL -addstore -enterprise -f -v root "$($certStorage)$domain.p7b"
	}
}

Set-Location $env:SystemRoot\system32\inetsrv\
function AddWebsiteToIIS {
	Param (
		[string]$name,
		[string]$domain,
		[int]$id
	)
	if ($(Get-Website | Where-Object { $_.Name -eq $name }) -eq $null) {
		$minorIP = 100 + $id

		$fulldomain = $domain
		Invoke-Expression ".\appcmd.exe add site /name:$name /id:$id /physicalPath:$MyRepoRootPath$name\wwwroot\ /bindings:http/127.0.$id.$($minorIP):80:$($fulldomain),https/127.0.$id.$($minorIP):443:$($fulldomain)"
		Write-Host "$name was successfully setup" -ForegroundColor Green
		"127.0.$id.$($minorIP) $fulldomain" | Add-Content -PassThru $hostsFile
		SetupWebsiteSSL $name $fulldomain
	} else {
		Write-Host "$name already exists" -ForegroundColor Gray
	}
	set-itemproperty IIS:\Sites\$name -name applicationDefaults.preloadEnabled -value True
}
Clear-Host

import-module webadministration
foreach ($app in $setupAppsToIIS) {
	AddWebsiteToIIS $app.IISProjectName $app.FullDomainName $app.AppId
}
