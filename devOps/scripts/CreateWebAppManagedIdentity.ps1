Param(
	[Parameter(Mandatory=$true)][String]$WebAppName,
	[Parameter(Mandatory=$true)][String]$WebAppResourceGroupName,
	[Parameter(Mandatory=$true)][String]$CreatedIdentityPrincipalIdVsoVariableName
)

function Create-WebApp-Managed-Identity {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$WebAppName,
		[Parameter(Mandatory=$true)][String]$WebAppResourceGroupName,
		[Parameter(Mandatory=$true)][String]$CreatedIdentityPrincipalIdVsoVariableName
	)
	
	$identity = az webapp identity show --name $WebAppName --resource-group $WebAppResourceGroupName  | ConvertFrom-Json
	if(!$identity) {
		$identity = az webapp identity assign --name $WebAppName --resource-group $WebAppResourceGroupName  | ConvertFrom-Json
		if(!$identity) {
			Write-Error "Error! Could not create identity"
			return
		} else {
			Write-Host "Success! Identity created." -ForegroundColor Green
		}
	}
	
	$identityPrincipalId = $identity.principalId
	Write-Host "##vso[task.setvariable variable=$CreatedIdentityPrincipalIdVsoVariableName;]$identityPrincipalId"
}

Create-WebApp-Managed-Identity -WebAppName $WebAppName -WebAppResourceGroupName $WebAppResourceGroupName -CreatedIdentityPrincipalIdVsoVariableName $CreatedIdentityPrincipalIdVsoVariableName