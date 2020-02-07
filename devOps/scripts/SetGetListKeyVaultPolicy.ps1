Param(
	[Parameter(Mandatory=$true)][String]$KeyVaultName,
	[Parameter(Mandatory=$true)][String]$KeyVaultResourceGroupName,
	[Parameter(Mandatory=$true)][String]$ObjectId
)

function Set-Get-List-Key-Vault-Policy {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$KeyVaultName,
		[Parameter(Mandatory=$true)][String]$KeyVaultResourceGroupName,
		[Parameter(Mandatory=$true)][String]$ObjectId
	)
	
	$output = az keyvault set-policy --name $KeyVaultName --resource-group $KeyVaultResourceGroupName --object-id $ObjectId --certificate-permissions get list --key-permissions get list --secret-permissions get list
}

Set-Get-List-Key-Vault-Policy -KeyVaultName $KeyVaultName -KeyVaultResourceGroupName $KeyVaultResourceGroupName -ObjectId $ObjectId