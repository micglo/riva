Param(
	[Parameter(Mandatory=$true)][String]$AcrName,
	[Parameter(Mandatory=$true)][String]$RgName,
	[Parameter(Mandatory=$true)][String]$VsoAcrUsernameVariableName,
	[Parameter(Mandatory=$true)][String]$VsoAcrPasswordVariableName
)

function Get-Acr-Credentials {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$AcrName,
		[Parameter(Mandatory=$true)][String]$RgName,
		[Parameter(Mandatory=$true)][String]$VsoAcrUsernameVariableName,
		[Parameter(Mandatory=$true)][String]$VsoAcrPasswordVariableName
	)
	
	Write-Host "Getting Azure Container Registry credentials"
	
	$acrUsername = az acr credential show -n $AcrName -g $RgName --query username
	$acrPassword = az acr credential show -n $AcrName -g $RgName --query passwords[0].value

	Write-Host "acrUsername: " $acrUsername
	Write-Host "acrPassword: " $acrPassword
	Write-Host "##vso[task.setvariable variable=$VsoAcrUsernameVariableName;]$acrUsername"
	Write-Host "##vso[task.setvariable variable=$VsoAcrPasswordVariableName;]$acrPassword"
}

Get-Acr-Credentials -AcrName $AcrName -RgName $RgName -VsoAcrUsernameVariableName $VsoAcrUsernameVariableName -VsoAcrPasswordVariableName $VsoAcrPasswordVariableName