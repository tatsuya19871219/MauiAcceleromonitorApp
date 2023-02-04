# MauiAcceleromonitorApp

An application that displays the temporal graph of accelaration data aquired via the pure functionality of MAUI.

## Files edited
 - CustomComponents/
     - [TemporalGraph.cs](./CustomComponents/TemporalGraph.cs)
 - [AccelarationRecorder.cs](./AccelarationRecorder.cs)
 - [MainPage.xaml](./MainPage.xaml)
 - [MainPage.xaml.cs](./MainPage.xaml.cs)

## What I learnt fron the project

- How to get information from device sensors
- How to use Custom Namespace Scheme
- How to use GraphicalView
- How to update UI by Task
- Role of Namespace in XAML
- How to support AppTheme (dark theme/light theme)

### UI update by Task

Combining Task (System.Threading.Tasks) and while-true loop, we can provide iteration processes for updating Data and/or GUI, like as timer callbacks.

```csharp
async Task UpdateXYZGraphs(TemporalGraph graphx, TemporalGraph graphy, TemporalGraph graphz, AccelarationRecorder recorder)
{
    while (true)
    {
        Vector3 acc = recorder.GetCurrentData();

        graphx.FeedData(acc.X);
        graphy.FeedData(acc.Y);
        graphz.FeedData(acc.Z);

        graphx.Invalidate();
        graphy.Invalidate();
        graphz.Invalidate();

        await Task.Delay(_samplingInterval);
    }
}
```

### Responsive design based on system theme

We can obtain the current theme (Dark/Light) of a device by Application.Current.RequestedTheme property. Based on the value, responsive color setting can be implimented for instance. 

```xml
<ContentPage.Resources>
    <Color x:Key="LightAxesColor">Black</Color>
    <Color x:Key="DarkAxesColor">White</Color>
</ContentPage.Resources>
```

```csharp
if (Application.Current.RequestedTheme == AppTheme.Dark)
{
    color = Resources["DarkAxesColor"] as Color;
}
else
{
    color = Resources["LightAxesColor"] as Color;
}
```
