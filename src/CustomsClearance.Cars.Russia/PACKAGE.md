### CustomsClearance.Cars.Russia

Библиотека для расчета таможенных пошлин в России

## Установка

Чтобы установить библиотеку, выполните:

```shell
dotnet add package CustomsClearance.Cars.Russia
```

## Как пользоваться

### Пример расчёта таможенных пошлин для авто

```csharp
var carCustomsClearanceCost = CarCustomsClearance
    .Create(
        importerType: ImporterType.LegalEntity, 
        carAge: 6, 
        engineType: EngineType.Diesel, 
        enginePower: 150, 
        engineVolume: 1800, 
        carPrice: 2000000, 
        euroExchangeRate: 102.9215M)
    .Calculate();

Console.WriteLine($"Customs Clearance Cost: {carCustomsClearanceCost.Total}");
```

## Лицензия

Проект распространяется под лицензией MIT.  
Подробнее см. файл [LICENSE](LICENSE).

## Контакты

Разработка: [5G0d](https://github.com/5G0D)  
Обратная связь: [t.me@N5G0d](https://t.me/n5g0d)  