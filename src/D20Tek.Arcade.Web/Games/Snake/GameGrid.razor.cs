﻿using D20Tek.Arcade.Web.Games.Snake.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.Snake;

public partial class GameGrid
{
    private SnakeGameEngine? _engine;

    [Parameter]
    public int Rows { get; set; }

    [Parameter]
    public int Columns { get; set; }

    [Parameter]
    public EventCallback<int> GameEnded { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var dotNetRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("addKeyListener", dotNetRef);

            _engine = new SnakeGameEngine(Rows, Columns, StateHasChanged);
            await _engine!.RunGameAsync();
            await GameEnded.InvokeAsync(_engine.GetScore());
        }
    }

    [JSInvokable]
    public void HandleKeydown(string key)
    {
        if (key == "ArrowUp") _engine?.ChangeDirection(Direction.Up);
        if (key == "ArrowDown") _engine?.ChangeDirection(Direction.Down);
        if (key == "ArrowLeft") _engine?.ChangeDirection(Direction.Left);
        if (key == "ArrowRight") _engine?.ChangeDirection(Direction.Right);
    }

    private string? GetCellStyle(int row, int col)
    {
        if (_engine is null) return null;

        var rotation = _engine.GetHeadRotation(row, col);
        var gridImage = _engine.GetGridImage(row, col);

        return !string.IsNullOrEmpty(gridImage)
            ? $"background-image: url('{gridImage}'); background-size: cover; transform: rotate({rotation}deg)"
            : null;
    }
}
