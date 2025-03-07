﻿using D20Tek.Arcade.Web.Games.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.Arcade.Web.Games.Tetris;

public partial class TetrisGrid
{
    private TetrisGameEngine _engine;
    private string? _levelText;
    private bool _hasLevelText => !string.IsNullOrEmpty(_levelText);

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

            _engine = new TetrisGameEngine(Rows, Columns, StateHasChanged, OnLevelChanged);
            await _engine.RunGameAsync();
            await GameEnded.InvokeAsync(_engine.GetScore());
        }
    }

    [JSInvokable]
    public void HandleKeydown(string key)
    {
        if (_engine is null || _engine.GameOver) return;

        if (key == KnownKeys.ArrowUp) _engine.Rotate();
        if (key == KnownKeys.Z || key == KnownKeys.z) _engine.RotateCounter();
        if (key == KnownKeys.ArrowDown) _engine.MoveDown();
        if (key == KnownKeys.ArrowLeft) _engine.MoveLeft();
        if (key == KnownKeys.ArrowRight) _engine.MoveRight();
        if (key == KnownKeys.Space) _engine.MoveDrop();

        _engine.Draw();
    }

    private string? GetCellStyle(int row, int col)
    {
        if (_engine is null) return null;

        var tileImage = _engine.GetTileImage(row, col);
        return !string.IsNullOrEmpty(tileImage)
                    ? $"background-image: url('{tileImage}'); background-size: cover;"
                    : null;
    }

    private async Task OnLevelChanged(int newLevel)
    {
        _levelText = $"MOVING TO LEVEL {newLevel} ...";
        StateHasChanged();
        await Task.Delay(200);
        _levelText = null;
    }
}
