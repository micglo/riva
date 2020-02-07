Param(
	[Parameter(Mandatory=$true)][String]$KeyVaultName,
	[Parameter(Mandatory=$true)][String]$SecretName,
	[Parameter(Mandatory=$true)][String]$SecretValue
)

function Set-Key-Vault-Secret {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$KeyVaultName,
		[Parameter(Mandatory=$true)][String]$SecretName,
		[Parameter(Mandatory=$true)][String]$SecretValue
	)
	
	$output = az keyvault secret show --vault-name $KeyVaultName --name $SecretName | ConvertFrom-Json
	if(!$output) {
		$output = az keyvault secret set --vault-name $KeyVaultName --name $SecretName --value $SecretValue | ConvertFrom-Json
		if(!$output) {
			Write-Error "Error! Could not set secret."
			return
		} else {
			Write-Host "Success! Secret set." -ForegroundColor Green
		}
	} else {
		Write-Host "Secret already exist."
	}
	
}

Set-Key-Vault-Secret -KeyVaultName $KeyVaultName -SecretName $SecretName -SecretValue $SecretValue