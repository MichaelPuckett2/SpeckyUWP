# SpeckyUWP
Reactive model package to help develop MVVM / reactive oriented models.

Example of using VisualStateCommand - No code behind required.

<Page x:Name="page"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
      xmlns:SpeckyInteraction="using:Specky.UI.Interaction"
      x:Class="SpecktUI_UWP_Tester.MainPage"
      mc:Ignorable="d">

    <Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
        <VisualStateManager.VisualStateGroups>
            <VisualStateGroup x:Name="DialogGroup">
                <VisualState x:Name="ShowDialogState">
                    <VisualState.Setters>
                        <Setter Target="textBlock.Visibility"
                                Value="Visible" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="HideDialogState">
                    <VisualState.Setters>
                        <Setter Target="textBlock.Visibility"
                                Value="Collapsed" />
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateManager.VisualStateGroups>

        <Grid Name="textBlock"
                    Visibility="Collapsed"
              Width="300"
              Height="120">
            <Rectangle Fill="Red" />
            <TextBlock Text="{Binding (SpeckyInteraction:VisualStateCommand.DataContext), ElementName=page}"
                       HorizontalAlignment="Center"
                       VerticalAlignment="Center" />
        </Grid>

        <StackPanel VerticalAlignment="Center">
            <Button Content="Show Dialog 1"
                    SpeckyInteraction:VisualStateCommand.StateControl="page"
                    SpeckyInteraction:VisualStateCommand.VisualState="ShowDialogState"
                    SpeckyInteraction:VisualStateCommand.DataContext="Button 1"/>

            <Button Content="Show Dialog 2"
                    SpeckyInteraction:VisualStateCommand.StateControl="page"
                    SpeckyInteraction:VisualStateCommand.VisualState="ShowDialogState"
                    SpeckyInteraction:VisualStateCommand.DataContext="Button 2" />

            <Button Content="Close Dialog"
                    SpeckyInteraction:VisualStateCommand.StateControl="page"
                    SpeckyInteraction:VisualStateCommand.VisualState="HideDialogState" />

            <CheckBox Content="Show or Hide"
                      SpeckyInteraction:VisualStateCommand.StateControl="page"
                      SpeckyInteraction:VisualStateCommand.CheckedVisualState="ShowDialogState"
                      SpeckyInteraction:VisualStateCommand.NotCheckedVisualState="HideDialogState"
                      SpeckyInteraction:VisualStateCommand.DataContext="CheckBox" />
        </StackPanel>

    </Grid>
</Page>

