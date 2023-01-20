using System.Management.Automation;
using System.Runtime.InteropServices;

namespace Ze.PowerShell.Core;

public class SetGlobalCompatabilityVariableCmdlet : PSCmdlet
{
    protected override void ProcessRecord()
    {
        this.InvokeCommand.InvokeScript(""" 

if ($PSVersionTable.PSVersion.Major -lt 6)
{ 
    $var = Get-Variable -Name "IsWindows" -EA SilentlyContinue

    if (!$var)
    {
        Set-Variable -Name "IsCoreClr" -Value $false -Scope Global -Force -Option Constant
        $platformString = [Environment]::OSVersion.Platform
        switch ($platformString)
        {
            "Win32NT"
            {  
                Set-Variable -Name "IsWindows" -Value $true -Scope Global -Force -Option Constant
                Set-Variable -Name "IsLinux" -Value $false -Scope Global -Force -Option Constant
                Set-Variable -Name "IsMacOS" -Value $false -Scope Global -Force -Option Constant
            }
            "MacOSX"
            {
                Set-Variable -Name "IsWindows" -Value $false -Scope Global -Force -Option Constant
                Set-Variable -Name "IsLinux" -Value $false -Scope Global -Force -Option Constant
                Set-Variable -Name "IsMacOS" -Value $true -Scope Global -Force -Option Constant
            }   
            "Unix" 
            {
                Set-Variable -Name "IsWindows" -Value $false -Scope Global -Force -Option Constant
                Set-Variable -Name "IsLinux" -Value $true -Scope Global -Force -Option Constant
                Set-Variable -Name "IsMacOS" -Value $false -Scope Global -Force -Option Constant
            }     
            Default 
            {
                Set-Variable -Name "IsWindows" -Value $true -Scope Global -Force -Option Constant
                Set-Variable -Name "IsLinux" -Value $false -Scope Global -Force -Option Constant
                Set-Variable -Name "IsMacOS" -Value $false -Scope Global -Force -Option Constant
            }
        }
    }
}

$var = Get-Variable -Name "IsWindowsServer" -EA SilentlyContinue
if (!$var)
{
    $isServer = $false;
    if ($null -ne (Get-Command Get-CimInstance -EA SilentlyContinue))
    {
        $osInfo = Get-CimInstance -ClassName Win32_OperatingSystem -EA SilentlyContinue
        if ($osInfo -and $osInfo.ProductType -ne 1)
        {
            $isServer = $true;
        }
    } 

    Set-Variable -Name "IsWindowsServer" -Value $isServer -Scope Global -Force -Option Constant
}

$var = Get-Variable -Name "IsOs64Bit" -EA SilentlyContinue
if (!$var)
{
    $osArch = [System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture;
    $is64Bit = $osArch -eq "z64" -or $osArch -eq "Arm64";
    Set-Variable -Name "IsOs64Bit -Value $is64Bit -Scope Global -Force -Option Constant
}

$var = Get-Variable -Name "IsProcess64Bit" -EA SilentlyContinue
if (!$var)
{
    $arch = [System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture;
    $is64Bit = $arch -eq "z64" -or $arch -eq "Arm64";
    Set-Variable -Name "IsProcess64Bit -Value $is64Bit -Scope Global -Force -Option Constant
}

""");
    }
}