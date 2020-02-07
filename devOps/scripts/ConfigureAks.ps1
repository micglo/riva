Param(
	[Parameter(Mandatory=$true)][String]$RgName,
	[Parameter(Mandatory=$true)][String]$AksName,
	[Parameter(Mandatory=$true)][String]$AcrName,
	[Parameter(Mandatory=$true)][String]$AksNamespaceName,
	[Parameter(Mandatory=$true)][String]$AcrPushSpName,
	[Parameter(Mandatory=$true)][String]$AcrPushSpSecretName,
	[Parameter(Mandatory=$true)][String]$KeyVaultName
)

function Configure-AKS {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$RgName,
		[Parameter(Mandatory=$true)][String]$AksName,
		[Parameter(Mandatory=$true)][String]$AcrName,
		[Parameter(Mandatory=$true)][String]$AksNamespaceName,
		[Parameter(Mandatory=$true)][String]$AcrPushSpName,
		[Parameter(Mandatory=$true)][String]$AcrPushSpSecretName,
		[Parameter(Mandatory=$true)][String]$KeyVaultName
	)
	
	az aks get-credentials -g $RgName -n $AksName
	Create-AKS-ServiceAccount-Tiller
	Create-AKS-Namespace -AksNamespaceName $AksNamespaceName
	Assign-AKS-ACR-Premissions -RgName $RgName -AksName $AksName -AcrName $AcrName -AcrPushSpName $AcrPushSpName -AcrPushSpSecretName $AcrPushSpSecretName -KeyVaultName $KeyVaultName 
}

function Create-AKS-ServiceAccount-Tiller {
	$serviceAccounts = kubectl get serviceaccounts -o jsonpath='{.items[*].metadata.name}' -n kube-system
	$tillerServiceAccountExists = $serviceAccounts.Contains('tiller')
	
	if($tillerServiceAccountExists -eq $false) {
		kubectl create serviceaccount tiller --namespace kube-system
		$msg = "Service account 'tiller' has been created."

		$clusterRoleBindingExists = Check-Cluster-Role-Binding -Name "tiller"
		if($clusterRoleBindingExists -eq $false) {
			kubectl create clusterrolebinding tiller --clusterrole cluster-admin --serviceaccount=kube-system:tiller
			$msg += " Cluster role binding 'tiller' has been created."
		}
	} else {
		$msg = "Service account 'tiller' already exists. Cluster role binding 'tiller' already exists."
	}

	Write-Host $msg
}

function Create-AKS-Namespace {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$AksNamespaceName
	)
	
	$namespaces = kubectl get namespaces -o jsonpath='{.items[*].metadata.name}' 
	$namespaceExists = $namespaces.Contains($AksNamespaceName)

	if($namespaceExists -eq $false) {
		kubectl create namespace $AksNamespaceName
		$msg = "Namespace '$AksNamespaceName' has been created."

		$namespaceClusterRoleBindingExists = Check-Cluster-Role-Binding -Name "default-view"
		if($namespaceClusterRoleBindingExists -eq $false) {
			kubectl create clusterrolebinding default-view --clusterrole=view --serviceaccount=gmca:default
			$msg += " Namespace role binding 'default-view' has been created."
		}
	} else {
		$msg = "Namespace '$AksNamespaceName' already exists. Namespace role binding 'default-view' already exists."
	}

	Write-Host $msg
}

function Check-Cluster-Role-Binding {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$Name
	)

	$clusterRoleBindings = kubectl get clusterrolebinding -o jsonpath='{.items[*].metadata.name}'
	$clusterRoleBindingExists = $clusterRoleBindings.Contains($Name)
	return $clusterRoleBindingExists
}

function Assign-AKS-ACR-Premissions {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$RgName,
		[Parameter(Mandatory=$true)][String]$AksName,
		[Parameter(Mandatory=$true)][String]$AcrName,
		[Parameter(Mandatory=$true)][String]$AcrPushSpName,
		[Parameter(Mandatory=$true)][String]$AcrPushSpSecretName,
	    [Parameter(Mandatory=$true)][String]$KeyVaultName
	)

	$clientId = $(az aks show -g $RgName -n $AksName --query "servicePrincipalProfile.clientId" -o tsv)
	$acrId = $(az acr show -g $RgName -n $AcrName --query "id" -o tsv)
	
	Assign-Pull-Role -ClientId $clientId -AcrId $acrId -AksName $AksName -AcrName $AcrName
	Assign-Push-Role -AcrId $acrId -AksName $AksName -AcrName $AcrName -AcrPushSpName $AcrPushSpName -AcrPushSpSecretName $AcrPushSpSecretName -KeyVaultName $KeyVaultName
}

function Assign-Pull-Role {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$ClientId,
		[Parameter(Mandatory=$true)][String]$AcrId,
		[Parameter(Mandatory=$true)][String]$AksName,
		[Parameter(Mandatory=$true)][String]$AcrName
	)

	Write-Host "Assign role 'acrpull' to AKS '$ClientId' for ACR scope '$AcrId'."
	az role assignment create --assignee $ClientId --role acrpull --scope $AcrId
	Write-Host "Role 'acrpull' has been assigned to AKS '$AksName' for ACR scope '$AcrName'."
}

function Assign-Push-Role {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$AcrId,
		[Parameter(Mandatory=$true)][String]$AksName,
		[Parameter(Mandatory=$true)][String]$AcrName,
		[Parameter(Mandatory=$true)][String]$AcrPushSpName,
		[Parameter(Mandatory=$true)][String]$AcrPushSpSecretName,
	    [Parameter(Mandatory=$true)][String]$KeyVaultName
	)

	$sp = Get-ServicePrincipal -Name $AcrPushSpName

	if($sp -eq $null) {
		Create-AcrPush-ServicePrincipal -Name $AcrPushSpName -AcrId $AcrId -AcrPushSpSecretName $AcrPushSpSecretName -KeyVaultName $KeyVaultName
		$msg = "Service principal '$AcrPushSpName' has been created. Secret has been assigned."
	} else {
		$msg = "Service principal '$AcrPushSpName' already exist."

		$secret = Get-Secret -SecretName $AcrPushSpSecretName -KeyVaultName $KeyVaultName
		if($secret -eq $null) {
			az ad sp delete --id $sp.objectId
			Create-AcrPush-ServicePrincipal -Name $AcrPushSpName -AcrId $AcrId -AcrPushSpSecretName $AcrPushSpSecretName -KeyVaultName $KeyVaultName
			$msg = "Service principal '$AcrPushSpName' has been re-created. Secret has been assigned."
		}
	}
	
	Write-Host $msg
}

function Get-ServicePrincipal {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$Name
	)

	$spJson = az ad sp list --display-name $Name
	$sp = $spJson | ConvertFrom-Json
	return $sp
}

function Create-AcrPush-ServicePrincipal {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$Name,
		[Parameter(Mandatory=$true)][String]$AcrId,
		[Parameter(Mandatory=$true)][String]$AcrPushSpSecretName,
	    [Parameter(Mandatory=$true)][String]$KeyVaultName
	)

	$password = $(az ad sp create-for-rbac -n $Name --scopes $AcrId --role acrpush --query password -o tsv)
	az keyvault secret set --name $AcrPushSpSecretName --vault-name $KeyVaultName --value $password
}

function Get-Secret {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$SecretName,
		[Parameter(Mandatory=$true)][String]$KeyVaultName
	)

	try {
		$secretJson = az keyvault secret show --vault-name $KeyVaultName --name $SecretName
		$secret = $secretJson | ConvertFrom-Json
		return $secret
	} catch {
		return $null
	}
}

Configure-AKS -RgName $RgName -AksName $AksName -AcrName $AcrName -AksNamespaceName $AksNamespaceName -AcrPushSpName $AcrPushSpName -AcrPushSpSecretName $AcrPushSpSecretName -KeyVaultName $KeyVaultName