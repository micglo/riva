Param(
	[Parameter(Mandatory=$true)][String]$ProjectName,
	[Parameter(Mandatory=$true)][String]$Stage,
	[Parameter(Mandatory=$true)][String]$LocationShort,
	[Parameter(Mandatory=$true)][String]$VsoResourceGroupVariableName
)

function Compose-Azure-Resource-Group-Name {
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)][String]$ProjectName,
		[Parameter(Mandatory=$true)][String]$Stage,
		[Parameter(Mandatory=$true)][String]$LocationShort,
		[Parameter(Mandatory=$true)][String]$VsoResourceGroupVariableName
	)
	
	Write-Host "Compose-Azure-Resource-Group-Name"
	
	$resourceGroupName = $ProjectName + "-" + $Stage + "-" + $LocationShort

	Write-Host  "resourceGroupName: " $resourceGroupName 
	Write-Host "##vso[task.setvariable variable=$VsoResourceGroupVariableName;]$resourceGroupName"
}

Compose-Azure-Resource-Group-Name -ProjectName $ProjectName -Stage $Stage -LocationShort $LocationShort -VsoResourceGroupVariableName $VsoResourceGroupVariableName