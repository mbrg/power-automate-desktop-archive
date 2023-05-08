<#
.SYNOPSIS
    A script that accepts a directory path as input, iterates over all .exe files in the directory,
    and outputs a table with information on whether they are signed, the signer's information,
    and the file version.

.DESCRIPTION
    This PowerShell script takes a directory path as input and scans all .exe files within the directory
    and its subdirectories. For each .exe file, it checks if it is signed and outputs the signer's
    information, as well as the file version, in a formatted table.

.USAGE
    Save the script as Get-ExeSignatureInfo.ps1.
    Open PowerShell and navigate to the folder where you saved the script.
    Run the script by providing the directory path as a parameter:
    .\Get-ExeSignatureInfo.ps1 -DirectoryPath "C:\path\to\directory"
#>

param(
    [Parameter(Mandatory=$true)]
    [string]$DirectoryPath
)

if (-not (Test-Path -Path $DirectoryPath -PathType Container)) {
    Write-Host "Invalid directory path provided. Please ensure the path exists."
    exit 1
}

$exeFiles = Get-ChildItem -Path $DirectoryPath -Filter "*.exe" -Recurse

$signCheckResults = @()

foreach ($exe in $exeFiles) {
    $isSigned = $false
    $signer = $null
    $fileVersion = $null

    try {
        $signature = Get-AuthenticodeSignature -FilePath $exe.FullName
        $fileVersion = (Get-ItemProperty -Path $exe.FullName).VersionInfo.FileVersion

        if ($signature.Status -eq "Valid") {
            $isSigned = $true
            $signer = $signature.SignerCertificate.Subject
        }
    }
    catch {
        Write-Host "Error processing file: $($exe.FullName)"
    }

    $signCheckResults += [PSCustomObject]@{
        "FileName" = $exe.Name
        "IsSigned" = $isSigned
        "Signer" = $signer
        "FileVersion" = $fileVersion
    }
}

$signCheckResults | Format-Table -AutoSize
