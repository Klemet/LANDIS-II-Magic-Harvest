# LANDIS-II support library GitHub URL
$master = "https://github.com/LANDIS-II-Foundation/Support-Library-Dlls-v8/raw/main/"


#************************************************
# LANDIS-II support library dependencies
# Modify here when any dependencies changed 

$dlls = "Landis.Library.HarvestManagement-v5.dll",
"Landis.Library.SiteHarvest-v3.dll"
# WARNING : You need the BiomassHarvest v7 dll, but it's not in the support library repo yet.
# You'll need to extract it from the v7 installer.
# "Landis.Extension.BiomassHarvest-v7.dll"

#************************************************


# LANDIS-II support libraries download
$current = Get-Location
$outpath = $current.toString() + "/"

try {
	ForEach ($item in $dlls) {
		$dll = $outpath + $item
		$url = $master + $item
		[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
		Invoke-WebRequest -uri $url -Outfile $dll
		($dll).split('/')[-1].toString() + "------------- downloaded"
	}
	"`n***** Download complete *****`n"
}
catch [System.Net.WebException],[System.IO.IOException]{
	"Unable to download file from " + $item.toString()
}
catch {
	"An error occurred."
}

