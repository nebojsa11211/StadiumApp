[xml]$en = Get-Content SharedResources.en.resx
[xml]$hr = Get-Content SharedResources.hr.resx

$untranslated = @()

foreach ($dataEl in $en.root.data) {
    $key = $dataEl.name
    $enVal = $dataEl.value
    $hrEl = $hr.root.data | Where-Object { $_.name -eq $key }
    if ($hrEl) {
        $hrVal = $hrEl.value
        if ($enVal -eq $hrVal) {
            $untranslated += @{ Key = $key; Value = $enVal }
        }
    }
}

Write-Host "Untranslated keys (HR = EN):" -ForegroundColor Yellow
$untranslated | ForEach-Object { Write-Host "  $($_.Key): $($_.Value)" }
Write-Host "`nTotal untranslated: $($untranslated.Count)"
