$urls = @(
    [pscustomobject]@{url = 'https://pkgs.dev.azure.com/CluedIn-io/_packaging/release/nuget/v3/index.json'; name='release'},
    [pscustomobject]@{url = 'https://pkgs.dev.azure.com/CluedIn-io/_packaging/develop/nuget/v3/index.json'; name='develop'},
    [pscustomobject]@{url = 'https://pkgs.dev.azure.com/CluedIn-io/_packaging/CluedIn/nuget/v3/index.json';name='AzurePipelines'},
    [pscustomobject]@{url = 'https://pkgs.dev.azure.com/CluedIn-io/_packaging/CluedIn-Crawlers/nuget/v3/index.json'; name='CluedIn-Crawlers'}
)

foreach ($url in $urls) {
	$output = Credentialprovider.VSS -U $url.url
    $json = $output | ConvertFrom-Json | Select-Object -Property * 

    nuget sources update -name $url.name -username $json.username -password $json.password -source $url.url
}

