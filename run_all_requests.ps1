# Путь к вашему файлу с запросами
$httpFilePath = ".\test_data.http"
$content = Get-Content -Path $httpFilePath -Raw

# Разделяем файл на отдельные запросы по разделителю "###"
$requests = $content -split '###'

foreach ($request in $requests) {
    # Пропускаем пустые блоки
    if ([string]::IsNullOrWhiteSpace($request)) { continue }

    # Извлекаем метод и URL
    if ($request -match '(?m)^\s*(GET|POST|PUT|DELETE)\s+(\S+)') {
        $method = $matches[1]
        $url = $matches[2]
        
        # Ищем тело запроса (JSON)
        $body = $null
        if ($request -match '(?s)\r?\n\r?\n(.+)') {
            $body = $matches[1].Trim()
        }

        Write-Host "Выполняется: $method $url" -ForegroundColor Cyan
        
        $params = @{
            Uri = $url
            Method = $method
        }

        if ($body) {
            $params['Body'] = $body
            $params['ContentType'] = 'application/json'
        }

        try {
            $response = Invoke-RestMethod @params
            Write-Host "Успешно! Ответ: $($response | ConvertTo-Json -Compress)" -ForegroundColor Green
        } catch {
            Write-Host "Ошибка: $($_.Exception.Message)" -ForegroundColor Red
        }
        Write-Host "----------------------------------------"
    }
}