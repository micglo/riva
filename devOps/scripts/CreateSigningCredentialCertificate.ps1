Param(
	[Parameter(Mandatory=$true)][String]$KeyVaultName,
	[Parameter(Mandatory=$true)][String]$SigningCredentialCertificateName
)

function Create-Signing-Credential-Certificate {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$KeyVaultName,
		[Parameter(Mandatory=$true)][String]$SigningCredentialCertificateName
	)
	
	$output = az keyvault certificate show --vault-name $KeyVaultName --name $SigningCredentialCertificateName | ConvertFrom-Json
	if(!$output) {
		$output =  az keyvault certificate create --vault-name $KeyVaultName --name $SigningCredentialCertificateName --policy "$(az keyvault certificate get-default-policy)" | ConvertFrom-Json
		if(!$output) {
			Write-Error "Error! Could not create certificate."
			return
		} else {
			Write-Host "Success! Certificate created." -ForegroundColor Green
		}
	} else {
		Write-Host "Certificate already exist."
	}
}

Create-Signing-Credential-Certificate -KeyVaultName $KeyVaultName -SigningCredentialCertificateName $SigningCredentialCertificateName