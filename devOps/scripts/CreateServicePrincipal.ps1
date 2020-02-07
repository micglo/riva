Param(
	[Parameter(Mandatory=$true)][String]$SpName,
	[Parameter(Mandatory=$true)][String]$KeyVaultName,
	[Parameter(Mandatory=$true)][String]$SpSecretName,
	[Parameter(Mandatory=$true)][String]$CreatedSpAppIdVsoVariableName
)

function Create-ServicePrincipal {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$SpName,
		[Parameter(Mandatory=$true)][String]$KeyVaultName,
		[Parameter(Mandatory=$true)][String]$SpSecretName,
		[Parameter(Mandatory=$true)][String]$CreatedSpAppIdVsoVariableName
	)
	
	$sp = Get-ServicePrincipal -SpName $SpName

	if($sp -eq $null) {
		$sp = Create-RBACServicePrincipal -SpName $SpName -KeyVaultName $KeyVaultName -SpSecretName $SpSecretName
		$msg = "Service principal '$SpName' has been created. Secret has been assigned."
	} else {
		$msg = "Service principal '$SpName' already exist."
		$secret = Get-Secret -KeyVaultName $KeyVaultName -SecretName $SpSecretName
		if($secret -eq $null) {
			az ad sp delete --id $sp.objectId
			$sp = Create-RBACServicePrincipal -SpName $SpName -KeyVaultName $KeyVaultName -SpSecretName $SpSecretName
			$msg = "Service principal '$SpName' has been re-created. Secret has been assigned."
		}
	}

	$appId = $sp.appId
	Write-Host "##vso[task.setvariable variable=$CreatedSpAppIdVsoVariableName;]$appId"
	Write-Host $msg
}

function Get-ServicePrincipal {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$SpName
	)

	$spJson = az ad sp list --display-name $SpName
	$sp = $spJson | ConvertFrom-Json
	return $sp
}

function Create-RBACServicePrincipal {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$SpName,
		[Parameter(Mandatory=$true)][String]$KeyVaultName,
		[Parameter(Mandatory=$true)][String]$SpSecretName
	)
	Write-Host "Create sp for rbac"
	$spJson = az ad sp create-for-rbac --skip-assignment -n $SpName
	$sp = $spJson | ConvertFrom-Json
	Write-Host "Set keyvault secret"
	az keyvault secret set --name $SpSecretName --vault-name $KeyVaultName --value "$($sp.password)"
	return $sp
}

function Get-Secret {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$KeyVaultName,
		[Parameter(Mandatory=$true)][String]$SecretName
	)

	try {
		$secretJson = az keyvault secret show --vault-name $KeyVaultName --name $SecretName
		$secret = $secretJson | ConvertFrom-Json
		return $secret
	} catch {
		return $null
	}
}

Create-ServicePrincipal -SpName $SpName -KeyVaultName $KeyVaultName -SpSecretName $SpSecretName -CreatedSpAppIdVsoVariableName $CreatedSpAppIdVsoVariableName
